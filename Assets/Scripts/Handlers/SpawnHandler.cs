using System;
using System.Linq;
using InteractableObject;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class SpawnHandler
    {
        private const int BASE_POOL_SIZE = 30;

        private SpawnHandlerData _spawnHandlerData;

        private Pool<Box> _boxPool;
        private Pool<Vase> _vasePool;
        private Pool<Portal> _portalPool;

        [Inject]
        public void Construct(BoxFactory boxFactory, VaseFactory vaseFactory, PortalFactory portalFactory, SpawnHandlerData spawnHandlerData)
        {
            GameObject boxParent = new GameObject("Boxes");
            _boxPool = new Pool<Box>(BASE_POOL_SIZE, boxFactory, boxParent.transform);

            GameObject vaseParent = new GameObject("Vases");
            _vasePool = new Pool<Vase>(BASE_POOL_SIZE / 3, vaseFactory, vaseParent.transform);

            GameObject portalParent = new GameObject("Portals");
            _portalPool = new Pool<Portal>(BASE_POOL_SIZE / 10, portalFactory, portalParent.transform);
                
            _spawnHandlerData = spawnHandlerData;
        }

        public T TrySpawnAndPlaceEntity<T>(BoxCollider levelCollider, Transform avoidObject = null) where T : ISpawnable
        {
            Vector3 randomPoint = CalculateValidPoint(levelCollider.bounds, 10f, avoidObject);

            Box box = _boxPool.ObjectGetFreeOrCreate();
            if (box is T convertableBox)
            {
                box.transform.position = randomPoint;
                convertableBox.Spawn();
                
                return convertableBox;
            }

            Vase vase = _vasePool.ObjectGetFreeOrCreate();
            if (vase is T convertableVase)
            {
                vase.transform.position = randomPoint;
                convertableVase.Spawn();

                for (int i = 0; i < _spawnHandlerData.BoxesWithVaseCount; i++)
                {
                    Box freeCrate = _boxPool.ObjectGetFreeOrCreate();

                    Vector3 cratePosition = GetRandomPointInCircle(levelCollider.bounds, _spawnHandlerData.VaseRadiusThreshold, vase.transform);
                    freeCrate.transform.position = cratePosition;
                    freeCrate.Spawn();
                }

                return convertableVase;
            }
            
            Portal portal = _portalPool.ObjectGetFreeOrCreate();
            if (portal is T convertablePortal)
            {
                portal.transform.position = randomPoint;
                convertablePortal.Spawn();
                
                return convertablePortal;
            }

            throw new Exception("Can't form entity from spawn request");
        }
        
        private Vector3 GetRandomPointInCircle(Bounds mapBounds, float radius, Transform center)
        {
            for (int i = 0; i < 200; i++)
            {
                Vector3 randomPointInCircle = center.GetRandomPointOnCircle(radius);
                
                Collider[] colliders = Physics.OverlapSphere(randomPointInCircle, radius);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;
                
                return randomPointInCircle;
            }

            throw new Exception("Can't get valid point on circle");
        }

        private Vector3 CalculateValidPoint(Bounds mapBounds, float offset, Transform avoid = null)
        {
            for (int i = 0; i < 200; i++)
            {
                Vector3 randomPoint = GetRandomPointInCollider(mapBounds, offset);

                if (avoid != null)
                    return randomPoint;
                
                Collider[] colliders = Physics.OverlapSphere(randomPoint, _spawnHandlerData.SpawnRadiusThreshold);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;
                
                return randomPoint;
            }

            return Vector3.zero;
        }

        private Vector3 GetRandomPointInCollider(Bounds mapBounds, float offset)
        {
            for (int i = 0; i < 200; i++)
            {
                float xComponent = Random.Range(mapBounds.min.x, mapBounds.max.x);
                float zComponent = Random.Range(mapBounds.min.z, mapBounds.max.z);
                Vector3 convertPosition = new Vector3(xComponent, 0, zComponent);
                Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
            
                Vector3 resultPosition = convertPosition + randomOffset;

                Collider[] colliders = Physics.OverlapSphere(resultPosition, _spawnHandlerData.MinRangeBetweenObjects);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;

                return resultPosition;
            }

            return Vector3.zero;
        }
    }
}