using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Views;
using UnityEngine.EventSystems;
using System;

public class PlayerView : MonoBehaviour, IPlayerView
{
    public Action<ICardView> DropCardCallback { get; set; } // —сылка на метод презентера, в котором вызываетс€ метод выполнени€ хода в модели
    public Action<ICardView> PlayCurrentCardCallback { get; set; } // —сылка на метод презентера, в котором вызываетс€ метод взаимодействи€ с выбранной картой в модели
    public Action EndMakeMoveCallback { get; set; } // —сылка на метод презентера, в котором вызываетс€ метод заврешени€ хода в модели

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
            Transform transform = gameObject.transform.Find("SelfHand");
            cardView.gameObject.transform.SetParent(transform, false);
            cardView.gameObject.SetActive(true);
        }
    }

    public void CompleteMoveClick() 
    {
        EndMakeMoveCallback?.Invoke();
    }
}
