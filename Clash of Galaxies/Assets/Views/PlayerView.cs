using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Controllers;
using UnityEngine.EventSystems;
using System;

public class PlayerView : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<CardView> DropCard { get; set; } // ������ �� ����� �����������

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
            Transform transform = gameObject.transform.Find("SelfHand");
            view.gameObject.transform.SetParent(transform, false);
            view.gameObject.SetActive(true);
        }
    }

    //���������� ���� � �������� �������
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (eventData.pointerDrag == null)
        //    return;

        //CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        //if (cardView != null)
        //    cardView.DefaultTempCardParent = transform;
    }

    //��������� ���� �� ������ �������
    public void OnPointerExit(PointerEventData eventData)
    {
        //if (eventData.pointerDrag == null) return;

        //CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        //if (cardView != null && cardView.DefaultTempCardParent == transform)
        //    cardView.DefaultTempCardParent = cardView.DefaultParent;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (eventData.pointerDrag == null)
        //    return;

        //CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        //DropCard.Invoke(cardView); // ����� ������� ���� ��������� �����, �� ������� �������� ����������
    }
}
