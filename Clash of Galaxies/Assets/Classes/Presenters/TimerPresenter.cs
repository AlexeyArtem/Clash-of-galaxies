using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class TimerPresenter
    {
        private Game game;
        private ITimerView timerView;

        public TimerPresenter(Game game, ITimerView timerView)
        {
            this.game = game;
            this.timerView = timerView;
            this.timerView.CheckStateMakeMoveCallback = CheckStateMakeMove;
            this.game.ChangedMakeMove += Game_ChangeMakeMove;
            this.game.EndRound += Game_StoppedRound;

            //timerView.StopTimer();
            //timerView.StartTimer();
        }

        private void Game_StoppedRound(object sender, EventArgs e)
        {
            //timerView.StopTimer();
        }

        private void Game_ChangeMakeMove(object sender, EventArgs e)
        {
            timerView.StopTimer();
            timerView.StartTimer();
        }

        public void CheckStateMakeMove(int timeToMoveInSeconds) 
        {
            game.CheckStateMakeMove(timeToMoveInSeconds);
        }

    }
}
