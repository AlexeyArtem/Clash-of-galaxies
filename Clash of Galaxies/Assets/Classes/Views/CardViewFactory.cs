using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Views
{
	public class CardViewFactory
	{
		private static CardViewFactory instance;
		private GameObject cardPref;
		private GameObject tempCard;

		// Create the view
		private CardViewFactory()
		{
			cardPref = Resources.Load<GameObject>("Prefabs/CardPref");
			var instance = Resources.Load<GameObject>("Prefabs/TempCardPref");
			tempCard = UnityEngine.Object.Instantiate(instance);
			tempCard.SetActive(true);
		}

		public static CardViewFactory GetInstance() 
		{
			if (instance == null)
				instance = new CardViewFactory();

			return instance;
		}

		public CardView GetView() 
		{
			var instance = UnityEngine.Object.Instantiate(cardPref);
			return instance.GetComponent<CardView>();
		}

		public GameObject GetTempCard() 
		{
			return tempCard;
		}
	}
}
