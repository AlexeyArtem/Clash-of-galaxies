using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Player
    {
        protected List<Card> cardsInHand;
        //protected Card CurrentCard;

        public Player(string name)
        {
            Name = name;
            cardsInHand = new List<Card>();
        }

        public event MakeMoveEventHandler MakeMove;
        public event MakeMoveEventHandler PlayCurrentCard;

        public string Name { get; }
        public Card CurrentCard { get; protected set; }
        public bool IsMoveCompleted { get; protected set; } = true;
        public bool IsPermissionMakeMove { get; protected set; } = false;
        public IReadOnlyCollection<Card> CardsInHand
        {
            get
            {
                return cardsInHand;
            }
        }

        public void GetPermissionToMove(object sender, PermissionMakeMoveEventArgs args)
        {
            if (args.Player == this)
            {
                IsPermissionMakeMove = args.IsPermissionMakeMove;
            }
        }

        public void SetStartCardsInHand(List<Card> cards)
        {
            cardsInHand = cards;
            CurrentCard = null;
        }

        public void GetCardsInHand(object sender, DealCardsEventArgs args)
        {
            if (args.Player == this)
            {
                cardsInHand.AddRange(args.Cards);
            }
        }

        public void OnMakeMove(Card card)
        {
            if (!cardsInHand.Contains(card) || !IsPermissionMakeMove) return;

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
                PlayCurrentCard?.Invoke(this, new MakeMoveEventArgs(targetCard));

                if (!CurrentCard.IsActivateBehaviour)
                    CompleteMove();
            }
        }
    }
}