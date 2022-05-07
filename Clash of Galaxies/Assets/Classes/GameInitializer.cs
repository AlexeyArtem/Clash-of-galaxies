using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Models;
using Assets.Presenters;
using Assets.Views;
using System.Collections.ObjectModel;

public class GameInitializer : MonoBehaviour
{
    private Game game;
    private List<IUnsubscribing> unsubscribings;

    public GameObject TimerTextObj, ResultPanelObj, DeckUserObj, DeckEnemyObj;

    void Awake()
    {
        Settings settings = SaveSettingsScr.CurrentSettings == null ? new Settings() : SaveSettingsScr.CurrentSettings;

        Player playerUser = new Player(settings.UserName);
        PlayerEnemy playerEnemy = new PlayerEnemy(Application.dataPath + "/StreamingAssets/cards info.xml");
        game = new Game(playerUser, playerEnemy, settings);

        PlayerView playerView = FindObjectOfType<PlayerView>();
        PlayerPresenter playerUserPresenter = new PlayerPresenter(playerUser, playerView);

        GameBoardView gameBoardView = FindObjectOfType<GameBoardView>();
        GameBoardPresenter gameBoardPresenter = new GameBoardPresenter(game.GameBoard, gameBoardView);

        PlayerEnemyView playerEnemyView = FindObjectOfType<PlayerEnemyView>();
        PlayerPresenter playerEnemyPresenter = new PlayerPresenter(playerEnemy, playerEnemyView);

        PlayerGameResultPresenter playerGameResultPresenterUser = new PlayerGameResultPresenter(game.PlayersResults[playerUser], playerView.GetComponentInChildren<PlayerGameResultView>());
        PlayerGameResultPresenter playerGameResultPresenterEnemy = new PlayerGameResultPresenter(game.PlayersResults[playerEnemy], playerEnemyView.GetComponentInChildren<PlayerGameResultView>());

        TimerPresenter timerPresenter = new TimerPresenter(game, TimerTextObj.GetComponent<TimerView>());
        ResultPresenter resultPresenter = new ResultPresenter(game, ResultPanelObj.GetComponent<ResultPanelView>());

        DeckCardsPresenter deckPresenterUser = new DeckCardsPresenter(game.Decks[playerUser], DeckUserObj.GetComponent<DeckCardsView>());
        DeckCardsPresenter deckPresenterEnemy = new DeckCardsPresenter(game.Decks[playerEnemy], DeckEnemyObj.GetComponent<DeckCardsView>());

        unsubscribings = new List<IUnsubscribing>()
        {
            playerUserPresenter, playerEnemyPresenter, deckPresenterEnemy, deckPresenterUser,
            gameBoardPresenter, resultPresenter, timerPresenter, 
            playerGameResultPresenterUser, playerGameResultPresenterEnemy
        };

        game.StartNewRound();
        unsubscribings.AddRange(CardPresenterFactory.GetInstance().Values);
    }

    void OnDestroy()
    {
        CardPresenterFactory.GetInstance().Clear();
        foreach (var obj in unsubscribings)
            obj.Unsubscribe();
        unsubscribings.Clear();

        game.OnEndGame(null);
    }
}
