using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public abstract class TargetBehaviour : Behaviour
    {
        protected Card currentCard;

        protected TargetBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {
            owner.PlayCurrentCard += ProcessPlayedCard;
        }

        public override void Activate(Card card)
        {
            if (owner.CurrentCard == card)
            {
                currentCard = card;
                IsActivate = true;
            }
        }

        protected virtual void ProcessPlayedCard(object sender, MakeMoveEventArgs args)
        {
            if (currentCard == null || !IsActivate) return;
        }
    }
}
