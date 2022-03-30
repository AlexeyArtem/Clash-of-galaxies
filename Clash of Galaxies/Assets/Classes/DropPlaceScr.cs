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

    //Привидение мыши к границам объекта
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardView cardView = eventData.pointerDrag.GetComponent<CardView>();
        if (cardView != null)
            cardView.DefaultTempCardParent = transform;
    }

    //Отведение мыши от границ объекта
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

        playerView.DropCardCallback.Invoke(cardView); // Вызов события хода выбранной карты, на которое подписан контроллер
    }
}
