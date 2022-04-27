using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Assets.Models
{
    public class PlayerEnemy : Player
    {
        private Dictionary<string, bool> cardsInfoIsAttacking;
        private Dictionary<string, bool> cardsInfoIsMassTarget;

        public PlayerEnemy(string name) : base(name) { }

        public PlayerEnemy(string name, string pathFileCardsInfo) : base(name)
        {
            cardsInfoIsMassTarget = new Dictionary<string, bool>();
            cardsInfoIsAttacking = new Dictionary<string, bool>();

            XDocument xDoc = XDocument.Load(pathFileCardsInfo);
            var records = xDoc.Root.Element("records").Elements("record");
            foreach (var record in records)
            {
                string nameCard = record.Element("Name").Value;
                bool isMassTarget = Convert.ToBoolean(record.Element("IsMassTarget").Value);
                bool isAttacking = Convert.ToBoolean(record.Element("IsAttacking").Value);

                cardsInfoIsMassTarget.Add(nameCard, isMassTarget);
                cardsInfoIsAttacking.Add(nameCard, isAttacking);
            }
        }

        private Card SelectCardToMove(IReadOnlyDictionary<Player, List<Card>> openCards, out Card targetCard) 
        {
            Card cardToMove = null;
            targetCard = null;
            int maxDiffGamePoints = 0;

            foreach (Card curCard in cardsInHand)
            {
                int curDiffGamePoints = 0;
                bool isMassTarget = cardsInfoIsMassTarget[curCard.Name];
                bool isAttacking = cardsInfoIsAttacking[curCard.Name];

                IEnumerable<Card> cards = new List<Card>();
                if (isAttacking)
                    cards = openCards.Where(p => p.Key != this).FirstOrDefault().Value;
                else 
                    cards = openCards[this];

                curDiffGamePoints = curCard.GamePoints;
                if (isMassTarget)
                    for (int i = 0; i < cards?.Count(); i++)
                        curDiffGamePoints += Math.Abs(curCard.InfluenceGamePoints);
                else 
                    curDiffGamePoints += Math.Abs(curCard.InfluenceGamePoints);

                if (curDiffGamePoints > maxDiffGamePoints)
                {
                    maxDiffGamePoints = curDiffGamePoints;
                    cardToMove = curCard;
                    if (!isMassTarget) targetCard = cards.LastOrDefault();
                    else targetCard = null;
                }
            }

            return cardToMove;
        }

        public override void SetPermissionToMove(object sender, PermissionMakeMoveEventArgs args)
        {
            if (args.Player == this)
            {
                IsPermissionMakeMove = args.IsPermissionMakeMove;
                if (IsPermissionMakeMove) 
                {
                    Card card = SelectCardToMove(args.OpenCards, out Card targetCard);
                    OnMakeMove(card);
                    if (targetCard != null) OnPlayCurrentCard(targetCard);
                    CompleteMove();
                }
            }
        }
    }
}
