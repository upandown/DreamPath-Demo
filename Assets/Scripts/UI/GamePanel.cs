using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button btn_pause;
    private Button btn_play;
    private Text txt_score;
    private Text txt_diamondText;

    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowGamePanel, Show);//监听游戏开始的事件码
        EventCenter.AddListener<int>(EventType.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventType.updateDiamondText, updateDiamondText);
        Init();
    }
    //更新成绩显示
    private void UpdateScoreText(int score)
    {
        txt_score.text = score.ToString();
    }

    private void Init() {


        btn_pause = transform.Find("btn_pause").GetComponent<Button>();
        btn_pause.onClick.AddListener(OnPauseButtonClick);
        btn_play = transform.Find("btn_play").GetComponent<Button>();
        btn_play.onClick.AddListener(OnPlayButtonClick);
        txt_score = transform.Find("txt_score").GetComponent<Text>();
        txt_diamondText = transform.Find("diamond/txt_diamondScore").GetComponent<Text>();
        gameObject.SetActive(false);
        btn_play.gameObject.SetActive(false);//继续按钮只有在暂停点击后才出现
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.ShowGamePanel, Show);
        EventCenter.RemoveListenter<int>(EventType.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListenter<int>(EventType.updateDiamondText, updateDiamondText);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void OnPlayButtonClick(){
        btn_play.gameObject.SetActive(false);
        btn_pause.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.IsPause = false;
    }
    private void OnPauseButtonClick() {
        btn_play.gameObject.SetActive(true);
        btn_pause.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameManager.Instance.IsPause =true;
    }

    private void updateDiamondText(int diamond)
    {
        txt_diamondText.text = diamond.ToString();
    }
}
