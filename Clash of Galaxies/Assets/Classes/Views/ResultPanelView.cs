using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ResultPanelView : MonoBehaviour, IResultView
{
    private float time = 20;
    private bool isEndRound = false;

    public TextMeshProUGUI GameResultText, RoundResultText, TimeToNextRoundText;
    public GameObject EndGameWindow, EndRoundWindow;

    void Update()
    {
        if (isEndRound && time > 0)
        {
            time -= Time.fixedDeltaTime;
            TimeToNextRoundText.text = Math.Round(time).ToString();
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
        EndRoundWindow.SetActive(false);
        gameObject.SetActive(true);
        EndGameWindow.SetActive(true);

        Animator animator = EndGameWindow.GetComponent<Animator>();
        animator.Play("ScaledDisplaying");

        Time.timeScale = 0;
    }

    public void ShowResultRound(RoundResult roundResult, int roundNumber)
    {
        string text = "";
        switch (roundResult) 
        {
            case RoundResult.WinUser:
                text = "Поздравляем! Вы победили в " + roundNumber + " раунде!";
                break;
            case RoundResult.WinEnemy:
                text = "К сожалению, Вы проиграли в " + roundNumber + " раунде!"; ;
                break;
            case RoundResult.Draw:
                text = "Ничья в " + roundNumber + " раунде!";
                break;
        }

        RoundResultText.text = text;
        gameObject.SetActive(true);
        EndRoundWindow.SetActive(true);

        Animator animator = EndRoundWindow.GetComponent<Animator>();
        animator.Play("ScaledDisplaying");

        isEndRound = true;
        Time.timeScale = 0f;
    }

    public void MainMenuClick() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}
