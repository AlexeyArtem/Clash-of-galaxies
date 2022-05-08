using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using TMPro;

public class PlayerView : MonoBehaviour, IPlayerView
{
    private Sequence sequence;

    public GameObject Hand;
    public TextMeshProUGUI PlayerName;

    public Action<ICardView> DropCardCallback { get; set; }
    public Action<ICardView> PlayCurrentCardCallback { get; set; }
    public Action EndMakeMoveCallback { get; set; }

    private Vector3 CalculateMovingPosition() 
    {
        Vector3 movingPosition = transform.position;
        var childCount = Hand.transform.childCount;
        if (childCount > 1)
        {
            Transform lastChild = Hand.transform.GetChild(Hand.transform.childCount - 1);
            Transform preLastChild = Hand.transform.GetChild(Hand.transform.childCount - 2);

            movingPosition = lastChild.position;
            float offset = lastChild.position.x - preLastChild.position.x;
            movingPosition.x += offset;
        }
        return movingPosition;
    }

    public void SetName(string name) 
    {
        PlayerName.text = name;
    }

    public virtual void SetCardViews(ICollection<ICardView> cardViews) 
    {
        if (cardViews.Count == 0) return;

        foreach (var cardView in cardViews)
        {
            MonoBehaviour objectCardView = cardView as MonoBehaviour;
            objectCardView.gameObject.SetActive(true);

            Vector3 movingPosition = CalculateMovingPosition();
            objectCardView.transform.DOMove(movingPosition, .7f).OnComplete(() =>
            {
                objectCardView.transform.SetParent(Hand.transform);
            });

            cardView.SetActiveCardShirt(false);
        }
    }

    public void CompleteMoveClick() 
    {
        EndMakeMoveCallback?.Invoke();
    }
}
