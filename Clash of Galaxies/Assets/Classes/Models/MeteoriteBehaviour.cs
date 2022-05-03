using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class MeteoriteBehaviour : Behaviour
    {
        public MeteoriteBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {

        }

        public override int InfluencingGamePoints { get; protected set; } = -1;

        public override void Activate(Card card)
        {
            if (owner.CurrentCard != card) return;
            currentCard = card;
            var targetCards = openCards.Where(p => p.Key != owner).FirstOrDefault().Value.ToList();

            IsActivate = true;
            foreach (Card targetCard in targetCards)
            {
                card.InfluenceOnCard(targetCard);
            }
            IsActivate = false;
        }
    }
}