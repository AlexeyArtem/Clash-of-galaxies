using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    class CometBehaviour : TargetBehaviour
    {
        public CometBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {

        }

        public override int InfluencingGamePoints { get; protected set; } = 2;

        protected override void ProcessPlayedCard(object sender, MakeMoveEventArgs args)
        {
            if (!CheckEventArgs(args)) return;

            Card targetCard = args.TargetCard;
            var targetCards = openCards.Where(p => p.Key == owner).FirstOrDefault().Value;
            if (!targetCards.Contains(targetCard)) return;

            if (targetCard.Name == "Star") // В два раза сильнее усилить, если выбрана карта Звезды
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