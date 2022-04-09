using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    // An area with open playing cards. Responsible for processing moves
    public class GameBoard
    {
        private Dictionary<Player, List<Card>> openCards;

        public GameBoard(Player playerA, Player playerB)
        {
            openCards = new Dictionary<Player, List<Card>>
            {
                { playerA, new List<Card>() },
                { playerB, new List<Card>() }
            };
            OpenCards = new ReadOnlyDictionary<Player, List<Card>>(openCards);
            PlayerA = playerA;
            PlayerB = playerB;
            playerA.MakeMove += ToProcessMove;
            playerB.MakeMove += ToProcessMove;
        }

        // For testing
        public event MakeMoveEventHandler NewOpenCard;

        public Player PlayerA { get; }
        public Player PlayerB { get; }

        public IReadOnlyDictionary<Player, List<Card>> OpenCards { get; }

        private void OnNewOpenCard(Player player, Card card) 
        {
            NewOpenCard?.Invoke(this, new MakeMoveEventArgs(player, card));
        }

        private void ToProcessMove(object sender, MakeMoveEventArgs args)
        {
            if (sender is Player player && openCards.ContainsKey(player))
            {
                Card card = args.Card;
                openCards[player].Add(card);
                card.Destroy += ToProcessDestructionCard;
                OnNewOpenCard(player, card);
            }
        }

        private void ToProcessDestructionCard(object sender, EventArgs args)
        {
            if (sender is Card card)
            {
                foreach (Player player in openCards.Keys)
                {
                    if (openCards[player].Remove(card))
                    {
                        card.Destroy -= ToProcessDestructionCard;
                        break;
                    }
                }
            }
        }

        public int GetTotalGamePoints(Player player)
        {
            int sum = openCards[player].Sum(c => c.GamePoints);
            return sum;
        }

        public void ClearOpenCards() 
        {
            foreach (var card in openCards[PlayerA].ToList())
                card.OnDestroy();

            foreach (var card in openCards[PlayerB].ToList())
                card.OnDestroy();
        }
    }
}