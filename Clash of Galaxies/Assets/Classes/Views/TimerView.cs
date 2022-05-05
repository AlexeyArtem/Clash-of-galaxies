using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Views;

public class TimerView : MonoBehaviour, ITimerView
{
    private int timeToMove;
    private System.Random random = new System.Random();

    public TextMeshProUGUI CurrentMoveText;
    public TextMeshProUGUI TimerText;

    public Action<int> CheckStateMakeMoveCallback { get; set; }

    public IEnumerator CheckStateMakeMove() 
    {
        int enemyTime = random.Next(2, 11);
        while (true) 
        {
            timeToMove++;
            TimerText.text = timeToMove.ToString();
            CheckStateMakeMoveCallback?.Invoke(timeToMove);
            yield return new WaitForSeconds(1);
        }
    }

    public void StartTimer()
    {
        timeToMove = 0;
        StartCoroutine(CheckStateMakeMove());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    public void StartTimer(bool isPlayerUserMove)
    {
        if (isPlayerUserMove)
            CurrentMoveText.text = "Ваш ход";
        else
            CurrentMoveText.text = "Ход противника";

        StartTimer();
    }
}
