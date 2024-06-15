using System;
using Handlers;
using System.Collections.Generic;
using Entities;
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
		
		private Coin _coinPrefab;
		private WalletHandler _walletHandler;
		private BoxFactory _boxFactory;
		private VaseFactory _vaseFactory;

		[Inject]
		private void Construct(WalletHandler walletHandler, BoxFactory boxFactory, VaseFactory vaseFactory)
		{
			_walletHandler = walletHandler;

			_boxFactory = boxFactory;
			_boxFactory.OnSpawnBoxEvent += Register;
			foreach (Box spawnedBox in _boxFactory.SpawnedBoxes)
				spawnedBox.OnSpawnEvent += Register;

			_vaseFactory = vaseFactory;
			_vaseFactory.OnSpawnVaseEvent += Register;
			foreach (Vase spawnedVase in _vaseFactory.SpawnedVases)
				spawnedVase.OnSpawnEvent += Register;
		}

		private void Awake()
		{
			_coinPrefab = Resources.Load<Coin>(COIN_PATH);
		}

		private void OnDestroy()
		{
			_boxFactory.OnSpawnBoxEvent -= Register;

			_vaseFactory.OnSpawnVaseEvent -= Register;
		}

		private void Register(ISpawnable spawnable)
		{
			if (spawnable is not ICoinGiver coinGiver)
				throw new Exception("Can't process registration on coin spawner");
			
			coinGiver.OnCoinGiveEvent += UnRegister;
		}

		private void UnRegister(ICoinGiver coinGiver)
		{
			coinGiver.OnCoinGiveEvent -= UnRegister;
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(coinGiver.Transform.position);
			Spawn(coinGiver, screenPoint);
		}
		
		private void Spawn(ICoinGiver coinGiver, Vector3 screenPoint)
		{
			List<Coin> coins = new List<Coin>();
			GameObject parent = new GameObject("CoinCluster")
			{
				transform =
				{
					position = screenPoint,
					parent = _canvas.transform,
				}
			};

			CoinCluster coinCluster = parent.AddComponent<CoinCluster>();
			
			int amountCoin = coinGiver.AmountCoin;
			for (; amountCoin > 0; --amountCoin)
			{
				Vector2 randomPoint = Random.insideUnitCircle * _spawnRadius;
				
				Coin newCoin = Instantiate(_coinPrefab, parent.transform);
				newCoin.gameObject.SetActive(false);
				newCoin.transform.localPosition = randomPoint;
				newCoin.transform.Rotate(new Vector3(0, Random.Range(-_angleOffSet, _angleOffSet), 0));

				_walletHandler.Register(newCoin);
				
				coins.Add(newCoin);
			}
			
			coinCluster.Init(coins, _finalPoint.position);
		}
	}
}