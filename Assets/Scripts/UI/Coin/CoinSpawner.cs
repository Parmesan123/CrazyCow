using Handlers;
using System.Collections.Generic;
using Services;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace UI
{
	public class CoinSpawner : MonoBehaviour, ISignalReceiver<CoinGiveSignal>
	{
		private const string COIN_PATH = "Prefabs/UI/Coin";

		[SerializeField] private int _spawnRadius;
		[SerializeField] private float _angleOffSet;
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Transform _finalPoint;
		
		private Coin _coinPrefab;
		private WalletHandler _walletHandler;
		private SignalBus _signalBus;

		[Inject]
		private void Construct(WalletHandler walletHandler, SignalBus signalBus)
		{
			_walletHandler = walletHandler;
			_signalBus = signalBus;
		}

		private void Awake()
		{
			_coinPrefab = Resources.Load<Coin>(COIN_PATH);
		}

		private void OnEnable()
		{
			_signalBus.RegisterUnique<CoinGiveSignal>(this);
		}

		private void OnDisable()
		{
			_signalBus.Unregister<CoinGiveSignal>(this);
		}
		
		public void Receive(CoinGiveSignal signal)
		{
			int amountCoin = signal.AmountCoin;
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(signal.Transform.position);
			
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