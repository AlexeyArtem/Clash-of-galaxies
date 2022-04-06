using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    static class GameRules
    {
        public static readonly int MaxRounds = 3;
        public static readonly int MaxStartPlayerCards = 6;
        public static readonly int MaxCardsInDeck = MaxStartPlayerCards * 2;
        public static readonly int MaxTimeToMoveInSeconds = 30;
    }
}