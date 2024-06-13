using System;
using UnityEngine;

namespace InteractableObject
{
	public interface ICoinGiver
	{
		public event Action<ICoinGiver> OnCoinGive;
		
		public int AmountCoin { get; }
		public Transform Transform { get; }
	}
}