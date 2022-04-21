using Assets.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DeckCardsView : MonoBehaviour, IDeckCardsView
{
    public void SetDeckCards(List<ICardView> cardViews)
    {
        foreach (var view in cardViews)
        {
            view.SetActiveCardShirt(true);
            MonoBehaviour monoBehaviourCard = view as MonoBehaviour;
            monoBehaviourCard.gameObject.transform.SetParent(gameObject.transform, false);
            monoBehaviourCard.gameObject.SetActive(true);
        }
    }
}
