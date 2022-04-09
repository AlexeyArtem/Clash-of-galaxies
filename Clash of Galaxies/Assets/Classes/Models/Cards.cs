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
                    new Card("Asteroid", 3, "��������� 2 ��. ����� ����� ����������. ��������� ���������� ��������� �����, ���� ��� Asteroid.",
                              new AsteroidBehaviour(player, openCards)),
                    new Card("Inter cloud", 1, "��������� �� 1 ��. ��� ������������� �����.",
                              new InterCloudBehaviour(player, openCards)),
                    new Card("Satellite", 2, "��������� ��������� ����� �� 1 ��. ���� ������� ����� Planet, �� ��������� � �� 2 ��.",
                              new SatelliteBehaviour(player, openCards)),
                    new Card("Black Hole", 0, "����������� ����� ����� ����������. �������� ����� �������������.",
                              new BlackHoleBehaviour(player, openCards)),
                    new Card("Meteorite", 3, "��������� 1 ��. ����� ������ ����� ����������.",
                              new MeteoriteBehaviour(player, openCards)),
                    new Card("Comet", 2, "���������, ��������� ����� �� 2 ��. ���� ������� ������, �� ��������� � �� 4 ��.",
                              new CometBehaviour(player, openCards)),
                    new Card("Planet", 5, "��������� �����."),
                    new Card("Star", 10, "��������� ����� ������."),

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
