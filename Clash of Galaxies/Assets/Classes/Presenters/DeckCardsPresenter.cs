using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class DeckCardsPresenter
    {
        private IEnumerable<Card> cards;
        private IDeckCardsView deckCardsView;

        public DeckCardsPresenter(IEnumerable<Card> cards, IDeckCardsView deckCardsView)
        {
            this.cards = cards;
            this.deckCardsView = deckCardsView;
            SetDeckCardsView(cards);

            this.deckCardsView = deckCardsView;
            var collectionCards = (INotifyCollectionChanged)cards;
            collectionCards.CollectionChanged += CollectionCards_CollectionChanged;
        }

        private void CollectionCards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) 
            {
                var cards = e.NewItems.Cast<Card>();
                SetDeckCardsView(cards);
            }
        }

        public void SetDeckCardsView(IEnumerable<Card> cards) 
        {
            List<ICardView> cardViews = new List<ICardView>();
            foreach (var card in cards)
            {
                ICardView cardView = CardViewFactory.GetInstance().GetNewView();
                CardPresenterFactory.GetInstance().CreateNewPresenter(card, cardView);
                cardViews.Add(cardView);
            }
            deckCardsView.SetDeckCards(cardViews);
        }
    }
}
