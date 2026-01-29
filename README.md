# PepperDash Plugin - Symetrix Composer

## Notes
1. Tested on-site with a Symetrix Radius NX 12x8 using TCP/IP control.
2. Unit tested did not implement a phone dialer (VoIP or POTS), currently this portion of the plugin is untested.

### DSP Control Methods Supported
```json
"method": "com",
"method": "tcpip"
```
### DSP Configuraton

```json
{
    "key": "dsp1",
    "uid": 41,
    "type": "SymetrixDsp",
    "name": "Symetrix",
    "group": "dsp",
    "properties": {
        "control": {
            "tcpSshProperties": {
                "address": "192.168.1.31",
                "port": 48631,
                "username": "",
                "password": "",
                "autoReconnect": true,
                "autoReconnectIntervalMs": 5000
            },
            "method": "tcpip",
            "controlPortDevKey": "processor"
        },
        "levelControlBlocks": {
            "fader1": {
                "label": "A Privacy",
                "levelControlId": 207,
                "muteControlId": 307,
                "disabled": false,
                "isMic": true,
                "unmuteOnVolChange": false,
                "userMinimum": -72,
                "userMaximim": 12,
                "faderMinimum": -72,
                "faderMaximum": 12,
                "increment": 2,
                "permissions": 0
            },
            "fader2": {
                "label": "A Room",
                "levelControlId": 216,
                "muteControlId": 316,
                "disabled": false,
                "isMic": false,
                "unmuteOnVolChange": false,
                "userMinimum": -72,
                "userMaximim": 12,
                "faderMinimum": -72,
                "faderMaximum": 12,
                "increment": 2,
                "permissions": 0
            },
            "fader3": {
                "label": "A Microphones",
                "levelControlId": 201,
                "muteControlId": 301,
                "disabled": false,
                "isMic": true,
                "unmuteOnVolChange": false,
                "userMinimum": -72,
                "userMaximum": 12,
                "faderMinimum": -72,
                "faderMaximum": 12,
                "increment": 2,
                "permissions": 0
            },
            "fader4": {
                "label": "A Video Call",
                "levelControlId": 207,
                "muteControlId": 307,
                "disabled": false,
                "isMic": false,
                "unmuteOnVolChange": false,
                "userMinimum": -72,
                "userMaximum": 12,
                "faderMinimum": -72,
                "faderMaximum": 12,
                "increment": 2,
                "permissions": 0
            }
        },
        "presets": {
            "1": {
                "label": "A Default",
                "presetNumber": 1
            },
            "2": {
                "label": "B Default",
                "presetNumber": 2
            },
            "3": {
                "label": "C Default",
                "presetNumber": 2
            }
        },
        "dialerControlBlocks": {
            "dialer01": {
                "label": "Room A Dialer",
                "unitNumber": 1,
                "cardSlot": 1,
                "lineNumber": 1,
                "rxVolumeId": 0,
                "rxMuteId": 0,
                "isRingingId": 0,
                "isConnectedId": 0,
                "isOnHoldId": 0,
                "isBusyId": 0,
                "isDialingId": 0,
                "dialStringId": 0,
                "connectAndDisconnectId": 0,
                "rejectId": 0,
                "dndId": 0,
                "redialId": 0,
                "keypadBackspaceId": 0,
                "keypadClearId": 0,
                "keypad1Id": 0,
                "keypad2Id": 0,
                "keypad3Id": 0,
                "keypad4Id": 0,
                "keypad5Id": 0,
                "keypad6Id": 0,
                "keypad7Id": 0,
                "keypad8Id": 0,
                "keypad9Id": 0,
                "keypad0Id": 0,
                "keypadPoundId": 0,
                "keypadStarId": 0
            }
        }
    }
}
```


### DSP Bridge Configuration (Application)

```json
{
    "key": "dsp1-bridge",
    "uid": 42,
    "name": "Bridge Dsp",
    "group": "api",
    "type": "eiscApiAdvanced",
    "properties": {
        "control": {
            "tcpSshProperties": {
                "address": "127.0.0.2",
                "port": 0
            },
            "ipid": "B3",
            "method": "ipidTcp"
        },
        "devices": [
            {
                "deviceKey": "dsp1",
                "joinStart": 1
            }
        ]
    }
}
```
<!-- START Minimum Essentials Framework Versions -->

<!-- END Minimum Essentials Framework Versions -->
<!-- START Config Example -->
### Config Example

```json
{
    "key": "GeneratedKey",
    "uid": 1,
    "name": "GeneratedName",
    "type": "Dialer",
    "group": "Group",
    "properties": {
        "ClearDialstringWhenConnected": true,
        "RxVolumeId": 0,
        "RxMuteId": 0,
        "RxUserMaximum": 0,
        "RxUserMinimum": 0,
        "UnitNumber": 0,
        "CardSlot": 0,
        "LineNumber": 0,
        "IsRingingId": 0,
        "IsConnectedId": 0,
        "IsOnHoldId": 0,
        "IsBusyId": 0,
        "IsDialingId": 0,
        "DialStringId": 0,
        "ConnectAndDisconnectId": 0,
        "RejectId": 0,
        "DndId": 0,
        "RedialId": 0,
        "KeypadBackspaceId": 0,
        "KeypadClearId": 0,
        "Keypad1Id": 0,
        "Keypad2Id": 0,
        "Keypad3Id": 0,
        "Keypad4Id": 0,
        "Keypad5Id": 0,
        "Keypad6Id": 0,
        "Keypad7Id": 0,
        "Keypad8Id": 0,
        "Keypad9Id": 0,
        "Keypad0Id": 0,
        "KeypadPoundId": 0,
        "KeypadStarId": 0
    }
}
```
<!-- END Config Example -->
<!-- START Supported Types -->

<!-- END Supported Types -->
<!-- START Join Maps -->
### Join Maps

#### Digitals

| Join | Type (RW) | Description |
| --- | --- | --- |
| 201 | R | Mute Toggle |
| 401 | R | Mute On |
| 601 | R | Mute Off |
| 801 | R | Volume Up |
| 1001 | R | Volume Down |
| 1 | R | Is Online |
| 100 | R | Preset Recall Discrete |
| 101 | R | Preset Recall |
| 1 | R | Volume Up |
| 2 | R | Volume Down |
| 3 | R | Mute On |
| 4 | R | Mute Off |
| 5 | R | Mute Toggle |
| 1 | R | Call Incoming |
| 37 | R | Call Incoming |
| 38 | R | Call Incoming |
| 8 | R | End Call |
| 11 | R | Keypad Digits 0-9 |
| 12 | R | Keypad Digits 0-9 |
| 13 | R | Keypad Digits 0-9 |
| 14 | R | Keypad Digits 0-9 |
| 15 | R | Keypad Digits 0-9 |
| 16 | R | Keypad Digits 0-9 |
| 17 | R | Keypad Digits 0-9 |
| 18 | R | Keypad Digits 0-9 |
| 19 | R | Keypad Digits 0-9 |
| 20 | R | Keypad Digits 0-9 |
| 21 | R | Keypad * |
| 22 | R | Keypad # |
| 23 | R | Keypad Clear |
| 24 | R | Keypad Backspace |
| 25 | R | Keypad Dial and Feedback |
| 26 | R | Auto Answer On and Feedback |
| 27 | R | Auto Answer Off and Feedback |
| 28 | R | Auto Answer Toggle and On Feedback |
| 30 | R | On Hook Set and Feedback |
| 31 | R | Off Hook Set and Feedback |
| 33 | R | Do Not Disturb Toggle and Feedback |
| 34 | R | Do Not Disturb On Set and Feedback |
| 35 | R | Do Not Disturb Of Set and Feedback |

#### Analogs

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Volume |
| 201 | R | Fader mute type, level mute (0) or mic mute (1) |
| 401 | R | Fader controls, level & mute (0), level only (1), mute only (2) |
| 601 | R | Fader permissions, user & tech accessible (0), user only (1), tech only (2) |
| 1 | R | Volume |
| 2 | R | Mute Icon sets the fader mute icon, level mute (0) or mic mute (1) |
| 3 | R | Fader controls, level & mute (0), level only (1), mute only (2) |
| 4 | R | Fader permissions, user & tech accessible (0), user only (1), tech only (2) |

#### Serials

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Name |
| 101 | R | Preset Name |
| 1 | R | Name |
| 1 | R | Dial String Send and Feedback |
| 5 | R | Caller ID Number |
<!-- END Join Maps -->
<!-- START Interfaces Implemented -->
### Interfaces Implemented

- IBasicVolumeWithFeedback
- IHasPhoneDialing
- IOnline
- IHasFeedback
- IDspPreset
- ICommunicationMonitor
- IHasDspPresets
<!-- END Interfaces Implemented -->
<!-- START Base Classes -->
### Base Classes

- EssentialsBridgeableDevice
- EssentialsDevice
- ReconfigurableBridgableDevice
- JoinMapBaseAdvanced
<!-- END Base Classes -->
<!-- START Public Methods -->
### Public Methods

- public void SetVolume(ushort level)
- public void MuteOn()
- public void MuteOff()
- public void DoNotDisturbToggle()
- public void DoNotDisturbOn()
- public void DoNotDisturbOff()
- public void DialPhoneCall(string number)
- public void AcceptIncomingCall()
- public void RejectIncomingCall()
- public void EndPhoneCall()
- public void SendDtmfToPhone(string digit)
- public void SetDialString(string dialString)
- public void HandleKeypadPress(EKeypadKeys digit)
- public void SendDtmfToPhone(EKeypadKeys digit)
- public void VolumeUp(bool pressRelease)
- public void VolumeDown(bool pressRelease)
- public void MuteToggle()
- public void VolumeUp(bool pressRelease)
- public void VolumeDown(bool pressRelease)
- public void SetVolume(ushort level)
- public void MuteToggle()
- public void MuteOn()
- public void MuteOff()
- public void LinkToApplicationApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
- public void RecallPreset(IDspPreset preset)
<!-- END Public Methods -->
<!-- START Bool Feedbacks -->
### Bool Feedbacks

- PhoneOffHookFeedback
- MuteFeedback
- IsOnline
- MuteFeedback
- IsOnline
<!-- END Bool Feedbacks -->
<!-- START Int Feedbacks -->
### Int Feedbacks

- VolumeLevelFeedback
- VolumeLevelFeedback
<!-- END Int Feedbacks -->
<!-- START String Feedbacks -->
### String Feedbacks

- CallerIdNameFeedback
- CallerIdNumberFeedback
<!-- END String Feedbacks -->
