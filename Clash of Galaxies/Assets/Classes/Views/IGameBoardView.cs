using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IGameBoardView
    {
        void SetCardView(ICardView cardView);
    }
}
