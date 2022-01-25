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