namespace Services
{
    public interface IBaseSignalReceiver
    {
        
    }

    public interface ISignalReceiver<in T> : IBaseSignalReceiver where T: ISignal
    {
        public void Receive(T signal);
    }
}