using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Views;

public class TimerView : MonoBehaviour, ITimerView
{
    private int timeToMove;

    public TextMeshProUGUI TimerText;
    public Action<int> CheckStateMakeMoveCallback { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CheckStateMakeMove() 
    {
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
}
