using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Models;
using Assets.Presenters;
using Assets.Views;
using System.Collections.ObjectModel;

public class GameController : MonoBehaviour
{
    private Game game;
    private Player playerUser;
    private PlayerEnemy playerEnemy;

    private PlayerPresenter playerUserPresenter, playerEnemyPresenter;
    private GameBoardPresenter gameBoardPresenterUser, gameBoardPresenterEnemy;
    private PlayerGameResultPresenter playerGameResultPresenterUser, playerGameResultPresenterEnemy;
    private TimerPresenter timerPresenter;
    private ResultPresenter resultPresenter;
    private DeckCardsPresenter deckPresenterUser, deckPresenterEnemy;

    public GameObject TimerTextObj, ResultPanelObj, DeckUserObj, DeckEnemyObj;

    void Awake()
    {
        playerUser = new Player("First player");
        playerEnemy = new PlayerEnemy("Second player", "Assets/Resources/cards info.xml");
        game = new Game(playerUser, playerEnemy);

        PlayerView playerView = FindObjectOfType<PlayerView>();
        playerUserPresenter = new PlayerPresenter(playerUser, playerView);

        GameBoardView gameBoardView = FindObjectOfType<GameBoardView>();
        GameBoardPresenter gameBoardPresenter = new GameBoardPresenter(game.GameBoard, gameBoardView);

        PlayerEnemyView playerEnemyView = FindObjectOfType<PlayerEnemyView>();
        playerEnemyPresenter = new PlayerPresenter(playerEnemy, playerEnemyView);

        playerGameResultPresenterUser = new PlayerGameResultPresenter(game.PlayersResults[playerUser], playerView.GetComponentInChildren<PlayerGameResultView>());
        playerGameResultPresenterEnemy = new PlayerGameResultPresenter(game.PlayersResults[playerEnemy], playerEnemyView.GetComponentInChildren<PlayerGameResultView>());

        timerPresenter = new TimerPresenter(game, TimerTextObj.GetComponent<TimerView>());
        resultPresenter = new ResultPresenter(game, ResultPanelObj.GetComponent<ResultPanelView>());

        deckPresenterUser = new DeckCardsPresenter(game.Decks[playerUser], DeckUserObj.GetComponent<DeckCardsView>());
        deckPresenterEnemy = new DeckCardsPresenter(game.Decks[playerEnemy], DeckEnemyObj.GetComponent<DeckCardsView>());

        game.StartNewRound();
    }
}
