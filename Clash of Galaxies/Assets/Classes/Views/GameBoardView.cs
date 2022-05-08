using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Views;

public class GameBoardView : MonoBehaviour, IGameBoardView
{
    public GameObject selfField, enemyField;

    public void SetCardViewPlayerA(ICardView cardView) 
    {
        CardView objectCardView = cardView as CardView;
        objectCardView.SetActiveCardShirt(false);
        objectCardView.transform.SetParent(selfField.transform);
        objectCardView.DefaultParent = selfField.transform;
    }

    public void SetCardViewPlayerB(ICardView cardView)
    {
        CardView objectCardView = cardView as CardView;
        objectCardView.SetActiveCardShirt(false);
        
        Vector3 newPosition = enemyField.transform.position;
        var childCount = enemyField.transform.childCount;
        if (childCount > 1)
        {
            Transform lastChild = enemyField.transform.GetChild(enemyField.transform.childCount - 1);
            Transform preLastChild = enemyField.transform.GetChild(enemyField.transform.childCount - 2);

            newPosition = lastChild.position;
            float offset = lastChild.position.x - preLastChild.position.x;
            newPosition.x += offset;
        }

        objectCardView.MoveToFieldAnimate(enemyField.transform, newPosition);
    }
}
