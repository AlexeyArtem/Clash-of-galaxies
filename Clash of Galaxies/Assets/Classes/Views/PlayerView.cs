using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class PlayerView : MonoBehaviour, IPlayerView
{
    public GameObject Hand;
    
    public Action<ICardView> DropCardCallback { get; set; } // Ссылка на метод презентера, в котором вызывается метод выполнения хода в модели
    public Action<ICardView> PlayCurrentCardCallback { get; set; } // Ссылка на метод презентера, в котором вызывается метод взаимодействия с выбранной картой в модели
    public Action EndMakeMoveCallback { get; set; } // Ссылка на метод презентера, в котором вызывается метод заврешения хода в модели

    private Vector3 CalculateMovingPosition(Transform transform) 
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

    public virtual void SetCardViews(ICollection<ICardView> cardViews) 
    {
        if (cardViews.Count == 0) return;

        Sequence movingAnimation = DOTween.Sequence();
        Transform transformMovingField = Hand.transform;
        Vector3 movingPosition = CalculateMovingPosition(transformMovingField);
        float offset = 0;
        foreach (var cardView in cardViews)
        {
            //Попробовать сделать через корутины

            cardView.SetActiveCardShirt(false);
            MonoBehaviour objectCardView = cardView as MonoBehaviour;
            //CardView objectCardView = cardView as CardView;
            objectCardView.gameObject.SetActive(true);

            //objectCardView.MoveToFieldAnimate(transformMovingField, movingPosition);

            //movingAnimation.Append(objectCardView.transform.DOMove(movingPosition, .7f)).OnComplete(() =>
            //{
            //    objectCardView.transform.SetParent(transformMovingField);
            //});

            objectCardView.transform.DOMove(movingPosition, .7f).OnComplete(() =>
            {
                objectCardView.transform.SetParent(transformMovingField);
            });

            offset += 1.2f;
            movingPosition.x += offset;
        }
        //movingAnimation.Play();
    }

    public void CompleteMoveClick() 
    {
        EndMakeMoveCallback?.Invoke();
    }
}
