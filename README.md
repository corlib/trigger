# Corlib Trigger Service
A Windows Service that performs actions when triggers are invoked.

## Trigger file
Similar to the “Folder Actions” feature of OS X. The service monitors for the existence of the file, and invokes the specified action. Subsequent actions are queued if another file is created while the action is executing.

Note: the trigger file is intended to be a zero-byte file specifically created to invoke this trigger action.

## Process action
Invokes the specified process with the specified arguments.

## Example cltrigger.json
```JSON
[
  {
    "type": "file",
    "trigger": "c:\\notepad.trigger",
    "process": {
      "fileName": "notepad.exe",
      "windowStyle": "minimized"
    }
  }
]
```

## Future direction
- Additional triggers
- 	UDP
- 	authenticated HTTP
- Additional actions
-	service operations
-	Windows tasks