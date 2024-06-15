using System;
using UnityEngine;

namespace Entities
{
	public interface ICoinGiver
	{
		public event Action<ICoinGiver> OnCoinGiveEvent;
		
		public int AmountCoin { get; }
		public Transform Transform { get; }
	}
}