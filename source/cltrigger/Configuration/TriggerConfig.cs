
namespace Corlib.Configuration {
    public class TriggerConfig {
        public TriggerType Type { get; set; }

        public string Trigger { get; set; }

        public ProcessConfig Process { get; set; }
    }
}