using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Наследовать от MenuScr
public class PauseMenuScr : MonoBehaviour
{
    private bool isPause = false;
    public GameObject PausePanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPause == false)
        {
            isPause = true;
            Time.timeScale = 0;
            PausePanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPause == true)
        {
            Resume();
        }
    }

    public void Resume() 
    {
        isPause = false;
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
    }

    public void ShowGameRules() 
    {

    }

    public void ExitMainMenu() 
    {

    }

    public void ExitGame() 
    {
        Application.Quit();
    }
}
