using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {
    private Button btn_start;
    private Button btn_shop;
    private Button btn_rank;
    private Button btn_sound;
    private ManagerVars vars;
    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowMainPanel, Show);
        EventCenter.AddListener<int>(EventType.ChangeSkin,ChangeSkin);
        Init();
        //GameManager.Instance.IsGameStarted = false;
        vars = ManagerVars.GetManagerVars();
       
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.ShowMainPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void ChangeSkin(int index)
    {
        //btn_shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[index];
    }

    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventType.ShowGamePanel);
            gameObject.SetActive(false);
        }

        ChangeSkin(GameManager.Instance.GetSelectSkin());

    }

    private void Init()
    {
        btn_start = transform.Find("btn_start").GetComponent<Button>();
        btn_start.onClick.AddListener(OnStartButtonClick);
        btn_shop = transform.Find("btns/btn_shop").GetComponent<Button>();
        btn_shop.onClick.AddListener(OnShopButtonClick);
        btn_rank = transform.Find("btns/btn_rank").GetComponent<Button>();
        btn_rank.onClick.AddListener(OnRankButtonClick);
        btn_sound = transform.Find("btns/btn_sound").GetComponent<Button>();
        btn_sound.onClick.AddListener(OnSoundButtonClick);
    }
    //点击开始游戏按钮
    private void OnStartButtonClick()
    {
        GameManager.Instance.IsGameStarted = true; //游戏开始
        EventCenter.Broadcast(EventType.ShowGamePanel); //广播游戏开始的事件码
        gameObject.SetActive(false);
    }
    private void OnShopButtonClick()
    {
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventType.ShowShopPanel); 
    }
    private void OnRankButtonClick() {
    }
    private void OnSoundButtonClick()
    {

    }
}
