using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShopPanel : MonoBehaviour {
    private ManagerVars vars;
    private Transform parent;
    private Text txt_Name;
    private Button btn_back;
    private Button btn_Select;
    private Button btn_Buy;
    private Text txt_diamond;
    private int currentIndex;


    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowShopPanel, Show);
        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("txt_name").GetComponent<Text>();
        btn_back = transform.Find("btn_back").GetComponent<Button>();
        btn_back.onClick.AddListener(OnBackButtonClick);

        btn_Buy = transform.Find("btn_buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);
        btn_Select = transform.Find("btn_select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        txt_diamond = transform.Find("Diamond/Text").GetComponent<Text>();
        vars = ManagerVars.GetManagerVars();
      
       
    }
    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.ShowShopPanel, Show);
    }

    private void OnBackButtonClick()
    {
        //点击返回后隐藏掉商店界面
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventType.ShowMainPanel);
        
    }

    private void OnBuyButtonClick()
    {
        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if(price > GameManager.Instance.GetAllDiamondCount())
        {
            Debug.Log("unBuy");
            return;
        }
        else
        {  //购买之后图标变为i=白色
            GameManager.Instance.UpdateAllDiamondCount(-price);
            GameManager.Instance.setSkinUnlocked(currentIndex);//解锁
            parent.GetChild(currentIndex).GetChild(0).GetComponent<Image>().color = Color.white;

        }
    }

    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventType.ChangeSkin, currentIndex);
        GameManager.Instance.SetSelectSkin(currentIndex);
        EventCenter.Broadcast(EventType.ShowMainPanel);
        gameObject.SetActive(false);
    }


    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 180, 404.9f);
        for(int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go =Instantiate(vars.skinChooseItem, parent);
            //未解锁


            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            //go.GetComponentInChildren<Image>().color = Color.red;
            //Debug.Log( GameManager.Instance.GetSkinUnlocked(i));
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.transform.localPosition = new Vector3((i + 1) * 180, -20, 0);
        }
        //打开界面拉选到选择的皮肤上
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetSelectSkin() * -180, 0, 0);
    }

    private void Update()
    {
        //滑到了哪
      currentIndex = (int)Mathf.Round(parent.transform.localPosition.x / -180f);
        if (Input.GetMouseButtonUp(0))//设置滑动条的跳转
        {
            parent.transform.DOLocalMoveX(currentIndex * -180f, 0.2f);
            //parent.transform.localPosition = new Vector2(currentIndex * -180f, 0);
        }
        SetItemSize(currentIndex);
        RefreshUI(currentIndex);
    }

    private void SetItemSize(int index)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            if(index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 180f);
                
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);

            }
        }
    }

    private void RefreshUI(int index)
    {
        txt_Name.text = vars.skinNameList[index];
        txt_diamond.text = GameManager.Instance.GetAllDiamondCount().ToString();
        //未解锁
        if (GameManager.Instance.GetSkinUnlocked(index) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[index].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }
}
