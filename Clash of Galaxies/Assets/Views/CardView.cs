using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Vector2 offset;

    public Image Logo;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;

    void Awake() 
    {
        //Получение объекта камеры сцены
        mainCamera = Camera.allCameras[0];
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCardInfo(string name, int gamePoints, int influenceGamePoints)
    {
        Name.text = name;
        GamePoints.text = gamePoints.ToString();
        InfluenceGamePoints.text = influenceGamePoints.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        ////Перемещение элемента вверх по иерархии игровых объектов
        //DefaultParent = DefaultTempCardParent = transform.parent;

        ////Начинать перетаскивание можно, только если карта находится в поле руки игрока или в поле выбораса карты и если сейчас ход игрока
        //isDraggable = (DefaultParent.GetComponent<DropPlaceScr>().FieldType == FieldType.SelfHand ||
        //               DefaultParent.GetComponent<DropPlaceScr>().FieldType == FieldType.SelfField)
        //              && GameManagerScr.IsPlayerTurn;
        //if (!isDraggable) return;

        //tempCard.transform.SetParent(DefaultParent); //В качестве родителя для временной карты выступает родитель текущей карта, т.е. Hand
        //tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        //transform.SetParent(DefaultParent.parent); //Установка для карты родителя её родителя, то есть BG (background)

        //GetComponent<CanvasGroup>().blocksRaycasts = false;

        ////Подсвечивание карт противника для атаки, когда мы берем её в руку
        //if (GetComponent<CardInfoScr>().SelfCard.CanAttack)
        //    GameManagerScr.HighlightTargets(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
