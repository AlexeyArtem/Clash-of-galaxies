using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Views;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Vector2 offset;
    //private Transform DefaultParent;
    //private Transform DefaultTempCardParent;

    private GameObject tempCard; // ��������� ������ �����, ������� ���������� ������� ��� ������� �����

    public Image Logo;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;

    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }

    void Awake() 
    {
        // ��������� ������� ������ �����
        mainCamera = Camera.allCameras[0];

        // 
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

    // �������� ������� ������� ����� ������������ ������ � ����������� ����� � ����������� �� ������� ����, ����������� �����
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

    public void SetCardInfo(string name, int gamePoints, int influenceGamePoints)
    {
        Name.text = name;
        GamePoints.text = gamePoints.ToString();
        InfluenceGamePoints.text = influenceGamePoints.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempCard.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        // ����������� �������� ����� �� �������� ������� ��������
        DefaultParent = DefaultTempCardParent = transform.parent;

        // �������� �������������� �����, ������ ���� ����� ��������� � ���� ���� ������ ��� � ���� ������� ����� � ���� ������ ��� ������
        //isDraggable = (DefaultParent.GetComponent<DropPlaceScr>().FieldType == FieldType.SelfHand ||
        //               DefaultParent.GetComponent<DropPlaceScr>().FieldType == FieldType.SelfField)
        //              && GameManagerScr.IsPlayerTurn;
        //if (!isDraggable) return;

        tempCard.transform.SetParent(DefaultParent, false); //� �������� �������� ��� ��������� ����� ��������� �������� ������� �����, �.�. Hand
        tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent); //��������� ��� ����� �������� � ��������, �� ���� BG (background)

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // ������������� ���� ���������� ��� �����, ����� �� ����� � � ����
        //if (GetComponent<CardInfoScr>().SelfCard.CanAttack)
            //GameManagerScr.HighlightTargets(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��������� ������� ��������� ������ � �������������� � ���������� ����������
        Vector2 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;

        if (tempCard.transform.parent != DefaultTempCardParent)
            tempCard.transform.SetParent(DefaultTempCardParent);

        RecalculatePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (!isDraggable) return;

        if (DefaultParent != null)
            transform.SetParent(DefaultParent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // ��������� ������� ��������� ����� ������� �����
        transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());

        // ������ ��������� ����� � �������� ����, ����� �������������� ���������
        tempCard.SetActive(false);
    }
}
