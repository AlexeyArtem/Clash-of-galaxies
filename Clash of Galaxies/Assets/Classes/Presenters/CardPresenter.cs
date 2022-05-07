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
    class CardPresenter : IUnsubscribing
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

        public ICardView CardView { get => cardView; }
        public Card Card { get => card; }

        private void Behaviour_BeginBehaviour(object sender, EventArgs e)
        {
            if(sender == card)
                cardView.ActivateBehaviour();
        }

        private void Behaviour_EndBehaviour(object sender, EventArgs e)
        {
            if(sender == card)
                cardView.DeactivateBehaviour();
        }

        private void Card_Destroy(object sender, EventArgs e)
        {
            cardView.DeactivateBehaviour();
            cardView.DestroyView();
            Unsubscribe();
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

        public void Unsubscribe()
        {
            card.PropertyChanged -= Card_PropertyChanged;
            card.Destroy -= Card_Destroy;
            if (card.Behaviour != null)
            {
                card.Behaviour.BeginBehaviour -= Behaviour_BeginBehaviour;
                card.Behaviour.EndBehaviour -= Behaviour_EndBehaviour;
            }
        }
    }
}