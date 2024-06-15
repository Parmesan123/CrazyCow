using ModestTree;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bot
{
	[SelectionBase, RequireComponent(typeof(NavMeshAgent))]
	public class BotBehavior : MonoBehaviour
	{
		[SerializeField] private Level _level;
		[SerializeField] private NavMeshSurface _navMeshSurface; //TODO another entity
		[SerializeField] private float _optimalDistance;
		[SerializeField] private float _destroyDistance;

		private NavMeshAgent _navMeshAgent;
		private BotVase _targetVase;
		private Coroutine _selectVaseCoroutine;
		private Coroutine _boxCoroutine;

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
		}

		/*public void OnEnable()
		{
			_navMeshSurface.BuildNavMesh();
			_selectVaseCoroutine = StartCoroutine(SelectVase());
		}

		public void OnDisable()
		{
			StopAllCoroutines();
		}*/

		[Button]
		public void Enable()
		{
			_navMeshSurface.BuildNavMesh();
			_selectVaseCoroutine = StartCoroutine(SelectVase());
		}

		[Button]
		public void Disable()
		{
			StopAllCoroutines();
		}

		private IEnumerator SelectVase()
		{
			WaitForSeconds wait = new WaitForSeconds(1);
			IReadOnlyList<BotVase> vases = _level.Vases;

			_targetVase = null;

			for (; vases.Count != 0;)
			{
				BotVase vaseClosest = vases.Aggregate((v, next) =>
												   Vector3.Distance(v.transform.position, transform.position) <
												   Vector3.Distance(next.transform.position, transform.position) ? v : next);

				IEnumerable<BotVase> vasesVisible = vases.Where(v =>
																	Vector3.Distance(v.transform.position, transform.position) <=
																	_optimalDistance);
				
				BotVase newVase = vasesVisible.FirstOrDefault(v => v.IncludedBox.Count < vaseClosest.IncludedBox.Count);

				vaseClosest = newVase ? newVase : vaseClosest;
				
				SetVase(vaseClosest);

				yield return wait;
			}

			gameObject.SetActive(false);
			yield break;


			void SetVase(BotVase newVase)
			{
				if (_targetVase == newVase)
					return;

				if (_boxCoroutine != null)
					StopCoroutine(_boxCoroutine);

				_targetVase = newVase;
				_boxCoroutine = StartCoroutine(SelectBox());
			}
		}

		private IEnumerator SelectBox()
		{
			BotBox targetBox = null;

			Color color = Color.white;

			for (; !_targetVase.IncludedBox.IsEmpty();)
			{
				if (targetBox != null)
					targetBox.GetComponent<MeshRenderer>().material.color = color;
				
				targetBox = _targetVase.IncludedBox.Aggregate((b, next) =>
																  Vector3.Distance(b.transform.position, transform.position) <
																  Vector3.Distance(next.transform.position, transform.position) ? b : next);

				float yCoordinate = _navMeshSurface.center.y;
				Vector3 newPosition = GetPosition();
				newPosition.y = yCoordinate;

				_navMeshAgent.SetDestination(newPosition);

				targetBox.GetComponent<MeshRenderer>().material.color = Color.red;

				Debug.Log($"vase name {_targetVase.name}, box name {targetBox.name}");

				yield return new WaitUntil(() => !targetBox.gameObject.activeSelf);
			}

			yield break;

			Vector3 GetPosition()
			{
				Vector3 localBoxPosition = targetBox.transform.position - _targetVase.transform.position;
				localBoxPosition.y = 0;
				Vector3 point;

				for (int i = 0; i < 1000; ++i)
				{
					Vector2 randomPoint = Random.insideUnitCircle * _destroyDistance;
					point = new Vector3(randomPoint.x, 0, randomPoint.y) + localBoxPosition;

					if (Mathf.Pow(point.x, 2) + Mathf.Pow(point.y, 2) > Mathf.Pow(_destroyDistance, 2))
						return _targetVase.transform.position + point;
				}

				point = localBoxPosition.normalized * _destroyDistance;

				return _targetVase.transform.position + point;
			}
		}
	}
}