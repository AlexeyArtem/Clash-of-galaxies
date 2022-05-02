using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Models;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSettingsScr : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField timeToMove, cardsInHand, cardsInDeck, playerName;
    [SerializeField]
    private TextMeshProUGUI resultSavingTxt, inputErrorTxt;

    private static string savePath = "Assets/Resources/settings.dat";
    public static Settings CurrentSettings { get; private set; }

    void Awake()
    {
        LoadSettings();
        SetCurrentSettings();
    }

    void OnDisable()
    {
        resultSavingTxt.enabled = false;
        inputErrorTxt.enabled = false;
        SetCurrentSettings();
    }

    private void SetCurrentSettings() 
    {
        timeToMove.text = CurrentSettings.MaxTimeToMoveInSeconds.ToString();
        cardsInDeck.text = CurrentSettings.MaxCardsInDeck.ToString();
        cardsInHand.text = CurrentSettings.MaxStartPlayerCards.ToString();
        playerName.text = CurrentSettings.UserName;
    }

    private void ShowError(string message) 
    {
        inputErrorTxt.text = message;
        inputErrorTxt.enabled = true;
    }

    private void HideError() 
    {
        inputErrorTxt.enabled = false;
    }

    private bool CheckForEmpty(TMP_InputField field, string fieldName) 
    {
        if (string.IsNullOrEmpty(field.text))
        {
            ShowError(fieldName + " не может быть пустым");
            return true;
        }
        else
            HideError();
            return false;
    }

    public static void LoadSettings() 
    {
        if (File.Exists(savePath))
        {
            using (FileStream fs = new FileStream(savePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                CurrentSettings = (Settings)formatter.Deserialize(fs);
            }
        }
        else CurrentSettings = new Settings();
    }

    public bool CheckInputPlayerName() 
    {
        resultSavingTxt.enabled = false;
        if (CheckForEmpty(playerName, "Имя игрока")) 
            return true;

        return false;
    }

    public bool CheckInputTimeToMove() 
    {
        bool isError = false;
        if (CheckForEmpty(timeToMove, "Время на ход")) 
        {
            isError = true;
            return isError;
        }

        int time = Convert.ToInt32(timeToMove.text);
        if (time <= 0 || time > 240)
        {
            ShowError("Время на ход должно быть в диапазоне от 1 и до 240");
            isError = true;
        }
        else  HideError();

        return isError;
    }

    public bool CheckInputCardsInHand() 
    {
        bool isError = false;
        if (CheckForEmpty(this.cardsInHand, "Количество стартовых карт"))
        {
            isError = true;
            return isError;
        }

        int.TryParse(this.cardsInDeck.text, out int cardsInDeck);
        int cardsInHand = Convert.ToInt32(this.cardsInHand.text);
        if (cardsInHand <= 0 || cardsInHand > 6)
        {
            ShowError("Количество стартовых карт должно быть в диапазоне от 0 и до 6");
            isError = true;
        }
        else if (cardsInHand >= cardsInDeck) 
        {
            ShowError("Количество стартовых карт должно быть меньше количества карт в колоде");
            isError = true;
        }
        else HideError();

        return isError;
    }

    public bool CheckInputCardsInDeck() 
    {
        bool isError = false;
        if (CheckForEmpty(this.cardsInDeck, "Количество карт в колоде")) 
        {
            isError = true;
            return isError;
        }

        int.TryParse(this.cardsInHand.text, out int cardsInHand);
        int cardsInDeck = Convert.ToInt32(this.cardsInDeck.text);
        if (cardsInDeck <= cardsInHand)
        {
            ShowError("Количество карт в колоде должно быть больше количества стартовых карт");
            isError = true;
        }
        else HideError();

        return isError;
    }

    public void SaveSettings() 
    {
        resultSavingTxt.enabled = false;

        Func<bool>[] checkFunctions = { CheckInputCardsInDeck, CheckInputCardsInHand, CheckInputPlayerName, CheckInputTimeToMove };
        bool isError = false;
        foreach (var func in checkFunctions) 
        {
            isError = func.Invoke();
            if (isError) 
            {
                resultSavingTxt.text = "Не удалось сохранить настройки";
                resultSavingTxt.enabled = true;
                return;
            }
        }

        if (!isError) 
        {
            int time = Convert.ToInt32(timeToMove.text);
            int cardsInHand = Convert.ToInt32(this.cardsInHand.text);
            int cardsInDeck = Convert.ToInt32(this.cardsInDeck.text);
            Settings newSettings = new Settings(cardsInHand, cardsInDeck, time, playerName.text);
            
            using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate)) 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, newSettings);
            }

            CurrentSettings = newSettings;
            resultSavingTxt.text = "Настройки успешно сохранены";
            resultSavingTxt.enabled = true;
        }
    }
}
