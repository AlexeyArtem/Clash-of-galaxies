using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Collections;

namespace Assets.Models
{
    public class Game
    {
        private static System.Random rand = new System.Random();

        private Player playerA, playerB;
        private GameInfo gameInfo;
        private Dictionary<Player, Stack<Card>> decks;
        private Dictionary<Player, PlayerGameResult> playersResults;

        private Timer moveTimer;
        private int currentMoveTime;
        private Player playerCurrentMove;
        private System.Timers.Timer timer;

        public Game(Player playerA, Player playerB)
        {
            if (playerA == playerB)
                throw new ArgumentException("The players is match");

            this.playerA = playerA;
            this.playerB = playerB;
            GameBoard = new GameBoard(playerA, playerB);
            Cards = Cards.GetInstance(GameBoard.OpenCards);

            decks = new Dictionary<Player, Stack<Card>>()
            {
                { playerA, new Stack<Card>() },
                { playerB, new Stack<Card>() },
            };
            Decks = new ReadOnlyDictionary<Player, Stack<Card>>(decks);
            playersResults = new Dictionary<Player, PlayerGameResult>()
            {
                {playerA, new PlayerGameResult() },
                {playerB, new PlayerGameResult() },
            };
            PlayersResults = new ReadOnlyDictionary<Player, PlayerGameResult>(playersResults);

            PermissionMakeMove += playerA.SetPermissionToMove;
            PermissionMakeMove += playerB.SetPermissionToMove;
            DealCards += playerA.SetCardsInHand;
            DealCards += playerB.SetCardsInHand;
        }

        public event EventHandler ChangeMakeMove;

        public int CurrentRound { get; private set; } = 1;
        public Cards Cards { get; }
        public GameBoard GameBoard { get; }
        public IReadOnlyDictionary<Player, Stack<Card>> Decks { get; }
        public IReadOnlyDictionary<Player, PlayerGameResult> PlayersResults { get; }

        private event PermissionMakeMoveEventHandler PermissionMakeMove;
        private event DealCardsEventHandler DealCards;

        private void GenerateDeck(Player player)
        {
            if (decks[player].Count > 0) return;

            Stack<Card> deckCards = new Stack<Card>();
            for (int i = 0; i < GameRules.MaxCardsInDeck; i++)
            {
                int index = rand.Next(0, Cards.Count);
                Card card = Cards.Get(player, index);
                deckCards.Push(card);
            }
            decks[player] = deckCards;
        }

        private void OnChangeMakeMove() 
        {
            ChangeMakeMove?.Invoke(this, new EventArgs());
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
            moveTimer?.Dispose();
            OnPermissionMakeMove(playerA, false);
            OnPermissionMakeMove(playerB, false);
        }

        public void RefreshPlayersResults()
        {
            playersResults[playerA].TotalGamePoints = GameBoard.GetTotalGamePoints(playerA);
            playersResults[playerB].TotalGamePoints = GameBoard.GetTotalGamePoints(playerB);
        }

        // Вызывать метод каждую секунду внутри корутины в UI
        public void CheckStateMakeMove(int timeToMoveInSeconds)
        {
            RefreshPlayersResults();
            if (timeToMoveInSeconds > GameRules.MaxTimeToMoveInSeconds || playerCurrentMove.IsMoveCompleted)
            {
                //CheckRound();

                if (playerCurrentMove == playerA)
                {
                    OnPermissionMakeMove(playerA, false);
                    AllowMove(playerB);
                }
                else
                {
                    OnPermissionMakeMove(playerB, false);
                    AllowMove(playerA);
                }
            }
        }

        private void AllowMove(Player player)
        {
            OnPermissionMakeMove(player, true);
            playerCurrentMove = player;
            OnChangeMakeMove();
        }

        public void CheckRound()
        {
            if (decks[playerA].Count != 0 || decks[playerB].Count != 0) return;

            if (GameBoard.GetTotalGamePoints(playerA) > GameBoard.GetTotalGamePoints(playerB)) playersResults[playerA].AddRoundWin();
            else if (GameBoard.GetTotalGamePoints(playerA) < GameBoard.GetTotalGamePoints(playerB)) playersResults[playerB].AddRoundWin();
            CurrentRound += 1;
            // Вызов события конца раунда (остановка корутины)

            if (playersResults[playerA].RoundsWins != playersResults[playerB].RoundsWins && CurrentRound >= GameRules.MaxRounds)
            {
                OnEndGame();
                return;
            }

            // Вызов начало следующего раунда (старт корутины)
        }

        public void StartRound()
        {
            GenerateDeck(playerA);
            GenerateDeck(playerB);

            OnDealCards(playerA, GameRules.MaxStartPlayerCards);
            OnDealCards(playerB, GameRules.MaxStartPlayerCards);

            // На время тестирования
            //OnPermissionMakeMove(playerA, true);
            //OnPermissionMakeMove(playerB, true);
            
            AllowMove(playerA);
        }
    }
}
