using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using UnityEngine;
using Assets.Views;

namespace Assets.Presenters
{
    class PlayerPresenter
    {
        private Player player;
        private IPlayerView playerView;
        private List<CardPresenter> cardPresenters;

        public IReadOnlyCollection<CardPresenter> CardPresenters 
        {
            get 
            {
                return cardPresenters;
            }
        }

        public PlayerPresenter(Player player, IPlayerView playerView)
        {
            this.player = player;
            this.playerView = playerView;
            cardPresenters = new List<CardPresenter>();

            List<ICardView> cardViews = new List<ICardView>(); 
            foreach (var card in player.CardsInHand)
            {
                CardView cardView = CardViewFactory.GetInstance().GetView();
                cardViews.Add(cardView);
                cardPresenters.Add(new CardPresenter(card, cardView));
            }
            playerView.SetCardViews(cardViews);
            playerView.DropCardCallback = DropCardView;
        }

        public void DropCardView(ICardView cardView)
        {
            CardPresenter cardPresenter = cardPresenters.Where(c => c.CardView == cardView).FirstOrDefault();
            if (cardPresenter != null) 
            {
                Card card = cardPresenter.Card;
                player.OnMakeMove(card);
            }
        }
    }
}
