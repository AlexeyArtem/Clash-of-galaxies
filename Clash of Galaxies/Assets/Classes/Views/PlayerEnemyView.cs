using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using System;

public class PlayerEnemyView : MonoBehaviour, IPlayerView
{
    public Action<ICardView> DropCardCallback { get; set; }
    public Action<ICardView> PlayCurrentCardCallback { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardViews(ICollection<ICardView> cardViews)
    {
        foreach (var view in cardViews)
        {
            CardView cardView = view as CardView;
            Transform transform = gameObject.transform.Find("EnemyHand");
            cardView.gameObject.transform.SetParent(transform, false);
            cardView.gameObject.SetActive(true);
            cardView.SetActiveCardShirt(true);
        }
    }
}
