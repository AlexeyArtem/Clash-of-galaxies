using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using TMPro;

public class PlayerGameResultView : MonoBehaviour, IPlayerGameResultView
{
    public TextMeshProUGUI GamePoints, RoundsWins;

    public void SetRoundsWins(int roundsWins)
    {
        RoundsWins.text = roundsWins.ToString();
    }

    public void SetTotalGamePoints(int totalGamePoints)
    {
        GamePoints.text = totalGamePoints.ToString();
    }
}
