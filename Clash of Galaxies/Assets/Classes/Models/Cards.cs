using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Cards
    {
        private Dictionary<Player, List<Card>> valuesCards;

        public Cards(IReadOnlyDictionary<Player, List<Card>> openCards)
        {
            valuesCards = new Dictionary<Player, List<Card>>();
            foreach (Player player in openCards.Keys)
            {
                List<Card> cards = new List<Card>()
                {
                    new Card("Asteroid", 3, "��������� 2 ��. ����� ����� ����������. ��������� ���������� ��������� �����, ���� ��� Asteroid.",
                              "asteroidTemp", new AsteroidBehaviour(player, openCards)),
                    new Card("Inter cloud", 1, "��������� �� 1 ��. ��� ������������� �����.",
                              "interCloudTemp", new InterCloudBehaviour(player, openCards)),
                    new Card("Satellite", 2, "��������� ��������� ����� �� 1 ��. ���� ������� ����� Planet, �� ��������� � �� 2 ��.",
                              "satelliteTemp", new SatelliteBehaviour(player, openCards)),
                    new Card("Black Hole", 0, "����������� ����� ����� ����������. �������� ����� �������������.",
                              "blackHoleTemp", new BlackHoleBehaviour(player, openCards)),
                    new Card("Meteorite", 3, "��������� 1 ��. ����� ������ ����� ����������.",
                              "meteoriteTemp", new MeteoriteBehaviour(player, openCards)),
                    new Card("Comet", 2, "���������, ��������� ����� �� 2 ��. ���� ������� ������, �� ��������� � �� 4 ��.",
                              "cometTemp", new CometBehaviour(player, openCards)),
                    new Card("Planet", 5, "��������� ����� �������.", "planetTemp"),
                    new Card("Star", 10, "��������� ����� ������.", "starTemp"),

                };
                Count = cards.Count;
                valuesCards.Add(player, cards);
            }
        }

        public int Count { get; private set; }

        public Card Get(Player owner, int index)
        {
            return valuesCards[owner][index].Clone();
        }
    }
}
