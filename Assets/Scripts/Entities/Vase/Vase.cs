using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Entities
{
	public class Vase : DestroyBehavior, ICoinGiver
	{
		public event Action<ICoinGiver> OnCoinGiveEvent;
		
		public int AmountCoin => Data.AmountCoin;
		public Transform Transform => transform;
		public IEnumerable<Box> Boxes => _boxes; 
		
		private List<Box> _boxes;

		private void Awake()
		{
			_boxes = new List<Box>();
		}
		
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
			return _boxes.IsEmpty();
		}
		
		public override void Destroy()
		{
			if (_boxes.Count == 0)
				OnCoinGiveEvent?.Invoke(this);
			
			base.Destroy();
		}
	}
}