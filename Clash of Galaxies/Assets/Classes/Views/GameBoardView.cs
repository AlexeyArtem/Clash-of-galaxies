using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Views;

public class GameBoardView : MonoBehaviour, IGameBoardView
{
    public GameObject selfField, enemyField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Удалить метод
    public void SetCardView(ICardView cardView)
    {
        CardView objectCardView = cardView as CardView;
        objectCardView.transform.SetParent(transform);
        objectCardView.DefaultParent = transform;
        objectCardView.SetActiveCardShirt(false);
    }

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
