using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;

namespace Assets.Controllers
{
    class CardController
    {
        private Card card;
        private CardView cardView;

        public CardController(Card card, CardView cardView)
        {
            this.card = card;
            this.cardView = cardView;
            cardView.SetCardInfo(card.Name, card.GamePoints, card.InfluenceGamePoints);
        }
    }
}