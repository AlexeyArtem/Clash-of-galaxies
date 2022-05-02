using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    [Serializable]
    public class Settings
    {
        private static int maxRounds = 3;
        
        private int maxStartPlayerCards;
        private int maxCardsInDeck;
        private int maxTimeToMoveInSeconds;

        public Settings() 
        {
            maxStartPlayerCards = 6;
            maxCardsInDeck = maxStartPlayerCards * 2;
            maxTimeToMoveInSeconds = 30;
            UserName = "User player";
        }

        public Settings(int maxStartPlayerCards, int maxCardsInDeck, int maxTimeToMoveInSeconds, string userName) : this()
        {
            MaxStartPlayerCards = maxStartPlayerCards;
            this.maxCardsInDeck = maxCardsInDeck;
            this.maxTimeToMoveInSeconds = maxTimeToMoveInSeconds;
            UserName = userName;
        }

        public int MaxRounds { get => maxRounds; }
        public string UserName { get; private set; }
        public int MaxStartPlayerCards
        {
            get 
            {
                return maxStartPlayerCards;
            }
            private set 
            {
                if (value > 0) 
                {
                    maxStartPlayerCards = value;
                }
            }
        }
        public int MaxCardsInDeck 
        {
            get 
            {
                return maxCardsInDeck;
            }
            private set 
            {
                if (value > maxStartPlayerCards) 
                {
                    maxCardsInDeck = value;
                }
            }
        }
        public int MaxTimeToMoveInSeconds 
        {
            get 
            {
                return maxTimeToMoveInSeconds;
            }
            private set 
            {
                if (value > 0) 
                {
                    maxTimeToMoveInSeconds = value;
                }
            }
        }
    }
}
