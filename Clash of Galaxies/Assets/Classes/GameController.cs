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
    private Player playerUser, playerEnemy;

    private PlayerPresenter playerUserPresenter, playerEnemyPresenter;
    private GameBoardPresenter gameBoardPresenterUser, gameBoardPresenterEnemy;
    private PlayerGameResultPresenter playerGameResultPresenterUser, playerGameResultPresenterEnemy;
    private TimerPresenter timerPresenter;

    public GameObject GameResultUserObj, GameResultEnemyObj, TimerTextObj;

    private void Awake()
    {
        playerUser = new Player("First player");
        playerEnemy = new Player("Second player");
        game = new Game(playerUser, playerEnemy);
        game.StartRound();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerView playerView = FindObjectOfType<PlayerView>();
        playerUserPresenter = new PlayerPresenter(playerUser, playerView);

        GameBoardView gameBoardView1 = FindObjectOfType<GameBoardView>();
        GameBoardPresenter gameBoardPresenter = new GameBoardPresenter(game.GameBoard, gameBoardView1);

        //GameBoardView gameBoardView = GameObject.Find("SelfField").GetComponent<GameBoardView>();
        //gameBoardPresenterUser = new GameBoardPresenter(game.GameBoard, gameBoardView, playerUserPresenter.CardPresentersInHand);

        PlayerEnemyView playerEnemyView = FindObjectOfType<PlayerEnemyView>();
        playerEnemyPresenter = new PlayerPresenter(playerEnemy, playerEnemyView);

        playerGameResultPresenterUser = new PlayerGameResultPresenter(game.PlayersResults[playerUser], GameResultUserObj.GetComponent<PlayerGameResultView>());
        playerGameResultPresenterEnemy = new PlayerGameResultPresenter(game.PlayersResults[playerEnemy], GameResultEnemyObj.GetComponent<PlayerGameResultView>());

        timerPresenter = new TimerPresenter(game, TimerTextObj.GetComponent<TimerView>());

        //GameBoardView gameBoardEnemyView = GameObject.Find("EnemyField").GetComponent<GameBoardView>();
        //gameBoardPresenterEnemy = new GameBoardPresenter(game.GameBoard, gameBoardEnemyView, playerEnemyPresenter.CardPresentersInHand);

        // Для теста
        //playerEnemy.OnMakeMove(playerEnemy.CardsInHand[0]);
        //playerEnemy.OnMakeMove(playerEnemy.CardsInHand[0]);
        //playerUser.OnMakeMove(playerUser.CardsInHand[0]);
        //game.RefreshPlayersResults();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
