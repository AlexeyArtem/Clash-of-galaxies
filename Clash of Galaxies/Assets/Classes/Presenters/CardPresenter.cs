using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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
            card.PropertyChanged += Card_PropertyChanged;
            card.Destroy += Card_Destroy;
        }

        private void Card_Destroy(object sender, EventArgs e)
        {
            cardView.DestroyView();
            card.PropertyChanged -= Card_PropertyChanged;
            card.Destroy -= Card_Destroy;
            card = null;
            cardView = null;
        }

        private void Card_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GamePoints") 
            {
                cardView.ChangeGamePoints(card.GamePoints);
            }
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