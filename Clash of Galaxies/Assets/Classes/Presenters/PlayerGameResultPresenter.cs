using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class PlayerGameResultPresenter
    {
        private PlayerGameResult playerGameResult;
        private IPlayerGameResultView playerGameResultView;

        public PlayerGameResultPresenter(PlayerGameResult playerGameResult, IPlayerGameResultView playerGameResultView)
        {
            this.playerGameResult = playerGameResult;
            this.playerGameResultView = playerGameResultView;
            playerGameResultView.SetRoundsWins(playerGameResult.RoundsWins);
            playerGameResultView.SetTotalGamePoints(playerGameResult.TotalGamePoints);
            playerGameResult.PropertyChanged += PlayerGameResult_PropertyChanged;
        }

        private void PlayerGameResult_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            playerGameResultView.SetRoundsWins(playerGameResult.RoundsWins);
            playerGameResultView.SetTotalGamePoints(playerGameResult.TotalGamePoints);
        }
    }
}
