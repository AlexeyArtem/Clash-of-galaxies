using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Player
    {
        protected List<Card> cardsInHand;
        protected bool isPermissionMakeMove = false;
        protected object lockObj = new object();

        public Player(string name)
        {
            Name = name;
            cardsInHand = new List<Card>();
            CardsInHand = new ReadOnlyCollection<Card>(cardsInHand);
        }

        public event MakeMoveEventHandler MakeMove;
        public event MakeMoveEventHandler PlayCurrentCard;

        public string Name { get; }
        public Card CurrentCard { get; protected set; }
        public bool IsMoveCompleted { get; protected set; } = false;
        public bool IsPermissionMakeMove 
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
        public ReadOnlyCollection<Card> CardsInHand { get; }


        public void SetPermissionToMove(object sender, PermissionMakeMoveEventArgs args)
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
                cardsInHand.AddRange(args.Cards);
            }
        }

        public void OnMakeMove(Card card)
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
    }
}