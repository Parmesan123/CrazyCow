using Handlers;
using InteractableObject;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace UI
{
	public class CoinSpawner : MonoBehaviour
	{
		private const string COIN_PATH = "Prefabs/UI/Coin";

		[SerializeField] private int _spawnRadius;
		[SerializeField] private float _angleOffSet;
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Transform _finalPoint;

		[Header("Test")]
		[SerializeField] private Box _coinGiver;
		
		private Coin _coinPrefab;
		private CoinHandler _coinHandler;

		[Inject]
		private void Construct(CoinHandler coinHandler)
		{
			_coinHandler = coinHandler;
		}
		
		private void Awake()
		{
			_coinPrefab = Resources.Load<Coin>(COIN_PATH);
			Register(_coinGiver);
		}

		public void Register(ICoinGiver destroyable)
		{
			destroyable.OnGiveCoinEvent += UnRegister;
		}

		public void UnRegister(ICoinGiver coinGiver)
		{
			//coinGiver.OnGiveCoinEvent -= UnRegister;
			Vector3 screenPoint = UnityEngine.Camera.main.WorldToScreenPoint(coinGiver.Transform.position);
			Spawn(coinGiver, screenPoint);
		}

		private void Spawn(ICoinGiver coinGiver, Vector3 screenPosition)
		{
			int amountCoin = coinGiver.AmountCoin;
			List<Coin> coins = new List<Coin>();

			GameObject parent = new GameObject("CoinCluster")
			{
				transform =
				{
					position = screenPosition,
					parent = _canvas.transform,
				}
			};

			CoinCluster coinCluster = parent.AddComponent<CoinCluster>();
			
			for (; amountCoin > 0; --amountCoin)
			{
				Vector2 randomPoint = Random.insideUnitCircle * _spawnRadius;
				
				Coin newCoin = Instantiate(_coinPrefab, parent.transform);
				newCoin.gameObject.SetActive(false);
				newCoin.transform.localPosition = randomPoint;
				newCoin.transform.Rotate(new Vector3(0, Random.Range(-_angleOffSet, _angleOffSet), 0));

				_coinHandler.Register(newCoin);
				
				coins.Add(newCoin);
			}
			
			coinCluster.Init(coins, _finalPoint.position);
		}
	}
}