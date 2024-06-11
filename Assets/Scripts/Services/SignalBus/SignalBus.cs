using System.Collections.Generic;

namespace Services
{
    public class SignalBus
    {
        private readonly Dictionary<string, List<IBaseSignalReceiver>> _signalReceivers;

        public SignalBus()
        {
            _signalReceivers = new Dictionary<string, List<IBaseSignalReceiver>>();
        }
        
        public void Register<T>(ISignalReceiver<T> callback) where T: ISignal
        {
            string callbackKey = typeof(T).Name;
            if (!_signalReceivers.ContainsKey(callbackKey))
                _signalReceivers[callbackKey] = new List<IBaseSignalReceiver>();

            _signalReceivers[callbackKey].Add(callback);
        }
        
        public void Invoke<T>(T signal) where T : ISignal
        {
            if (!_signalReceivers.TryGetValue(typeof(T).Name, out var receivers))
                return;

            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] is not ISignalReceiver<T> receiver) 
                    continue;

                receiver.Receive(signal);
            }
        }

        public void Unregister<T>(ISignalReceiver<T> callback) where T: ISignal
        {
            if (!_signalReceivers.TryGetValue(typeof(T).Name, out var receivers))
                return;

            receivers.Remove(callback);
        }
    }
}