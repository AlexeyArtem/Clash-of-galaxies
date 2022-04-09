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
    private PausePanelPresenter pausePanelPresenter;

    public GameObject GameResultUserObj, GameResultEnemyObj, TimerTextObj, PausePanelObj;

    private void Awake()
    {
        //playerUser = new Player("First player");
        //playerEnemy = new PlayerEnemy("Second player");
        //game = new Game(playerUser, playerEnemy);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerUser = new Player("First player");
        playerEnemy = new PlayerEnemy("Second player");
        game = new Game(playerUser, playerEnemy);

        PlayerView playerView = FindObjectOfType<PlayerView>();
        playerUserPresenter = new PlayerPresenter(playerUser, playerView);

        GameBoardView gameBoardView = FindObjectOfType<GameBoardView>();
        GameBoardPresenter gameBoardPresenter = new GameBoardPresenter(game.GameBoard, gameBoardView);

        PlayerEnemyView playerEnemyView = FindObjectOfType<PlayerEnemyView>();
        playerEnemyPresenter = new PlayerPresenter(playerEnemy, playerEnemyView);

        playerGameResultPresenterUser = new PlayerGameResultPresenter(game.PlayersResults[playerUser], GameResultUserObj.GetComponent<PlayerGameResultView>());
        playerGameResultPresenterEnemy = new PlayerGameResultPresenter(game.PlayersResults[playerEnemy], GameResultEnemyObj.GetComponent<PlayerGameResultView>());

        timerPresenter = new TimerPresenter(game, TimerTextObj.GetComponent<TimerView>());
        pausePanelPresenter = new PausePanelPresenter(game, PausePanelObj.GetComponent<PausePanelView>());

        game.StartNewRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
