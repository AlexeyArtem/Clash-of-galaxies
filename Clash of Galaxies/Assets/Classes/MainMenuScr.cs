using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScr : MonoBehaviour
{
    public GameObject BackMenuBG, GameRulesPanel, SettingsPanel;

    void Start()
    {
        SaveSettingsScr.LoadSettings();
    }

    public void NewGameClick() 
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGameClick() 
    {
        Application.Quit();
    }

    public void SettingsClick() 
    {
        BackMenuBG.SetActive(true);
        SettingsPanel.SetActive(true);
    }

    public void RulesClick() 
    {
        BackMenuBG.SetActive(true);
        GameRulesPanel.SetActive(true);
    }

    public void BackMenuClick() 
    {
        GameRulesPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        BackMenuBG.SetActive(false);
    }
}
