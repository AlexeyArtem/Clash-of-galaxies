using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class CardPresenter
    {
        private Card card;
        private ICardView cardView;

        public CardPresenter(Card card, ICardView cardView)
        {
            this.card = card;
            this.cardView = cardView;
            cardView.SetCardInfo(card.Name, card.Description, card.GamePoints, card.InfluenceGamePoints);
        }

        public ICardView CardView 
        {
            get 
            {
                return cardView;
            }
        }

        public Card Card 
        {
            get 
            {
                return card;
            }
        }
    }
}