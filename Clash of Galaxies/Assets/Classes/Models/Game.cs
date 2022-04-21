using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace Assets.Models
{
    public class Game
    {
        private static Random rand = new Random();

        private Player playerA, playerB;
        private Dictionary<Player, ObservableStack<Card>> decks;
        private Dictionary<Player, PlayerGameResult> playersResults;

        private Player playerCurrentMove;

        public Game(Player playerA, Player playerB)
        {
            if (playerA == playerB)
                throw new ArgumentException("The players is match");

            this.playerA = playerA;
            this.playerB = playerB;
            GameBoard = new GameBoard(playerA, playerB);
            Cards = new Cards(GameBoard.OpenCards);

            decks = new Dictionary<Player, ObservableStack<Card>>()
            {
                { playerA, new ObservableStack<Card>() },
                { playerB, new ObservableStack<Card>() },
            };
            Decks = new ReadOnlyDictionary<Player, ObservableStack<Card>>(decks);

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

        public int CurrentRound { get; private set; } = 1;
        public Cards Cards { get; }
        public GameBoard GameBoard { get; }
        public IReadOnlyDictionary<Player, ObservableStack<Card>> Decks { get; }
        public IReadOnlyDictionary<Player, PlayerGameResult> PlayersResults { get; }

        private event PermissionMakeMoveEventHandler PermissionMakeMove;
        private event DealCardsEventHandler DealCards;
        public event DealCardsEventHandler NewCardsInDeck;
        public event EventHandler ChangedMakeMove;
        public event EndEventHandler EndRound;
        public event EndEventHandler EndGame;

        private void OnChangeMakeMove() 
        {
            ChangedMakeMove?.Invoke(this, new EventArgs());
        }

        private void OnEndRound(Player winPlayer) 
        {
            EndRound?.Invoke(this, new EndEventArgs(winPlayer));
        }

        private void OnPermissionMakeMove(Player player, bool isPermissionMakeMove)
        {
            var args = new PermissionMakeMoveEventArgs(player, isPermissionMakeMove);
            PermissionMakeMove?.Invoke(this, args);
        }

        private void OnNewCardsInDeck(Player player, List<Card> cards) 
        {
            NewCardsInDeck?.Invoke(this, new DealCardsEventArgs(player, cards));
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
                catch (InvalidOperationException)
                {

                }
            }
            var args = new DealCardsEventArgs(player, cards);
            DealCards?.Invoke(this, args);
        }

        public void OnEndGame(Player winPLayer)
        {
            OnPermissionMakeMove(playerA, false);
            OnPermissionMakeMove(playerB, false);
            // Вызов события окончания игры
            EndGame?.Invoke(this, new EndEventArgs(winPLayer));

            // Отписка от всех событий
            PermissionMakeMove = null;
            DealCards = null;
            ChangedMakeMove = null;
            EndRound = null;
            EndGame = null;
            NewCardsInDeck = null;
        }

        private void GenerateDeck(Player player)
        {
            if (decks[player].Count > 0) return;

            //ObservableStack<Card> deckCards = new ObservableStack<Card>();
            decks[player].Clear();
            for (int i = 0; i < GameRules.MaxCardsInDeck; i++)
            {
                int index = rand.Next(0, Cards.Count);
                Card card = Cards.Get(player, index);
                decks[player].Push(card);
                //deckCards.Push(card);
            }
            //decks[player] = deckCards;
            //OnNewCardsInDeck(player, deckCards.ToList());
        }

        private void AllowMove(Player player)
        {
            OnPermissionMakeMove(player, true);
            playerCurrentMove = player;
            OnChangeMakeMove();
        }

        public void RefreshPlayersResults()
        {
            playersResults[playerA].TotalGamePoints = GameBoard.GetTotalGamePoints(playerA);
            playersResults[playerB].TotalGamePoints = GameBoard.GetTotalGamePoints(playerB);
        }

        public void CheckStateMakeMove(int timeToMoveInSeconds)
        {
            RefreshPlayersResults();
            if (timeToMoveInSeconds > GameRules.MaxTimeToMoveInSeconds || playerCurrentMove.IsMoveCompleted)
            {
                Player winPlayerRound = null;
                if (CheckEndRound(ref winPlayerRound)) 
                {
                    Player winPlayerGame = null;
                    if (CheckEndGame(ref winPlayerGame)) 
                    {
                        OnEndGame(winPlayerGame);
                        return;
                    }
                    OnEndRound(winPlayerRound);
                    StartNewRound();
                    return;
                }

                if (playerCurrentMove == playerA)
                {
                    OnPermissionMakeMove(playerA, false);
                    AllowMove(playerB);
                    OnDealCards(playerA, 1);
                }
                else
                {
                    OnPermissionMakeMove(playerB, false);
                    AllowMove(playerA);
                    OnDealCards(playerB, 1);
                }
            }
        }

        public bool CheckEndRound(ref Player winPlayer)
        {
            if (decks[playerA].Count == 0 && decks[playerB].Count == 0) 
            {
                if (playersResults[playerA].TotalGamePoints > playersResults[playerB].TotalGamePoints)
                {
                    playersResults[playerA].AddRoundWin();
                    winPlayer = playerA;
                }
                else if (playersResults[playerA].TotalGamePoints < playersResults[playerB].TotalGamePoints)
                {
                    playersResults[playerB].AddRoundWin();
                    winPlayer = playerB;
                }
                else 
                {
                    playersResults[playerA].AddRoundWin();
                    playersResults[playerB].AddRoundWin();
                }
                CurrentRound += 1;
                return true;
            }
            return false;
        }

        public bool CheckEndGame(ref Player winPlayer) 
        {
            if (playersResults[playerA].RoundsWins != playersResults[playerB].RoundsWins && CurrentRound >= GameRules.MaxRounds) 
            {
                if (playersResults[playerA].RoundsWins > playersResults[playerB].RoundsWins)
                    winPlayer = playerA;
                else
                    winPlayer = playerB;

                return true;
            }

            return false;
        }

        public void StartNewRound()
        {
            playerA.ClearCardsInHand();
            playerB.ClearCardsInHand();
            GameBoard.ClearOpenCards();

            GenerateDeck(playerA);
            GenerateDeck(playerB);

            OnDealCards(playerA, GameRules.MaxStartPlayerCards);
            OnDealCards(playerB, GameRules.MaxStartPlayerCards);

            int randNum = rand.Next(1, 3);
            Player player = randNum == 1 ? playerA : playerB;
            AllowMove(player);
        }
    }
}
