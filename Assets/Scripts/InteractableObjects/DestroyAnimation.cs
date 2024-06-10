using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using ModestTree;
using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace InteractableObject
{
	public class DestroyAnimation : MonoBehaviour
	{
		[SerializeField, Min(0)] private float _minLifeTime;
		[SerializeField, Min(0)] private float _maxLifeTime;
		[SerializeField] private GameObject _model;
		[SerializeField] private Material _crossSectionMaterial;

		private DestroyBehaviour _destroyBehaviour;
		private readonly List<ParticleData> _modelParticles = new List<ParticleData>();
		
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
		
		private void Awake()
		{
			_destroyBehaviour = GetComponent<DestroyBehaviour>();

			if (_destroyBehaviour == null)
				throw new NullReferenceException("Destroyable component not found");
		}

		private void OnEnable()
		{
			_destroyBehaviour.OnDestroy += Animation;
		}

		private void OnDisable()
		{
			_destroyBehaviour.OnDestroy -= Animation;
		}

		private void Animation(DestroyBehaviour _)
		{
			List<GameObject> particles = CreateParticle();
			AddToList(particles);
			DestroyParticle();
		}
		
		private List<GameObject> CreateParticle()
		{
			List<GameObject> finalParticle = new List<GameObject>();
			List<GameObject> temp = new List<GameObject>();

			GameObject parent = new GameObject("AnimationParent")
			{
				transform =
				{
					position = _model.transform.position,
					rotation = _model.transform.rotation
				}
			};
			GameObject copyModel = Instantiate(_model, parent.transform);

			finalParticle.Add(copyModel);

			for (int i = 0; i < 4; ++i)
			{
				Vector3 position = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
				Vector3 direction = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * Vector3.left;

				foreach (GameObject piece in finalParticle)
				{
					GameObject[] pieces = piece.SliceInstantiate(position, direction, _crossSectionMaterial);

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
					Destroy(piece);
				}

				finalParticle.Clear();
				finalParticle.AddRange(temp);
				temp.Clear();
			}

			foreach (GameObject piece in finalParticle)
			{
				Rigidbody rb = piece.AddComponent<Rigidbody>();
				piece.AddComponent<MeshCollider>().convex = true;
				rb.AddExplosionForce(200f, transform.position, 2);
			}
			
			return finalParticle;
		}

		private void AddToList(List<GameObject> particles)
		{
			foreach (GameObject particle in particles)
			{
				_modelParticles.Add(new ParticleData(particle, Random.Range(_minLifeTime, _maxLifeTime)));
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
					Destroy(particleData.Particle);
				}
				
				keysMarkedToDestroy.Clear();
				
				if (_modelParticles.IsEmpty())
					break;
				
				await Task.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime));
			}
			
			Destroy(parent);
		}
	}
}