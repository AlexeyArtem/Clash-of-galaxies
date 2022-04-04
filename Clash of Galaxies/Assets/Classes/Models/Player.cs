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
        protected List<Card> openCards;

        public Player(string name)
        {
            Name = name;
            cardsInHand = new List<Card>();
            openCards = new List<Card>();
            CardsInHand = new ReadOnlyCollection<Card>(cardsInHand);
            //OpenCards = new ReadOnlyCollection<Card>(openCards);
        }

        public event MakeMoveEventHandler MakeMove;
        public event MakeMoveEventHandler PlayCurrentCard;

        public string Name { get; }
        public Card CurrentCard { get; protected set; }
        public bool IsMoveCompleted { get; protected set; } = true;
        public bool IsPermissionMakeMove { get; protected set; } = false;
        //public ReadOnlyCollection<Card> OpenCards { get; }
        public ReadOnlyCollection<Card> CardsInHand { get; }
        public int TotalGamePoints 
        {
            get 
            {
                int value = openCards.Sum(c => c.GamePoints);
                return value;
            }
        }

        private void ToProcessDestructionCard(object sender, EventArgs args)
        {
            if (sender is Card card && openCards.Contains(card))
            {
                openCards.Remove(card);
                card.Destroy -= ToProcessDestructionCard;
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
                //foreach (var card in args.Cards)
                //{
                //    card.Destroy += ToProcessDestructionCard;
                //}
                cardsInHand.AddRange(args.Cards);
            }
        }

        public void OnMakeMove(Card card)
        {
            if (!cardsInHand.Contains(card) || !IsPermissionMakeMove) return;

            MakeMove?.Invoke(this, new MakeMoveEventArgs(card));
            cardsInHand.Remove(card);
            openCards.Add(card);
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