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
        objectCardView.transform.SetParent(selfField.transform);
        objectCardView.DefaultParent = selfField.transform;
        objectCardView.SetActiveCardShirt(false);
    }

    public void SetCardViewPlayerB(ICardView cardView)
    {
        CardView objectCardView = cardView as CardView;
        objectCardView.transform.SetParent(enemyField.transform);
        objectCardView.DefaultParent = enemyField.transform;
        objectCardView.SetActiveCardShirt(false);
    }
}
