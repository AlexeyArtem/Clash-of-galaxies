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

        public PlayerController(Player player, PlayerView playerView)
        {
            this.player = player;
            this.playerView = playerView;

            List<CardView> cardViews = new List<CardView>(); 
            foreach (var card in player.CardsInHand)
            {
                CardView cardView = CardViewFactory.GetInstance().GetView();
                cardViews.Add(cardView);
                new CardController(card, cardView);
            }
            playerView.SetCardViews(cardViews);
        }


    }
}
