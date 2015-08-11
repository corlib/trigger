[![ci](https://ci.appveyor.com/api/projects/status/3sg6a90yoxj9lwer?svg=true)](https://ci.appveyor.com/project/ConditionalOperator/trigger) ![download](https://img.shields.io/github/downloads/corlib/trigger/latest/total.svg?style=flat-square) ![license](https://img.shields.io/github/license/corlib/trigger.svg?style=flat-square) ![pr](http://issuestats.com/github/corlib/trigger/badge/pr?style=flat-square) ![issues](http://issuestats.com/github/corlib/trigger/badge/issue?style=flat-square)
# Corlib Trigger Service
A Windows Service that performs actions when triggers are invoked.

## Installation

Install the Windows Service
`cltrigger.exe install`

Configure one or more triggers: `cltrigger.json`
```JSON
[
  {
    "triggerFile": "c:\\notepad.trigger",
    "processAction": {
      "fileName": "notepad.exe",
      "windowStyle": "minimized"
    }
  }
]
```

## Usage

Start the Windows Service
`NET START CLTRIGGER`

Create the trigger file
`c:\notepad.trigger`

Notepad should start minimized.

When notepad exists, the `c:\notepad.trigger` file should be automatically deleted.

## Trigger file
Similar to the “Folder Actions” feature of OS X. The service monitors for the existence of the file, and invokes the specified action. Subsequent actions are queued if another file is created while the action is executing.

Note: the trigger file is intended to be a zero-byte file specifically created to invoke this trigger action.

## Process action
Invokes the specified process with the specified arguments.

Options
- fileName
- arguments
- createNoWindow
- useShellExecute
- verb
- WindowStyle
- workingDirectory

## Future direction
- Additional triggers
  - UDP
  - authenticated HTTP
- Additional actions
  - Windows service operations
  - Windows tasks
