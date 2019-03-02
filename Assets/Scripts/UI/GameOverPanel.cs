using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverPanel : MonoBehaviour {

    public Text txt_Score, txt_BestScore, AddDiamondText;

    public Button txt_Restart, btn_rank, btn_home;

    private void Awake()
    {
        txt_Restart.onClick.AddListener(OnResterButtonClick);
        btn_home.onClick.AddListener(OnHomeButtonClick);
        btn_rank.onClick.AddListener(OnRankButtonClick);
        EventCenter.AddListener(EventType.ShowGameOverPanel, Show);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.ShowGameOverPanel, Show);
    }

    private void Show()
    {
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        AddDiamondText.text = "+" + GameManager.Instance.GetGameDiamond().ToString();
        gameObject.SetActive(true);
    }
    //再来一局
    private void OnResterButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
    }

    private void OnRankButtonClick()
    {


    }

    private void OnHomeButtonClick()
    {
        //重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;

    }
}
