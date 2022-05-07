using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class TimerPresenter : IUnsubscribing
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
        }

        private void Game_StoppedRound(object sender, EventArgs e)
        {
            timerView.StopTimer();
        }

        private void Game_ChangeMakeMove(object sender, EventArgs e)
        {
            bool isPlayerUserMove = true;
            if (game.PlayerCurrentMove is PlayerEnemy)
                isPlayerUserMove = false;

            timerView.StopTimer();
            timerView.StartTimer(isPlayerUserMove);
        }

        public void CheckStateMakeMove(int timeToMoveInSeconds) 
        {
            game.CheckStateMakeMove(timeToMoveInSeconds);
        }

        public void Unsubscribe()
        {
            timerView.CheckStateMakeMoveCallback = null;
            game.ChangedMakeMove -= Game_ChangeMakeMove;
            game.EndRound -= Game_StoppedRound;
        }
    }
}
