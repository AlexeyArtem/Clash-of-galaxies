using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Cards
    {
        //private static Cards instance;
        private Dictionary<Player, List<Card>> valuesCards;

        public Cards(IReadOnlyDictionary<Player, List<Card>> openCards)
        {
            valuesCards = new Dictionary<Player, List<Card>>();
            foreach (Player player in openCards.Keys)
            {
                List<Card> cards = new List<Card>()
                {
                    new Card("Asteroid", 3, "Нанесение 2 ед. урона карте противника. Полностью уничтожает выбранную карту, если она Asteroid.",
                              new AsteroidBehaviour(player, openCards)),
                    new Card("Inter cloud", 1, "Усиливает на 1 ед. все дружественные карты.",
                              new InterCloudBehaviour(player, openCards)),
                    new Card("Satellite", 2, "Усиливает выбранную карту на 1 ед. Если выбрана карта Planet, то усиливает её на 2 ед.",
                              new SatelliteBehaviour(player, openCards)),
                    new Card("Black Hole", 0, "Уничтожение любой карты противника. Исчезает после использования.",
                              new BlackHoleBehaviour(player, openCards)),
                    new Card("Meteorite", 3, "Нанесение 1 ед. урона каждой карте противника.",
                              new MeteoriteBehaviour(player, openCards)),
                    new Card("Comet", 2, "Усиливает, выбранную карту на 2 ед. Если выбрана звезда, то усиливает её на 4 ед.",
                              new CometBehaviour(player, openCards)),
                    new Card("Planet", 5, "Статичная карта."),
                    new Card("Star", 10, "Статичная карта звезды."),

                };
                Count = cards.Count;
                valuesCards.Add(player, cards);
            }
        }

        //public static Cards GetInstance(IReadOnlyDictionary<Player, List<Card>> openCards)
        //{
        //    if (instance == null)
        //        instance = new Cards(openCards);

        //    return instance;
        //}

        public int Count { get; private set; }

        public Card Get(Player owner, int index)
        {
            return valuesCards[owner][index].Clone();
        }
    }
}
