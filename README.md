[![Build status](https://ci.appveyor.com/api/projects/status/3sg6a90yoxj9lwer?svg=true)](https://ci.appveyor.com/project/ConditionalOperator/trigger) ![Pull requests](http://issuestats.com/github/corlib/trigger/badge/pr?style=flat) ![Issues](http://issuestats.com/github/corlib/trigger/badge/issue?style=flat)
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
