using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Models;
using Assets.Controllers;

public class GameController : MonoBehaviour
{
    private Game game;
    private Player playerA, playerB;

    private PlayerController playerControllerA, playerControllerB;
    private GameBoardController gameBoardController;
    
    // All controllers next level:
    // - Game board controller
    // - Players controllers
    //

    private void Awake()
    {
        playerA = new Player("First player");
        playerB = new Player("Second player");
        game = new Game(playerA, playerB);
    }

    // Start is called before the first frame update
    void Start()
    {
        game.StartRound();
        PlayerView playerView = GameObject.Find("PlayerUserUI").GetComponent<PlayerView>();
        playerControllerA = new PlayerController(playerA, playerView);

        GameBoardView gameBoardView = FindObjectOfType<GameBoardView>();
        gameBoardController = new GameBoardController(game.GameBoard, gameBoardView, playerControllerA);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
