using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class Game
    {
        private static Random rand = new Random();

        private Player playerA, playerB;
        private GameInfo gameInfo;
        private Dictionary<Player, Stack<Card>> decks;
        private GameBoard gameBoard;
        public Cards cards; //public на время теста

        private Timer moveTimer;
        private int currentMoveTime;
        private Player playerCurrentMove;

        public Game(Player playerA, Player playerB)
        {
            if (playerA == playerB)
                throw new ArgumentException("The players match");

            this.playerA = playerA;
            this.playerB = playerB;
            gameBoard = new GameBoard(playerA, playerB);
            cards = Cards.GetInstance(gameBoard.OpenCards);

            decks = new Dictionary<Player, Stack<Card>>()
            {
                { playerA, new Stack<Card>() },
                { playerB, new Stack<Card>() },
            };

            PermissionMakeMove += playerA.GetPermissionToMove;
            PermissionMakeMove += playerB.GetPermissionToMove;
            DealCards += playerA.GetCardsInHand;
            DealCards += playerB.GetCardsInHand;
        }

        public GameBoard GameBoard 
        {
            get 
            {
                return gameBoard;
            }
        }

        private event PermissionMakeMoveEventHandler PermissionMakeMove;
        private event DealCardsEventHandler DealCards;

        private void GenerateDeck(Player player)
        {
            if (decks[player].Count > 0) return;

            Stack<Card> deckCards = new Stack<Card>();
            for (int i = 0; i < GameRules.MaxCardsInDeck; i++)
            {
                int index = rand.Next(0, cards.Count);
                Card card = cards.Get(player, index);
                deckCards.Push(card);
            }
            decks[player] = deckCards;
        }

        private void OnPermissionMakeMove(Player player, bool isPermissionMakeMove)
        {
            var args = new PermissionMakeMoveEventArgs(player, isPermissionMakeMove);
            PermissionMakeMove?.Invoke(this, args);
        }

        private void OnDealCards(Player player, int countCards)
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < countCards; i++)
            {
                try
                {
                    cards.Add(decks[player].Pop());
                }
                catch(InvalidOperationException)
                {

                }
            }
            var args = new DealCardsEventArgs(player, cards);
            DealCards?.Invoke(this, args);
        }

        private void OnEndGame()
        {
            moveTimer.Dispose();
            OnPermissionMakeMove(playerA, false);
            OnPermissionMakeMove(playerB, false);
        }

        private void CheckStateMoveCallback(object state)
        {
            if (currentMoveTime >= GameRules.TimeToMoveInSeconds || playerCurrentMove.IsMoveCompleted)
            {
                CheckRound();

                if (playerCurrentMove == playerA)
                {
                    OnPermissionMakeMove(playerA, false);
                    AllowMove(playerB);
                }
                else if (playerCurrentMove == playerB)
                {
                    OnPermissionMakeMove(playerB, false);
                    AllowMove(playerA);
                }

                return;
            }
            currentMoveTime += 1;
        }

        private void AllowMove(Player player)
        {
            OnPermissionMakeMove(player, true);
            playerCurrentMove = player;
            moveTimer?.Dispose();
            currentMoveTime = 0;
            moveTimer = new Timer(CheckStateMoveCallback, null, 0, 1000);
        }

        public void CheckRound()
        {
            if (decks[playerA].Count != 0 || decks[playerB].Count != 0) return;

            if (gameBoard.GetGamePoints(playerA) > gameBoard.GetGamePoints(playerB)) gameInfo.AddRoundWin(playerA);
            else if (gameBoard.GetGamePoints(playerA) < gameBoard.GetGamePoints(playerB)) gameInfo.AddRoundWin(playerB);
            else gameInfo.AddRoundDraw();

            if (gameInfo.RoundWins[playerA] != gameInfo.RoundWins[playerB] && gameInfo.CurrentRound >= GameRules.MaxRounds)
            {
                OnEndGame();
                return;
            }

            StartRound();
        }

        public void StartRound()
        {
            GenerateDeck(playerA);
            GenerateDeck(playerB);

            OnDealCards(playerA, GameRules.MaxStartPlayerCards);
            OnDealCards(playerB, GameRules.MaxStartPlayerCards);

            OnPermissionMakeMove(playerA, true);

            //AllowMove(playerA);
        }
    }
}
