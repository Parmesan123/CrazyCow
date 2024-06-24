using ModestTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entities
{
    [SelectionBase, RequireComponent(typeof(NavMeshAgent))]
    public class BotBehavior : MonoBehaviour
    {
        [SerializeField] private CharacterData _data;
        [SerializeField] private BonusLevelHandler _level;
        [SerializeField] private float _optimalDistance;

        private float _destroyDistance;
        private NavMeshAgent _navMeshAgent;
        private Vase _targetVase;
        private Coroutine _boxCoroutine;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _destroyDistance = _data.DestroyRange;
            BoxDestroyer botBoxDestroyer = GetComponent<BoxDestroyer>();
            botBoxDestroyer.UpdateRange(_destroyDistance);
            _navMeshAgent.speed = _data.CharacterSpeed;
        }

        public void OnEnable()
        {
            StartCoroutine(SelectVase());
        }

        public void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SelectVase()
        {
            WaitForSeconds wait = new WaitForSeconds(1);
            IEnumerable<Vase> vases = _level.Vases;

            _targetVase = null;

            for (; vases.Count() != 0;)
            {
                Vase vaseClosest = vases.Aggregate((v, next) =>
                    Vector3.Distance(v.transform.position, transform.position) <
                    Vector3.Distance(next.transform.position, transform.position) ? v : next);

                IEnumerable<Vase> vasesVisible = vases.Where(v =>
                    Vector3.Distance(v.transform.position, transform.position) <=
                    _optimalDistance);

                Vase newVase = vasesVisible.FirstOrDefault(v => v.Boxes.Count() < vaseClosest.Boxes.Count());

                vaseClosest = newVase ? newVase : vaseClosest;

                SetVase(vaseClosest);

                yield return wait;
            }

            gameObject.SetActive(false);
            yield break;
            
            void SetVase(Vase newVase)
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
            Box targetBox;

            for (; !_targetVase.Boxes.IsEmpty();)
            {
                targetBox = _targetVase.Boxes.Aggregate((b, next) =>
                    Vector3.Distance(b.transform.position, transform.position) <
                    Vector3.Distance(next.transform.position, transform.position) ? b : next);

                Vector3 newPosition = GetPosition();

                _navMeshAgent.SetDestination(newPosition);

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