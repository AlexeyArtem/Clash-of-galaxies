using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Controllers;

public class PlayerView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCardViews(ICollection<CardView> cardViews) 
    {
        foreach (var view in cardViews)
        {
            view.gameObject.transform.SetParent(gameObject.transform, false);
            view.gameObject.SetActive(true);
        }
    }
}
