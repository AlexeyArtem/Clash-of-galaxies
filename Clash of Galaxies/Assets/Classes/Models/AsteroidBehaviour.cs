using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class AsteroidBehaviour : TargetBehaviour
    {
        public override int InfluencingGamePoints { get; protected set; } = -2;

        public AsteroidBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {

        }

        protected override void ProcessPlayedCard(object sender, MakeMoveEventArgs args)
        {
            base.ProcessPlayedCard(sender, args);

            Card targetCard = args.Card;
            var targetCards = openCards.Where(p => p.Key != owner).FirstOrDefault().Value;
            if (targetCards.Contains(targetCard))
            {
                if (targetCard.Name == currentCard.Name) // Полностью уничтожить карту, если она совпадает с текущей
                {
                    int influencingGamePoints = InfluencingGamePoints;
                    InfluencingGamePoints = 0 - targetCard.GamePoints;
                    currentCard.InfluenceOnCard(targetCard);

                    InfluencingGamePoints = influencingGamePoints;
                    IsActivate = false;

                    return;
                }
                currentCard.InfluenceOnCard(targetCard);
                IsActivate = false;
            }
        }
    }
}
