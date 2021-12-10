using System.Linq;
using PepperDash.Core;
using PepperDash.Essentials.Core;

namespace SymetrixComposerEpi.Utils
{
    public static class FeedbackUtils
    {
        public static void RegisterForFeedbackText<T>(this FeedbackCollection<T> feedbacks, IKeyed parent)
            where T : Feedback
        {
            const string valueUpdatedTemplate = "{0}: ValueUpdated | {1}";
            var feedbacksWithKey = feedbacks.Where(b => !string.IsNullOrEmpty(b.Key)).ToList();
            feedbacksWithKey.OfType<BoolFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.Console(1, parent, valueUpdatedTemplate, fb.Key, fb.BoolValue);
                });

            feedbacksWithKey.OfType<IntFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.Console(1, parent, valueUpdatedTemplate, fb.Key, fb.IntValue);
                });

            feedbacksWithKey.OfType<StringFeedback>().ToList().ForEach(
                fb =>
                {
                    fb.OutputChange +=
                        (sender, args) => Debug.Console(1, parent, valueUpdatedTemplate, fb.Key, fb.StringValue);
                });
        }
    }
}