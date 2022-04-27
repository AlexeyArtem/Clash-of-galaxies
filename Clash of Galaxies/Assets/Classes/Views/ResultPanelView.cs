using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultPanelView : MonoBehaviour, IResultView
{
    private float time = 10;
    private bool isEndRound = false;

    public TextMeshProUGUI GameResultText;
    public TextMeshProUGUI RoundResultText;
    public GameObject EndGameWindow, EndRoundWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEndRound && time > 0)
        {
            time -= Time.fixedDeltaTime;
        }
        if (time <= 0)
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
            isEndRound = false;
            time = 5;
        }
    }

    public void ShowResultGame(bool isPlayerUserWin)
    {
        string text;
        
        if (isPlayerUserWin)
            text = "Поздравляем! Вы победили!";
        else
            text = "К сожалению, Вы проиграли!";

        GameResultText.text = text;
        gameObject.SetActive(true);
        EndGameWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowResultRound(bool isPlayerUserWin, int roundNumber)
    {
        string text;

        if (isPlayerUserWin)
            text = "Поздравляем! Вы победили в " + roundNumber + " раунде!";
        else
            text = "К сожалению, Вы проиграли в " + roundNumber + " раунде!"; ;

        RoundResultText.text = text;
        gameObject.SetActive(true);
        isEndRound = true;
        Time.timeScale = 0;
    }

    public void MainMenuClick() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
