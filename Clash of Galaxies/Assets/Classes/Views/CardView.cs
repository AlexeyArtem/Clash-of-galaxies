using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Views;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;
using System;


public class CardView : MonoBehaviour, ICardView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Camera mainCamera;
    private Vector2 offset;
    private static GameObject tempCardObj; // Временный шаблон карты, который отображает позицию для вставки карты
    private static Arrow greenArrowScr;
    private Animator attackPointsAnimator, strengtheningPointsAnimator, cardAnimator;
    private bool isActivateBehaviour;
    private bool isDraggable;

    public Image CardImage;
    public TextMeshProUGUI Name, Description, GamePoints, InfluenceGamePoints, AttackPoints, StrengtheningPoints;
    public GameObject ShirtObj, FrameObj, TooltipObj;
    public Animation AnimationCard;

    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }
    public bool IsActivateBehaviour 
    {
        get 
        {
            return isActivateBehaviour;
        }
        private set 
        {
            isActivateBehaviour = value;
            if (isActivateBehaviour) ActivateTargetArrow();
            else DeactivateTargetArrow();
        }
    }

    void Awake() 
    {
        AttackPoints.text = "";
        StrengtheningPoints.text = "";
        attackPointsAnimator = AttackPoints.GetComponent<Animator>();
        strengtheningPointsAnimator = StrengtheningPoints.GetComponent<Animator>();
        cardAnimator = GetComponent<Animator>();

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

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (IsActivateBehaviour)
                DeactivateTargetArrow();
        }
    }

    private void ActivateTargetArrow() 
    {
        if (transform.parent.name == "SelfField")
        {
            greenArrowScr?.SetupAndActivate(transform);
        }
    }

    private void DeactivateTargetArrow() 
    {
        greenArrowScr?.Deactivate();
    }

    // Проверка позиции текущей карты относительно других и перемещение карты в зависимости от позиции карт, находящихся рядом
    private void RecalculatePositionTempCard()
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

    public void MoveToFieldAnimate(Transform parent, Vector2 position, float time = .8f) 
    {
        Sequence sequence = DOTween.Sequence();

        transform.DOMove(position, time)
        .OnComplete(() => {
            transform.SetParent(parent);
        });
    }

    public void MoveToFieldAnimate(Transform parent, Vector2 position, int siblingIndex, float time = .8f)
    {
        Sequence sequence = DOTween.Sequence();

        transform.DOMove(position, time)
        .OnComplete(() => {
            transform.SetParent(parent);
            transform.SetSiblingIndex(siblingIndex);
        });
    }

    public void SetCardInfo(string name, string description, int gamePoints, int influenceGamePoints)
    {
        Name.text = name;
        GamePoints.text = gamePoints.ToString();
        Description.text = description;
        InfluenceGamePoints.text = influenceGamePoints.ToString();
        if (influenceGamePoints < 0) 
        {
            InfluenceGamePoints.color = Color.red;
        }
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
        if (transform.parent.name != "SelfHand") return;

        isDraggable = true;
        TooltipObj.SetActive(false);
        tempCardObj.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        DefaultParent = DefaultTempCardParent = transform.parent;

        tempCardObj.transform.SetParent(DefaultParent, false);
        tempCardObj.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;


        // Получение текущих координат экрана и преобразование к глобальным коодинатам
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (tempCardObj.transform.parent != DefaultTempCardParent)
            tempCardObj.transform.SetParent(DefaultTempCardParent);

        RecalculatePositionTempCard();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        if (DefaultParent != null) 
        {
            int index = tempCardObj.transform.GetSiblingIndex();
            Vector2 position = DefaultParent.GetChild(DefaultParent.childCount - 1).position;
            if (index < DefaultParent.childCount)
                position = DefaultParent.GetChild(index).position;

            if (tempCardObj.transform.parent.name == "SelfHand")
                transform.SetParent(DefaultParent);
            MoveToFieldAnimate(DefaultParent, position, index, .4f);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(tempCardObj.transform.GetSiblingIndex());
        tempCardObj.SetActive(false);
        isDraggable = false;
    }

    public void SetActiveCardShirt(bool isActive)
    {
        ShirtObj.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(IsActivateBehaviour) ActivateTargetArrow();

        if (transform.parent.name == "SelfField" || transform.parent.name == "EnemyField") 
        {
            PlayerView playerView = FindObjectOfType<PlayerView>();
            playerView?.PlayCurrentCardCallback?.Invoke(this);
        }
    }

    public void ChangeGamePoints(int gamePoints)
    {
        int difference = gamePoints - Convert.ToInt32(GamePoints.text.ToString());
        GamePoints.text = gamePoints.ToString();
        if (difference < 0)
        {
            AttackPoints.text = difference.ToString();
            attackPointsAnimator.SetTrigger("OnFlyUp");
            cardAnimator.SetTrigger("OnRedBlink");
        }
        else if (difference > 0)
        {
            StrengtheningPoints.text = "+" + difference.ToString();
            strengtheningPointsAnimator.SetTrigger("OnFlyUp");
            cardAnimator.SetTrigger("OnBlueBlink");
        }
    }

    public void DestroyView() 
    {
        cardAnimator.SetTrigger("OnMissing");
    }

    public void DestroyGameObject() 
    {
        Destroy(gameObject);
    }

    public void ActivateBehaviour()
    {
        IsActivateBehaviour = true;
    }

    public void DeactivateBehaviour()
    {
        IsActivateBehaviour = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.name == "SelfHand") 
        {
            TooltipObj.SetActive(true);
        }
        FrameObj.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.parent.name == "SelfHand")
        {
            TooltipObj.SetActive(false);
        }
        FrameObj.SetActive(false);
    }
}
