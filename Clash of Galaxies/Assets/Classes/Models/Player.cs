using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Assets.Models
{
    public class Player
    {
        protected ObservableCollection<Card> cardsInHand;
        protected bool isPermissionMakeMove = false;

        public Player(string name)
        {
            Name = name;
            cardsInHand = new ObservableCollection<Card>();
            CardsInHand = new ReadOnlyObservableCollection<Card>(cardsInHand);
        }

        public event MakeMoveEventHandler MakeMove;
        public event MakeMoveEventHandler PlayCurrentCard;

        public string Name { get; protected set; }
        public Card CurrentCard { get; protected set; }
        public bool IsMoveCompleted { get; protected set; } = false;
        public virtual bool IsPermissionMakeMove 
        {
            get 
            {
                return isPermissionMakeMove;
            }
            protected set 
            {
                isPermissionMakeMove = value;
            }
        }
        public ReadOnlyObservableCollection<Card> CardsInHand { get; }

        public virtual void SetPermissionToMove(object sender, PermissionMakeMoveEventArgs args)
        {
            if (args.Player == this)
            {
                IsPermissionMakeMove = args.IsPermissionMakeMove;
                if (IsPermissionMakeMove)
                {
                    IsMoveCompleted = false;
                    CurrentCard = null;
                }
            }
        }

        public void SetCardsInHand(object sender, DealCardsEventArgs args)
        {
            if (args.Player == this)
            {
                foreach (var card in args.Cards)
                {
                    cardsInHand.Add(card);
                }
            }
        }

        public virtual void OnMakeMove(Card card)
        {
            if (!cardsInHand.Contains(card) || !IsPermissionMakeMove || CurrentCard != null) return;

            MakeMove?.Invoke(this, new MakeMoveEventArgs(card));
            cardsInHand.Remove(card);
            CurrentCard = card;
            card.ActivateBehaviour();

            if (!card.IsActivateBehaviour)
                CompleteMove();
        }

        public void CompleteMove()
        {
            IsMoveCompleted = true;
            CurrentCard = null;
        }

        public void OnPlayCurrentCard(Card targetCard)
        {
            if (CurrentCard != null && CurrentCard.IsActivateBehaviour)
            {
                PlayCurrentCard?.Invoke(this, new MakeMoveEventArgs(CurrentCard, targetCard));

                if (!CurrentCard.IsActivateBehaviour)
                    CompleteMove();
            }
        }

        public void ClearCardsInHand() 
        {
            foreach (var card in cardsInHand)
                card.OnDestroy();

            cardsInHand.Clear();
        }
    }
}