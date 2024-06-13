using Handlers;
using Player;
using UnityEngine;

namespace Level
{
    public abstract class BaseLevelHandler : MonoBehaviour, IPausable
    {
        protected SpawnHandler _spawnHandler;
        protected PlayerMovement _player;
        private PauseHandler _pauseHandler;
        
        protected void Construct(PauseHandler pauseHandler, SpawnHandler spawnHandler, PlayerMovement player)
        {
            _pauseHandler = pauseHandler;
            _pauseHandler.Register(this);
            
            _spawnHandler = spawnHandler;
            
            _player = player;
        }

        protected virtual void OnDestroy()
        {
            _pauseHandler.Unregister(this);
        }

        public abstract void Pause();

        public abstract void Unpause();
    }
}
