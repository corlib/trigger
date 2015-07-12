using Topshelf;

namespace Corlib {
    internal static class Program {
        static void Main (string[] args) {
            HostFactory.Run (hostConfigurator => {

                hostConfigurator.Service<Trigger.TriggerService> (serviceConfigurator => {
                    serviceConfigurator.ConstructUsing (hostSettings => new Trigger.TriggerService ());
                    serviceConfigurator.WhenStarted (service => service.Start ());
                    serviceConfigurator.WhenStopped (service => service.Stop ());
                    serviceConfigurator.WhenPaused (service => service.Pause ());
                    serviceConfigurator.WhenContinued (service => service.Continue ());
                });

                hostConfigurator.StartAutomaticallyDelayed ();
                hostConfigurator.SetDescription ("Launches configured processes when trigger files are detected");
                hostConfigurator.SetDisplayName ("Corlib Trigger Service");
                hostConfigurator.SetServiceName ("cltrigger");
            });
        }
    }
}