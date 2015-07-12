using System.Diagnostics;

namespace Corlib.Configuration {
    public class ProcessConfig {
        public string FileName { get; set; }

        public string Arguments { get; set; }

        public bool? CreateNoWindow { get; set; }

        public bool? UseShellExecute { get; set; }

        public string Verb { get; set; }

        public ProcessWindowStyle? WindowStyle { get; set; }

        public string WorkingDirectory { get; set; }
    }
}