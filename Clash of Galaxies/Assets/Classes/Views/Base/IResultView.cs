using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IResultView
    {
        void ShowResultRound(RoundResult roundResult, int winRoundNumber);
        void ShowResultGame(bool isPlayerUserWin);
    }
}
