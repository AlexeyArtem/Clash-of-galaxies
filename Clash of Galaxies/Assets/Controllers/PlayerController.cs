using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using UnityEngine;
using Assets.Views;

namespace Assets.Controllers
{
    class PlayerController
    {
        private Player player;
        private PlayerView playerView;
        private List<CardController> cardControllers;

        public IReadOnlyCollection<CardController> CardControllers 
        {
            get 
            {
                return cardControllers;
            }
        }

        public PlayerController(Player player, PlayerView playerView)
        {
            this.player = player;
            this.playerView = playerView;
            cardControllers = new List<CardController>();

            List<CardView> cardViews = new List<CardView>(); 
            foreach (var card in player.CardsInHand)
            {
                CardView cardView = CardViewFactory.GetInstance().GetView();
                cardViews.Add(cardView);
                cardControllers.Add(new CardController(card, cardView));
            }
            playerView.SetCardViews(cardViews);
            playerView.DropCard = DropCardView;
        }

        public void DropCardView(CardView cardView)
        {
            CardController cardController = cardControllers.Where(c => c.CardView == cardView).First();
            if (cardController != null) 
            {
                Card card = cardController.Card;
                player.OnMakeMove(card);
            }
        }
    }
}
