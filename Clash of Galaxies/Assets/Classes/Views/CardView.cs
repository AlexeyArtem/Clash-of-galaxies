using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Views;

public class CardView : MonoBehaviour, ICardView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Camera mainCamera;
    private Vector2 offset;
    private static GameObject tempCardObj; // ¬ременный шаблон карты, который отображает позицию дл€ вставки карты
    private static Arrow greenArrowScr;
    public Image CardImage;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;
    public GameObject ShirtObj, FrameObj;
    public Animation AnimationCard;
    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }

    void Awake() 
    {
        // ѕолучение объекта камеры сцены
        mainCamera = Camera.allCameras[0];
        if (tempCardObj == null) 
        {
            var instance = Resources.Load<GameObject>("Prefabs/TempCardPref");
            tempCardObj = Instantiate(instance);
            tempCardObj.SetActive(true);
        }
        if (greenArrowScr == null) 
        {
            var instance = Resources.Load<GameObject>("Prefabs/ArrowPref");
            var obj = Instantiate(instance);
            obj.transform.SetParent(GameObject.Find("BG").transform, false);
            greenArrowScr = obj.GetComponent<Arrow>();
        }
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

                if (tempCardObj.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }
        tempCardObj.transform.SetSiblingIndex(newIndex);
    }

    public void SetCardInfo(string name, string description, int gamePoints, int influenceGamePoints)
    {
        Name.text = name;
        GamePoints.text = gamePoints.ToString();
        InfluenceGamePoints.text = influenceGamePoints.ToString();
    }

    public void SetImage(string nameImg) 
    {
        Image imgCard = GetComponent<Image>();
        string path = "Sprites/Cards/" + nameImg;
        Sprite spriteCard = Resources.Load<Sprite>(path);
        imgCard.sprite = spriteCard;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return; // Ќельз€ перетаскивать карту, котора€ находитс€ у противника

        tempCardObj.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        // ѕеремещение элемента вверх по иерархии игровых объектов
        DefaultParent = DefaultTempCardParent = transform.parent;

        tempCardObj.transform.SetParent(DefaultParent, false); //¬ качестве родител€ дл€ временной карты выступает родитель текущей карта, т.е. Hand
        tempCardObj.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent.parent); //”становка дл€ карты родител€ в качестве BG (background)

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return;

        // ѕолучение текущих координат экрана и преобразование к глобальным коодинатам
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (tempCardObj.transform.parent != DefaultTempCardParent)
            tempCardObj.transform.SetParent(DefaultTempCardParent);

        RecalculatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return;

        if (DefaultParent != null)
            transform.SetParent(DefaultParent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // ”становка индекса временной карты текущей карте
        transform.SetSiblingIndex(tempCardObj.transform.GetSiblingIndex());

        // ”брать временную карту с игрового пол€, когда перетаскивание завершено
        tempCardObj.SetActive(false);
    }

    public void SetActiveCardShirt(bool isActive)
    {
        ShirtObj.SetActive(isActive);
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
        AnimationCard.Play("destroyCard");
    }

    public void DestroyGameObject() 
    {
        Destroy(gameObject);
    }

    public void ActivateTargetArrow()
    {
        if (transform.parent.name == "SelfField")
        {
            greenArrowScr.SetupAndActivate(transform);
        }
    }

    public void DeactivateTargetArrow()
    {
        if (transform.parent.name == "SelfField")
        {
            greenArrowScr.Deactivate();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FrameObj.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FrameObj.SetActive(false);
    }
}
