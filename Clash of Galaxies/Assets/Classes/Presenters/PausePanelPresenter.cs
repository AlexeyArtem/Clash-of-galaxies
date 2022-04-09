using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class PausePanelPresenter
    {
        private Game game;
        private IPausePanelView pausePanelView;

        public PausePanelPresenter(Game game, IPausePanelView pausePanelView)
        {
            this.game = game;
            this.pausePanelView = pausePanelView;
            this.game.EndRound += Game_EndRound;
            this.game.EndGame += Game_EndGame;
        }

        private void Game_EndGame(object sender, EndRoundEventArgs e)
        {
            bool isPlayerUserWin = true;
            
            if (e.WinPlayer is PlayerEnemy)
                isPlayerUserWin = false;

            pausePanelView.ShowResultGame(isPlayerUserWin);
        }

        private void Game_EndRound(object sender, EndRoundEventArgs e)
        {
            bool isPlayerUserWin = true;

            if (e.WinPlayer is PlayerEnemy)
                isPlayerUserWin = false;

            pausePanelView.ShowResultRound(isPlayerUserWin, game.CurrentRound - 1);
        }
    }
}
