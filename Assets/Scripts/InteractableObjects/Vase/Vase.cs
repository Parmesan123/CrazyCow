using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace InteractableObject
{
	public class Vase : DestroyBehaviour, ICoinGiver
	{
		private List<Box> _boxes;

		private void Awake()
		{
			_boxes = new List<Box>();
		}

		public event Action<ICoinGiver> OnCoinGiveEvent;

		public int AmountCoin => Data.AmountCoin;
		public Transform Transform => transform;
		
		public bool TryAddBox(Box box)
		{
			if (_boxes.Contains(box))
				return false;
			
			_boxes.Add(box);
			return true;
		}

		public bool TryRemoveBox(Box box)
		{
			if (!_boxes.Contains(box))
				return false;
			
			_boxes.Remove(box);
			
			if (!_boxes.IsEmpty())
				return false;
			
			return true;
		}
		
		public override void Destroy()
		{
			if (_boxes.Count == 0)
				OnCoinGiveEvent?.Invoke(this);
			
			base.Destroy();
		}
	}
}