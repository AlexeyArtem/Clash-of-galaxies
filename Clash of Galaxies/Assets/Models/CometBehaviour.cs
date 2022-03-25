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
            base.ProcessPlayedCard(sender, args);

            Card targetCard = args.Card;
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