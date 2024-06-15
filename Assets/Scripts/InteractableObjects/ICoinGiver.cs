using System;
using UnityEngine;

namespace InteractableObject
{
	public interface ICoinGiver
	{
		public event Action<ICoinGiver> OnCoinGiveEvent;
		
		public int AmountCoin { get; }
		public Transform Transform { get; }
	}
}