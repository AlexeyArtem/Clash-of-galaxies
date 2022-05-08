using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScr : MonoBehaviour
{
    private bool isPause = false;
    public GameObject PausePanel, BackPanelBG, GameRulesPanel;

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

    public void ExitToPauseMenu() 
    {
        BackPanelBG.SetActive(false);
        GameRulesPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ShowGameRules() 
    {
        BackPanelBG.SetActive(true);
        GameRulesPanel.SetActive(true);
    }

    public void ExitMainMenu() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuScene");
    }

    public void ExitGame() 
    {
        Application.Quit();
    }
}
