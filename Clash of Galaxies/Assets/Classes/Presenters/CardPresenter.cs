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
            cardView.SetImage(card.NameImg);
            card.PropertyChanged += Card_PropertyChanged;
            card.Destroy += Card_Destroy;

            if (card.Behaviour != null) 
            {
                card.Behaviour.BeginBehaviour += Behaviour_BeginBehaviour;
                card.Behaviour.EndBehaviour += Behaviour_EndBehaviour;
            }
        }

        private void Behaviour_EndBehaviour(object sender, EventArgs e)
        {
            cardView.DeactivateTargetArrow();
        }

        private void Behaviour_BeginBehaviour(object sender, EventArgs e)
        {
            cardView.ActivateTargetArrow();
        }

        private void Card_Destroy(object sender, EventArgs e)
        {
            cardView.DeactivateTargetArrow();
            cardView.DestroyView();
            card.PropertyChanged -= Card_PropertyChanged;
            card.Destroy -= Card_Destroy;
            if (card.Behaviour != null)
            {
                card.Behaviour.BeginBehaviour -= Behaviour_BeginBehaviour;
                card.Behaviour.EndBehaviour -= Behaviour_EndBehaviour;
            }
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