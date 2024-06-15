using InteractableObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bot
{
	public class Level : MonoBehaviour
	{
		[SerializeField] private List<BotVase> _vases;

		public IReadOnlyList<BotVase> Vases => _vases;

		private void Awake()
		{
			foreach (BotVase vase in _vases)
			{
				vase.OnDestroyEvent += RemoveVase;
			}
		}

		private void RemoveVase(IDestroyable destroyBehaviour)
		{
			BotVase vase = destroyBehaviour as BotVase;

			if (vase == null)
				throw new Exception("Incorrect value");
			
			_vases.Remove(vase);
			destroyBehaviour.OnDestroyEvent -= RemoveVase;
		}
	}
}