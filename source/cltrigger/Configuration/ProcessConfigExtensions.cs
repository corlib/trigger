using System.Diagnostics;

namespace Corlib.Configuration {
    public static class ProcessConfigExtensions {
        public static ProcessStartInfo ToProcessStartInfo (this ProcessConfig processConfig) {
            var result = new ProcessStartInfo (
                processConfig.FileName, 
                processConfig.Arguments);

            if (null != processConfig.CreateNoWindow)
                result.CreateNoWindow = processConfig.CreateNoWindow.Value;

            if (null != processConfig.UseShellExecute)
                result.UseShellExecute = processConfig.UseShellExecute.Value;

            if (null != processConfig.Verb)
                result.Verb = processConfig.Verb;
            
            if (null != processConfig.WindowStyle)
                result.WindowStyle = processConfig.WindowStyle.Value;

            if (null != processConfig.WorkingDirectory)
                result.WorkingDirectory = processConfig.WorkingDirectory;

            return result;
        }
    }
}