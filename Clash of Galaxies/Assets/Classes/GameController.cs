using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Models;
using Assets.Presenters;

public class GameController : MonoBehaviour
{
    private Game game;
    private Player playerUser, playerEnemy;

    private PlayerPresenter playerUserPresenter, playerEnemyPresenter;
    private GameBoardPresenter gameBoardPresenterUser, gameBoardPresenterEnemy;
    
    // All controllers next level:
    // - Game board controller
    // - Players controllers
    //

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

        GameBoardView gameBoardView = GameObject.Find("PlayerUserUI").GetComponentInChildren<GameBoardView>();
        gameBoardPresenterUser = new GameBoardPresenter(game.GameBoard, gameBoardView, playerUserPresenter.CardPresenters);

        PlayerEnemyView playerEnemyView = FindObjectOfType<PlayerEnemyView>();
        playerEnemyPresenter = new PlayerPresenter(playerEnemy, playerEnemyView);

        GameBoardView gameBoardEnemyView = GameObject.Find("PlayerEnemyUI").GetComponentInChildren<GameBoardView>();
        gameBoardPresenterEnemy = new GameBoardPresenter(game.GameBoard, gameBoardEnemyView, playerEnemyPresenter.CardPresenters);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
