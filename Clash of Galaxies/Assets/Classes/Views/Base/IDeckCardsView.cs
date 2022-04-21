using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IDeckCardsView
    {
        void SetDeckCards(List<ICardView> cardViews);
    }
}
