using System.Collections.Generic;
using Entities;
using Handlers;
using UI;
using UnityEngine;

namespace Level
{
    public abstract class BaseLevelHandler : MonoBehaviour
    {
        [SerializeField] private BoxCollider _levelBounds;
        
        protected List<Box> _boxesOnField;
        protected List<Vase> _vasesOnField;
        protected CoinSpawner _coinSpawner;
        protected PlayerBehavior _player;
        
        private SpawnHandler _spawnHandler;
        
        protected void Construct(SpawnHandler spawnHandler, CoinSpawner coinSpawner, PlayerBehavior player)
        {
            _spawnHandler = spawnHandler;

            _coinSpawner = coinSpawner;

            _player = player;
        }

        protected virtual void Awake()
        {
            _boxesOnField = new List<Box>();
            _vasesOnField = new List<Vase>();
        }

        protected virtual void OnDestroy()
        {
            _boxesOnField.Clear();
            
            _vasesOnField.Clear();
        }

        protected T EntitySpawn<T>(Transform avoidObject = null) where T : ISpawnable
        {
            T entityInstance = _spawnHandler.TrySpawnAndPlaceEntity<T>(_levelBounds, avoidObject);

            if (entityInstance is not IDestroyable convertable) 
                return entityInstance;
            
            convertable.OnDestroyEvent += EntityDestroy;
                
            if (convertable is Box box)
            {
                _boxesOnField.Add(box);
                return entityInstance;
            }

            if (convertable is Vase vase)
            {
                _vasesOnField.Add(vase);
                return entityInstance;
            }

            return entityInstance;

            void EntityDestroy(IDestroyable destroyable)
            {
                destroyable.OnDestroyEvent -= EntityDestroy;

                if (destroyable is Box convertableBox)
                {
                    _boxesOnField.Remove(convertableBox);
                    return;
                }

                if (destroyable is Vase convertableVase)
                    _vasesOnField.Remove(convertableVase);
            }
        }
    }
}
