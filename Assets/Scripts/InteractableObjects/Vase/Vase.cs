using System;
using UnityEngine;

namespace InteractableObject
{
	public class Vase : DestroyBehaviour, ICoinGiver
	{
		public event Action<ICoinGiver> OnGiveCoinEvent;
		public Transform Transform { get; private set; }
		public int AmountCoin { get; private set; }

		private void Awake()
		{
			Transform = transform;
			AmountCoin = _data.AmountCoin;
		}
	}
}