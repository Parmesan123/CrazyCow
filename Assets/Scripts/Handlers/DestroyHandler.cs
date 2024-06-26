using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using ModestTree;
using System;
using System.Threading.Tasks;
using Entities;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Handlers
{
	public class DestroyHandler : IDisposable
	{
		private readonly List<ParticleData> _modelParticles;
		private readonly BoxFactory _boxFactory;
		private readonly VaseFactory _vaseFactory;

		private class ParticleData
		{
			public GameObject Particle { get; }
			public float LifeTime;

			public ParticleData(GameObject particle, float lifeTime)
			{
				Particle = particle;
				LifeTime = lifeTime;
			}
		}
		
		[Inject]
		private DestroyHandler(BoxFactory boxFactory, VaseFactory vaseFactory)
		{
			_modelParticles = new List<ParticleData>();
			
			_boxFactory = boxFactory;
			_boxFactory.OnDestroyBoxEvent += DestroyEventAnimation;
			foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
				spawnedBox.OnDestroyEvent += DestroyEventAnimation;

			_vaseFactory = vaseFactory;
			_vaseFactory.OnDestroyVaseEvent += DestroyEventAnimation;
			foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
				spawnedVase.OnDestroyEvent += DestroyEventAnimation;
		}
		
		public void Dispose()
		{
			_boxFactory.OnDestroyBoxEvent -= DestroyEventAnimation;

			_vaseFactory.OnDestroyVaseEvent -= DestroyEventAnimation;
		}
		
		private void DestroyEventAnimation(IDestroyable destroyable)
		{
			if (destroyable is not DestroyBehavior convertableDestroyable)
				throw new Exception("Destroy request can't be processed in destroy handler");
			
			List<GameObject> particles = CreateParticle(convertableDestroyable.Model, convertableDestroyable.Data.CrossSectionMaterial);
			AddToList(particles, convertableDestroyable.Data.Lifetime);
			DestroyParticle();
		}

		private List<GameObject> CreateParticle(GameObject model, Material crossSection)
		{
			List<GameObject> finalParticle = new List<GameObject>();
			List<GameObject> temp = new List<GameObject>();

			GameObject parent = new GameObject("AnimationParent")
			{
				transform =
				{
					position = model.transform.position,
					rotation = model.transform.rotation
				},
			};
			GameObject copyModel = Object.Instantiate(model, parent.transform);

			finalParticle.Add(copyModel);

			for (int i = 0; i < 4; ++i)
			{
				Vector3 randomOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f),
					Random.Range(-0.2f, 0.2f));
				Vector3 position = model.transform.position + randomOffset;
				Vector3 direction = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * Vector3.left;

				foreach (GameObject piece in finalParticle)
				{
					GameObject[] pieces = piece.SliceInstantiate(position, direction, crossSection);

					if (pieces == null)
					{
						temp.Add(piece);
						continue;
					}

					foreach (GameObject particle  in pieces)
					{
						particle.transform.SetParent(parent.transform);
						particle.transform.localPosition = Vector3.zero;
					}
					
					temp.AddRange(pieces);
					Object.Destroy(piece);
				}

				finalParticle.Clear();
				finalParticle.AddRange(temp);
				temp.Clear();
			}

			foreach (GameObject piece in finalParticle)
			{
				Rigidbody rb = piece.AddComponent<Rigidbody>();
				piece.AddComponent<MeshCollider>().convex = true;
				rb.AddExplosionForce(200f, copyModel.transform.position, 2);
			}
			
			return finalParticle;
		}

		private void AddToList(List<GameObject> particles, float lifetime)
		{
			foreach (GameObject particle in particles)
			{
				_modelParticles.Add(new ParticleData(particle, lifetime));
			}
		}

		private async void DestroyParticle()
		{
			GameObject parent = _modelParticles[0].Particle.transform.parent.gameObject;
			List<ParticleData> keysMarkedToDestroy = new List<ParticleData>();
			
			for (int i = 0; i < 500; ++i)
			{
				foreach (ParticleData particleData in _modelParticles)
				{
					particleData.LifeTime -= Time.fixedDeltaTime;
					if (particleData.LifeTime <= 0)
						keysMarkedToDestroy.Add(particleData);
				}

				foreach (ParticleData particleData in keysMarkedToDestroy)
				{
					_modelParticles.Remove(particleData);
					Object.Destroy(particleData.Particle);
				}
				
				keysMarkedToDestroy.Clear();
				
				if (_modelParticles.IsEmpty())
					break;
				
				await Task.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime));
			}
			
			Object.Destroy(parent);
		}
	}
}