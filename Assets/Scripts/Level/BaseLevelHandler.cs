using Handlers;
using Player;
using Services;
using UnityEngine;
using Zenject;

namespace Level
{
    public abstract class BaseLevelHandler : MonoBehaviour, IPausable
    {
        protected SpawnHandler _spawnHandler;
        protected PlayerMovement _player;
        protected SignalBus _signalBus;
        private PauseHandler _pauseHandler;
        
        protected virtual void Construct(SpawnHandler spawnHandler, PlayerMovement player, SignalBus signalBus, PauseHandler pauseHandler)
        {
            _spawnHandler = spawnHandler;
            _player = player;
            _signalBus = signalBus;
            _pauseHandler = pauseHandler;
            
            _pauseHandler.Register(this);
        }

        private void OnDestroy()
        {
            _pauseHandler.Unregister(this);
        }

        public abstract void Pause();

        public abstract void Unpause();
    }
}
