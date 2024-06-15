using InteractableObject;
using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bot
{
	public class BotVase : DestroyBehaviour
	{
		[field: SerializeField] private List<BotBox> _includedBox;

		public IReadOnlyList<BotBox> IncludedBox => _includedBox;

		private void Awake()
		{
			foreach (BotBox box in _includedBox)
			{
				box.OnDestroyEvent += RemoveBox;
			}
		}

		public void RemoveBox(DestroyBehaviour destroyBehaviour)
		{
			BotBox box = destroyBehaviour as BotBox;

			if (box == null)
				throw new Exception("Incorrect value");

			_includedBox.Remove(box);
			destroyBehaviour.OnDestroyEvent -= RemoveBox;
			
			DestroyVase();
		}

		private void DestroyVase()
		{
			if (!_includedBox.IsEmpty())
				return;
			
			Destroy();
		}
	}
}