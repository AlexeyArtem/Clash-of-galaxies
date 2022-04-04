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
        private IReadOnlyCollection<CardPresenter> cardPresenters;

        public GameBoardPresenter(GameBoard gameBoard, IGameBoardView gameBoardView, IReadOnlyCollection<CardPresenter> cardPresenters)
        {
            this.gameBoard = gameBoard;
            this.gameBoardView = gameBoardView;
            this.cardPresenters = cardPresenters;
            gameBoard.NewOpenCard += GameBoard_NewOpenCard;
        }

        public GameBoardPresenter(GameBoard gameBoard, IGameBoardView gameBoardView)
        {
            this.gameBoard = gameBoard;
            this.gameBoardView = gameBoardView;
            gameBoard.NewOpenCard += GameBoard_NewOpenCard;
        }

        private void GameBoard_NewOpenCard(object sender, MakeMoveEventArgs args)
        {
            //CardPresenter cardPresenter = cardPresenters.Where(c => c.Card == args.Card)?.FirstOrDefault();
            //if (cardPresenter != null) 
            //{
            //    gameBoardView.SetCardView(cardPresenter.CardView);
            //}
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
