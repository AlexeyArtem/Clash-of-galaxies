using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Views;

namespace Assets.Presenters
{
    class GameBoardPresenter
    {
        private GameBoard gameBoard;
        private IGameBoardView gameBoardView;

        public GameBoardPresenter(GameBoard gameBoard, IGameBoardView gameBoardView)
        {
            this.gameBoard = gameBoard;
            this.gameBoardView = gameBoardView;
            gameBoard.NewOpenCard += GameBoard_NewOpenCard;
        }

        private void GameBoard_NewOpenCard(object sender, MakeMoveEventArgs args)
        {
            CardPresenter cardPresenter = CardPresenterFactory.GetInstance().FindPresenter(args.Card);
            if (cardPresenter != null)
            {
                if (args.Player == gameBoard.PlayerA)
                    gameBoardView.SetCardViewPlayerA(cardPresenter.CardView);

                else if (args.Player == gameBoard.PlayerB)
                    gameBoardView.SetCardViewPlayerB(cardPresenter.CardView);
            }
        }
    }
}
