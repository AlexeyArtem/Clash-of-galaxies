using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class BlackHoleBehaviour : TargetBehaviour
    {
        public override int InfluencingGamePoints { get; protected set; } = 0;

        public BlackHoleBehaviour(Player owner, IReadOnlyDictionary<Player, List<Card>> openCards) : base(owner, openCards)
        {

        }

        protected override void ProcessPlayedCard(object sender, MakeMoveEventArgs args)
        {
            //base.ProcessPlayedCard(sender, args);
            if (!CheckEventArgs(args)) return;

            Card targetCard = args.TargetCard;
            var targetCards = openCards.Where(p => p.Key != owner).FirstOrDefault().Value;
            if (!targetCards.Contains(targetCard)) return;
            
            InfluencingGamePoints = 0 - targetCard.GamePoints;
            currentCard.InfluenceOnCard(targetCard);

            InfluencingGamePoints = 0 - currentCard.GamePoints;
            currentCard.InfluenceOnCard(currentCard); // ”ничтожение карты самой себ€

            IsActivate = false;
        }
    }
}