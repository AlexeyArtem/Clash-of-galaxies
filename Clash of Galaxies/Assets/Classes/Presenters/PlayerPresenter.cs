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

        public IReadOnlyCollection<CardPresenter> CardPresentersInHand
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

                CardPresenter cardPresenter = CardPresenterFactory.GetInstance().CreateNewPresenter(card, cardView);
                if(cardPresenter != null) cardPresenters.Add(cardPresenter);
            }
            playerView.SetCardViews(cardViews);

            playerView.DropCardCallback = DropCardView;
            playerView.PlayCurrentCardCallback = PlayCurrentCardView;
        }

        public void DropCardView(ICardView cardView)
        {
            CardPresenter cardPresenter = CardPresenterFactory.GetInstance().FindPresenter(cardView);
            if (cardPresenter != null) 
            {
                Card card = cardPresenter.Card;
                player.OnMakeMove(card);
            }
        }

        public void PlayCurrentCardView(ICardView cardView) 
        {
            CardPresenter cardPresenter = CardPresenterFactory.GetInstance().FindPresenter(cardView);
            if (cardPresenter != null)
            {
                Card card = cardPresenter.Card;
                player.OnPlayCurrentCard(card);
            }
        }
    }
}
