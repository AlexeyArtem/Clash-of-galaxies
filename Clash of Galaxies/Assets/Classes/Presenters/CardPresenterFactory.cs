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
            //Presenters = new ReadOnlyCollection<CardPresenter>(presenters);
        }

        public ReadOnlyCollection<CardPresenter> Presenters { get; }

        public static CardPresenterFactory GetInstance() 
        {
            if (instance == null)
                instance = new CardPresenterFactory();

            return instance;
        }

        public CardPresenter CreateNewPresenter(Card card, ICardView cardView) 
        {
            if (FindPresenter(card) != null || FindPresenter(cardView) != null)
                return null;

            CardPresenter cardPresenter = new CardPresenter(card, cardView);
            presenters.Add(cardPresenter);

            return cardPresenter;
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
