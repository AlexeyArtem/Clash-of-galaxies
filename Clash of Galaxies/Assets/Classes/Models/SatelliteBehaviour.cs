using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class SatelliteBehaviour : TargetBehaviour
    {
        public SatelliteBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {

        }

        public override int InfluencingGamePoints { get; protected set; } = 1;

        protected override void ProcessPlayedCard(object sender, MakeMoveEventArgs args)
        {
            base.ProcessPlayedCard(sender, args);
            var targetCards = openCards.Where(p => p.Key == owner).FirstOrDefault().Value;
            if (!targetCards.Contains(args.Card)) return;

            Card targetCard = args.Card;
            if (targetCard.Name == "Planet") // В два раза сильнее усилить, если выбрана карта Планеты
            {
                int influencingGamePoints = InfluencingGamePoints;
                InfluencingGamePoints += influencingGamePoints;
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
