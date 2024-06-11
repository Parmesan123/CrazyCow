using Services;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSignalBus();
    }

    private void BindSignalBus()
    {
        Container
            .Bind<SignalBus>()
            .FromNew()
            .AsSingle();
    }
}