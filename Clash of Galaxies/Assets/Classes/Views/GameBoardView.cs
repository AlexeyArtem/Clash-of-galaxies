using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Views;

public class GameBoardView : MonoBehaviour, IGameBoardView
{
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
        var objectCardView = cardView as CardView;
        objectCardView.DefaultParent = transform;
    }
}
