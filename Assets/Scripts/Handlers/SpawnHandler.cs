﻿using System;
using System.Linq;
using Entities;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class SpawnHandler
    {
        private const int BASE_POOL_SIZE = 30;

        private readonly SpawnHandlerData _spawnHandlerData;

        private readonly Pool<Box> _boxPool;
        private readonly Pool<Vase> _vasePool;
        private readonly Pool<Portal> _portalPool;

        [Inject]
        private SpawnHandler(BoxFactory boxFactory, VaseFactory vaseFactory, PortalFactory portalFactory, SpawnHandlerData spawnHandlerData)
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
            Vector3 randomPoint = CalculateValidPoint(levelCollider.bounds, avoidObject);

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
                    Vector3 cratePosition = GetRandomPointInCircle(_spawnHandlerData.VaseRadiusThreshold, vase.transform);
                    
                    Box freeCrate = _boxPool.ObjectGetFreeOrCreate();
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
        
        private Vector3 GetRandomPointInCircle(float radius, Transform center)
        {
            for (int i = 0; i < 400; i++)
            {
                Vector3 randomPointInCircle = center.GetRandomPointOnCircle(radius);
                
                Collider[] colliders = Physics.OverlapSphere(randomPointInCircle, _spawnHandlerData.MinRangeBetweenObjects);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;
                
                return randomPointInCircle;
            }

            throw new Exception("Can't get valid point on circle");
        }

        private Vector3 CalculateValidPoint(Bounds mapBounds, Transform avoid = null)
        {
            for (int i = 0; i < 200; ++i)
            {
                Vector3 randomPoint = GetRandomPointInCollider(mapBounds);

                Collider[] colliders = Physics.OverlapSphere(randomPoint, _spawnHandlerData.MinRangeBetweenObjects);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;
                
                if (avoid is null)
                    return randomPoint;

                colliders = Physics.OverlapSphere(randomPoint, _spawnHandlerData.SpawnRadiusThreshold);
                if (colliders.Any(c => c.gameObject == avoid.gameObject))
                    continue;
                
                return randomPoint;
            }

            return Vector3.zero;
        }

        private Vector3 GetRandomPointInCollider(Bounds mapBounds)
        {
            for (int i = 0; i < 200; i++)
            {
                float xComponent = Random.Range(mapBounds.min.x, mapBounds.max.x);
                float zComponent = Random.Range(mapBounds.min.z, mapBounds.max.z);
                Vector3 convertPosition = new Vector3(xComponent, 0, zComponent);
            
                Collider[] colliders = Physics.OverlapSphere(convertPosition, _spawnHandlerData.MinRangeBetweenObjects);
                if (colliders.Any(c => c.TryGetComponent(out ISpawnable _)))
                    continue;

                return convertPosition;
            }

            return Vector3.zero;
        }
    }
}