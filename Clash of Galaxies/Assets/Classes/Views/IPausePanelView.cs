using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Views
{
    interface IPausePanelView
    {
        void ShowResultRound(bool isPlayerUserWin, int winRoundNumber);
        void ShowResultGame(bool isPlayerUserWin);
    }
}
