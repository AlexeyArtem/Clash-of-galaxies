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
    private static GameObject tempCardObj; // ��������� ������ �����, ������� ���������� ������� ��� ������� �����
    private static Arrow arrowScr;
    public Image Logo;
    public TextMeshProUGUI Name, GamePoints, InfluenceGamePoints;
    public GameObject ShirtObj;
    public Transform DefaultTempCardParent { get; set; }
    public Transform DefaultParent { get; set; }

    void Awake() 
    {
        // ��������� ������� ������ �����
        mainCamera = Camera.allCameras[0];
        if (tempCardObj == null) 
        {
            var instance = Resources.Load<GameObject>("Prefabs/TempCardPref");
            tempCardObj = Instantiate(instance);
            tempCardObj.SetActive(true);
        }
        if (arrowScr == null) 
        {
            var instance = Resources.Load<GameObject>("Prefabs/ArrowPref");
            var obj = Instantiate(instance);
            obj.transform.SetParent(GameObject.Find("BG").transform, false);
            arrowScr = obj.GetComponent<Arrow>();
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

    // �������� ������� ������� ����� ������������ ������ � ����������� ����� � ����������� �� ������� ����, ����������� �����
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return; // ������ ������������� �����, ������� ��������� � ����������

        tempCardObj.SetActive(true);

        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);

        // ����������� �������� ����� �� �������� ������� ��������
        DefaultParent = DefaultTempCardParent = transform.parent;

        tempCardObj.transform.SetParent(DefaultParent, false); //� �������� �������� ��� ��������� ����� ��������� �������� ������� �����, �.�. Hand
        tempCardObj.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent.parent); //��������� ��� ����� �������� � �������� BG (background)

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "EnemyHand" || transform.parent.name == "EnemyField") return;

        // ��������� ������� ��������� ������ � �������������� � ���������� ����������
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

        // ��������� ������� ��������� ����� ������� �����
        transform.SetSiblingIndex(tempCardObj.transform.GetSiblingIndex());

        // ������ ��������� ����� � �������� ����, ����� �������������� ���������
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
        Destroy(gameObject);
    }

    public void ActivateTargetArrow()
    {
        if (transform.parent.name == "SelfField")
        {
            arrowScr.SetupAndActivate(transform);
        }
    }

    public void DeactivateTargetArrow()
    {
        if (transform.parent.name == "SelfField")
        {
            arrowScr.Deactivate();
        }
    }
}
