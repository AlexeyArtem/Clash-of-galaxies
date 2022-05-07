using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class CardPresenterFactory
    {
        private static CardPresenterFactory instance;
        private List<CardPresenter> presenters;

        private CardPresenterFactory() 
        {
            presenters = new List<CardPresenter>();
            Values = new ReadOnlyCollection<CardPresenter>(presenters);
        }

        public ReadOnlyCollection<CardPresenter> Values { get; }

        private void Card_Destroy(object sender, EventArgs e)
        {
            Card card = sender as Card;
            CardPresenter cardPresenter = presenters.Where(c => card == c.Card || c.Card == null)?.FirstOrDefault();
            presenters.Remove(cardPresenter);
            card.Destroy -= Card_Destroy;
        }

        public static CardPresenterFactory GetInstance() 
        {
            if (instance == null)
                instance = new CardPresenterFactory();

            return instance;
        }

        public CardPresenter GetOrCreatePresenter(Card card, ICardView cardView) 
        {
            CardPresenter presenter = FindPresenter(card);
            if (presenter == null) 
            {
                presenter = new CardPresenter(card, cardView);
                card.Destroy += Card_Destroy;
                presenters.Add(presenter);
            }

            return presenter;
        }

        public void Clear() 
        {
            foreach (var presenter in presenters)
            {
                presenter.Card.Destroy -= Card_Destroy;
                presenter.Unsubscribe();
            }
            presenters.Clear();
        }

        public CardPresenter FindPresenter(Card card)
        {
            CardPresenter cardPresenter = presenters.Where(c => card == c.Card)?.FirstOrDefault();
            return cardPresenter;
        }

        public CardPresenter FindPresenter(ICardView cardView)
        {
            CardPresenter cardPresenter = presenters.Where(c => cardView == c.CardView)?.FirstOrDefault();
            return cardPresenter;
        }
    }
}
