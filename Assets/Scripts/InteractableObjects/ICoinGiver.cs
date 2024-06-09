using System;
using UnityEngine;

namespace InteractableObject
{
	public interface ICoinGiver
	{
		public event Action<ICoinGiver> OnGiveCoinEvent;
		
		public Transform Transform { get; }
		public int AmountCoin { get; }
	}
}