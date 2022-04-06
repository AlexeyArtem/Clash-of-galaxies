using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IPlayerGameResultView
    {
        void SetTotalGamePoints(int totalGamePoints);
        void SetRoundsWins(int roundsWins);
    }
}
