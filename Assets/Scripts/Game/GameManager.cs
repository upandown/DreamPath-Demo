using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private GameData gameData;
    private ManagerVars vars;

    public bool IsGameStarted { get; set; }

    public bool IsGameOver { get; set; }

    public bool IsPause { get; set; }

    public bool playerIsMove { get; set; }
    public float fallTime { get; set; }


    private bool isFirstGame;

    private bool isMusicOn;

    private int[] bestScoreArr;

    private int selectSkin;

    private bool[] skinUnlocked;

    private int diamondCount;


    //游戏成绩
    private int gameScore;
    //钻石数量
    private int gameDiamond;
    private void Awake()
    {
       IsGameStarted = false;
        IsPause = false;
        Instance = this;
        fallTime = 6f;
        EventCenter.AddListener(EventType.AddScore, AddGameScore);
        EventCenter.AddListener(EventType.PlayerMove, PlayerIsMove);
        EventCenter.AddListener(EventType.AddDiamond, AddDiamond);
        //再来一局时直接进入开始游戏环节
        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
        vars = ManagerVars.GetManagerVars();
        //gameData = new GameData();
        InitGameData();
    }
    //玩家移动设置标值位
    private void PlayerIsMove()
    {
        playerIsMove = true;

    }
    private void AddGameScore()
    {
        if (IsGameStarted == false || IsGameOver || IsPause)
            return;
        gameScore++;
        EventCenter.Broadcast(EventType.UpdateScoreText, gameScore);
    }

    public int GetGameScore()
    {
        return gameScore;
    }

    public int GetGameDiamond()
    {
        return gameDiamond;
    }

    private void AddDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventType.updateDiamondText, gameDiamond);
    }

    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }

    public void setSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    public int GetAllDiamondCount()
    {
        return diamondCount;
    }

    public void UpdateAllDiamondCount(int value)
    {
        diamondCount += value;
        Save();
    }
    //设置皮肤
    public void SetSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }

    public int GetSelectSkin()
    {
        return selectSkin;
    }
    //初始化游戏数据
    private void InitGameData()
    {
        Read();
        if(gameData != null)
        {
            //Debug.Log("notfirstGame");
            isFirstGame = gameData.GetIsFirstGame();
        }
        else
        {
            //Debug.Log("firstGame");
            isFirstGame = true;
        }
        //第一次玩游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            gameData = new GameData();
            Save();
        }
        else
        {
            isMusicOn = gameData.GetIsMusicOn();
            bestScoreArr = gameData.GetBestScoreArr();
            skinUnlocked = gameData.GetSkinUnlocked();
            selectSkin = gameData.GetSelectSkin();
            diamondCount = gameData.GetDiamondCount();

        }
    }


    //保存游戏数据
    private void Save()
    {
        //对游戏数据进行序列化
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            //使用using自动释放流  不然就要用close关闭流
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                gameData.SetDiamondCount(diamondCount);
                gameData.SetBestScoreArr(bestScoreArr);
                gameData.SetIsFirstGame(isFirstGame);
                gameData.SetSelectSkin(selectSkin);
                gameData.SetIsMusicOn(isMusicOn);
                gameData.SetSkinUnlocked(skinUnlocked);
                
                //序列化
                bf.Serialize(fs,gameData);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    //读取游戏数据
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                //反序列化
                gameData = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
     }
      
    
}
