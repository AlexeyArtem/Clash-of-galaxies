using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;

namespace Assets.Controllers
{
    class GameBoardController
    {
        private GameBoard gameBoard;
        private GameBoardView gameBoardView;
        private PlayerController playerController;

        public GameBoardController(GameBoard gameBoard, GameBoardView gameBoardView, PlayerController playerController)
        {
            this.gameBoard = gameBoard;
            this.gameBoardView = gameBoardView;
            this.playerController = playerController;
            gameBoard.NewOpenCard += GameBoard_NewOpenCard;
        }

        private void GameBoard_NewOpenCard(object sender, MakeMoveEventArgs args)
        {
            CardController cardController = playerController.CardControllers.Where(c => c.Card == args.Card).First();
            if (cardController != null) 
            {
                gameBoardView.SetCardView(cardController.CardView);
            }
        }
    }
}
