using System.Linq;
using PepperDash.Core;
using PepperDash.Essentials.Core;

namespace PepperDashPluginSymetrixComposer.Utils
{
    public static class FeedbackUtils
    {
        public static void RegisterForFeedbackText<T>(this FeedbackCollection<T> feedbacks, IKeyed parent)
            where T : Feedback
        {
            const string valueUpdatedTemplate = "{key}: ValueUpdated | {value}";
            var feedbacksWithKey = feedbacks.Where(b => !string.IsNullOrEmpty(b.Key)).ToList();
            feedbacksWithKey.OfType<BoolFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.LogVerbose(parent, valueUpdatedTemplate, fb.Key, fb.BoolValue);
                });

            feedbacksWithKey.OfType<IntFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.LogVerbose(parent, valueUpdatedTemplate, fb.Key, fb.IntValue);
                });

            feedbacksWithKey.OfType<StringFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.LogVerbose(parent, valueUpdatedTemplate, fb.Key, fb.StringValue);
                });
        }
    }
}
