using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Assets.Models
{
    public class PlayerEnemy : Player
    {
        private Card targetCard;
        private IReadOnlyDictionary<Player, List<Card>> openCards;

        public PlayerEnemy(string name) : base(name)
        {

        }

        private Card SelectCard() 
        {
            return cardsInHand[0];
        }

        private Card SelectTargetCard() 
        {
            return null;
        }

        public override bool IsPermissionMakeMove 
        {
            get 
            {
                return isPermissionMakeMove;
            }
            protected set 
            {
                isPermissionMakeMove = value;
                if (isPermissionMakeMove) 
                {
                    Card currentCard = SelectCard();
                    OnMakeMove(currentCard);
                    CompleteMove(); // На время тестирования

                    // Выбрать карту
                    // Выбрать карту для атаки или усиления
                    // Сделать ход выбранной картой
                    // Сыграть текущую карту
                }
            }
        }

        public override void SetPermissionToMove(object sender, PermissionMakeMoveEventArgs args)
        {
            if (args.Player == this)
            {
                IsPermissionMakeMove = args.IsPermissionMakeMove;
            }
        }
    }
}
