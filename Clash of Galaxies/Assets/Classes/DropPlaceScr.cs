using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerView playerView;

    // Start is called before the first frame update
    void Start()
    {
        playerView = gameObject.GetComponentInParent<PlayerView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���������� ���� � �������� �������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        if (cardView != null)
            cardView.DefaultTempCardParent = transform;
    }

    //��������� ���� �� ������ �������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        if (cardView != null && cardView.DefaultTempCardParent == transform)
            cardView.DefaultTempCardParent = cardView.DefaultParent;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardView cardView = eventData.pointerDrag.GetComponent<CardView>();

        playerView.DropCardCallback.Invoke(cardView); // ����� ������� ���� ��������� �����, �� ������� �������� ����������
    }
}
