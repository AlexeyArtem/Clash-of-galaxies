using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    struct GameInfo
    {
        private Dictionary<Player, int> roundWins;

        public GameInfo(Player playerA, Player playerB)
        {
            PlayerA = playerA;
            PlayerB = playerB;
            roundWins = new Dictionary<Player, int>()
            {
                {playerA, 0},
                {playerB, 0}
            };
            CurrentRound = 1;
        }

        public Player PlayerA { get; }
        public Player PlayerB { get; }
        public IReadOnlyDictionary<Player, int> RoundWins
        {
            get
            {
                return roundWins;
            }
        }
        public int CurrentRound { get; private set; }

        public void AddRoundWin(Player player)
        {
            roundWins[player] += 1;
            CurrentRound += 1;
        }

        public void AddRoundDraw()
        {
            roundWins[PlayerA] += 1;
            roundWins[PlayerB] += 1;
            CurrentRound += 1;
        }
    }
}
