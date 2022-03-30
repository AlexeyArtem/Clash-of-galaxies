using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Views;

public class CardView : MonoBehaviour, ICardView, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Vector2 offset;
    private GameObject tempCard; // Временный шаблон карты, который отображает позицию для вставки карты
    public Image Logo;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;

    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }

    void Awake() 
    {
        // Получение объекта камеры сцены
        mainCamera = Camera.allCameras[0];
        tempCard = CardViewFactory.GetInstance().GetTempCard();
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Проверка позиции текущей карты относительно других и перемещение карты в зависимости от позиции карт, находящихся рядом
    private void RecalculatePosition()
    {
        int newIndex = DefaultTempCardParent.childCount;
        for (int i = 0; i < DefaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;

                if (tempCard.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }
        tempCard.transform.SetSiblingIndex(newIndex);
    }

    public void SetCardInfo(string name, string description, int gamePoints, int influenceGamePoints)
    {
        Name.text = name;
        GamePoints.text = gamePoints.ToString();
        InfluenceGamePoints.text = influenceGamePoints.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand") return; // Нельзя перетаскивать карту, которая находится у противника

        tempCard.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        // Перемещение элемента вверх по иерархии игровых объектов
        DefaultParent = DefaultTempCardParent = transform.parent;

        tempCard.transform.SetParent(DefaultParent, false); //В качестве родителя для временной карты выступает родитель текущей карта, т.е. Hand
        tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent); //Установка для карты родителя её родителя, то есть BG (background)

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand") return;

        // Получение текущих координат экрана и преобразование к глобальным коодинатам
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (tempCard.transform.parent != DefaultTempCardParent)
            tempCard.transform.SetParent(DefaultTempCardParent);

        RecalculatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand") return;

        if (DefaultParent != null)
            transform.SetParent(DefaultParent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Установка индекса временной карты текущей карте
        transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());

        // Убрать временную карту с игрового поля, когда перетаскивание завершено
        tempCard.SetActive(false);
    }
}
