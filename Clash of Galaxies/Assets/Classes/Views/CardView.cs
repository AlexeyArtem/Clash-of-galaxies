using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Views;

public class CardView : MonoBehaviour, ICardView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Vector2 offset;
    private static GameObject tempCard; // ¬ременный шаблон карты, который отображает позицию дл€ вставки карты
    public Image Logo;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;
    public GameObject Shirt;
    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }

    void Awake() 
    {
        // ѕолучение объекта камеры сцены
        mainCamera = Camera.allCameras[0];
        if (tempCard == null) 
        {
            var instance = Resources.Load<GameObject>("Prefabs/TempCardPref");
            tempCard = Instantiate(instance);
            tempCard.SetActive(true);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ѕроверка позиции текущей карты относительно других и перемещение карты в зависимости от позиции карт, наход€щихс€ р€дом
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
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return; // Ќельз€ перетаскивать карту, котора€ находитс€ у противника

        tempCard.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        // ѕеремещение элемента вверх по иерархии игровых объектов
        DefaultParent = DefaultTempCardParent = transform.parent;

        tempCard.transform.SetParent(DefaultParent, false); //¬ качестве родител€ дл€ временной карты выступает родитель текущей карта, т.е. Hand
        tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent.parent); //”становка дл€ карты родител€ в качестве BG (background)

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return;

        // ѕолучение текущих координат экрана и преобразование к глобальным коодинатам
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (tempCard.transform.parent != DefaultTempCardParent)
            tempCard.transform.SetParent(DefaultTempCardParent);

        RecalculatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return;

        if (DefaultParent != null)
            transform.SetParent(DefaultParent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // ”становка индекса временной карты текущей карте
        transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());

        // ”брать временную карту с игрового пол€, когда перетаскивание завершено
        tempCard.SetActive(false);
    }

    public void SetActiveCardShirt(bool isActive)
    {
        Shirt.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.name == "SelfField" || transform.parent.name == "EnemyField") 
        {
            PlayerView playerView = FindObjectOfType<PlayerView>();
            playerView?.PlayCurrentCardCallback?.Invoke(this);
        }
    }

    public void ChangeGamePoints(int gamePoints)
    {
        GamePoints.text = gamePoints.ToString();
    }

    public void DestroyView() 
    {
        Destroy(gameObject);
    }
}
