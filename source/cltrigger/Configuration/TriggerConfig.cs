
namespace Corlib.Configuration {
    public class TriggerConfig {

        public string TriggerFile { get; set; }

        public ProcessAction ProcessAction { get; set; }

        public TriggerType GetTriggerType () {
            if (!string.IsNullOrWhiteSpace (TriggerFile))
                return TriggerType.File;

            return TriggerType.Undefined;
        }

        public ActionType GetActionType () {
            if (null != ProcessAction)
                return ActionType.Process;

            return ActionType.Undefined;
        }
    }
}