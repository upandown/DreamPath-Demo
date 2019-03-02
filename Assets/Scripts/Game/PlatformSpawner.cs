using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter
}

public class PlatformSpawner : MonoBehaviour {

    public Vector3 startSpawnPos;
    public int milestoneCount = 10;//管理平台掉落的阈值
    //public float fallTime;//掉落时间
    public float minTime;//最小掉落时间
    public float factor; //掉落时间的缩减系数

    public float fallTime = 2f;

    private ManagerVars vars; //获取资源池
    private Vector3 platformSpawnPosition;//获取生成位置
    private bool isLeftSpawn = false; //控制生成方向

    private int spawnPlatformCount;
    private Sprite selectPlatformSprite; //选择的主题
    private PlatformGroupType platformType; //组合的主题

    private bool spikeSpawnLeft = false; //钉子生成方向
    private Vector3 spikeSpawnDirPlatformPos;//生成钉子方向平台的位置
    private int spikePlatformCount; //钉子方向平台的数量
    private bool isSpawnSpike;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();

        EventCenter.AddListener(EventType.DecidePath, DecidePath);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.DecidePath, DecidePath);
    }


    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos; //初始化位置
       
        for (int i = 0; i < 5; i++) {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go = Instantiate(vars.Charactor1);
    }

    //平台掉落
    private void Update()
    {
        //fallTime = GameManager.Instance.fallTime;
        if (GameManager.Instance.IsGameStarted && GameManager.Instance.IsGameOver == false)
        {
            UpdateFallTime();
        }
                }
    //掉落时间与时间有关
    private void UpdateFallTime()
    {
        if (GameManager.Instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= factor;
            if(fallTime < minTime)
            {
                fallTime = minTime;
            }
        }

    }

    //决定路径
    private void DecidePath() {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }
        if (spawnPlatformCount > 0) {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }

    }
    //平台随机主题
    private void RandomPlatformTheme()
    {
        int rand = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[rand];

        if(rand == 2)
        {
            platformType = PlatformGroupType.Winter;
        }
        else
        {
            platformType = PlatformGroupType.Grass;
        }
    }


    //生成平台
    private void SpawnPlatform() {

        int randObstacleDir = Random.Range(0, 2);
        //生成普通平台
        if(spawnPlatformCount >= 1)
        {
            SpawnGrassPlatform(randObstacleDir);
        }
        else if(spawnPlatformCount == 0)
        {
            int rand = Random.Range(0, 3);
            if(rand == 0)  //生成通用组合平台
            {
                SpawnCommonGroupPlatform(randObstacleDir);
            }
            else if(rand == 1)//生成主题组合平台
            {
                switch (platformType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassGroupPlatform(randObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterGroupPlatform(randObstacleDir);
                        break;
                    default:
                        break;
                }

            }else if(rand == 2)//生成钉子障碍
            {
                int value = -1;
                if (isLeftSpawn)//向左
                { //生成向右的钉子平台
                    spikeSpawnLeft = false;
                    value = 0;
                }
                else  //向右
                {
                    spikeSpawnLeft = true;
                    value = 1;
                }
                SpawnSpikePlatform(value, randObstacleDir);
                isSpawnSpike = true;
                spikePlatformCount = 4;
                if (spikeSpawnLeft)//钉子在左边
                {
                    spikeSpawnDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f, platformSpawnPosition.y + vars.nextYPos, vars.ZPos);
                }
                else
                {
                    spikeSpawnDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f, platformSpawnPosition.y + vars.nextYPos, vars.ZPos);
                }
            }
        }

        int randSpawnDiamond = Random.Range(0, 10);
        if(randSpawnDiamond == 6 && GameManager.Instance.playerIsMove)
        {
            GameObject go = ObjectPool.Instance.GetDiamond();
            go.transform.position = new  Vector3(platformSpawnPosition.x , platformSpawnPosition.y + 0.9f, vars.ZPos);
            go.SetActive(true);
        }
        if (isLeftSpawn)//向左
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, startSpawnPos.z);
        }
        else  //向右
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, startSpawnPos.z);
        }
    }

    private void SpawnGrassPlatform(int randObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
        go.SetActive(true);
    }
    //生成通用平台
    private void SpawnCommonGroupPlatform(int randObstacleDir)
    {
    //    int rand = Random.Range(0, 4);
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
        go.SetActive(true);

    }
    //生成草地主题的组合平台
    private void SpawnGrassGroupPlatform(int randObstacleDir)
    {
        //int rand = Random.Range(0, 2);
        //GameObject go = Instantiate(vars.grassPlatformGroup[rand], transform);
        //go.transform.position = platformSpawnPosition;
        //go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir);
        GameObject go = ObjectPool.Instance.GetSpawnGrassGroupPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
        go.SetActive(true);
    }
    //生成冬季主题的组合平台
    private void SpawnWinterGroupPlatform(int randObstacleDir)
    {
        //int rand = Random.Range(0, 2);
        //GameObject go = Instantiate(vars.winterPlatformGroup[rand], transform);
        //go.transform.position = platformSpawnPosition;
        //go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir);
        GameObject go = ObjectPool.Instance.GetSpawnWinterGroupPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
        go.SetActive(true);
    }
    //生成钉子平台
    private void SpawnSpikePlatform(int value, int randObstacleDir)
    {   
            //GameObject go = Instantiate(vars.SpikePlatformGroup[value], transform);
            //go.transform.position = platformSpawnPosition;
            //go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir);
        if(value == 0)
        {
            GameObject go = ObjectPool.Instance.GetSpikeRightGroupPlatform();
            go.transform.position = platformSpawnPosition;
            go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
            go.SetActive(true);
        }
        else
        {
            GameObject go = ObjectPool.Instance.GetSpikeLeftGroupPlatform();
            go.transform.position = platformSpawnPosition;
            go.GetComponent<Platform>().Init(selectPlatformSprite, randObstacleDir, fallTime);
            go.SetActive(true);
        }
    }

    //生成钉子平台方向的平台
    private void AfterSpawnSpike()
    {
        if(spikePlatformCount > 0)
        {
            spikePlatformCount--;
            for(int i = 0; i < 2; i++)
            {

                GameObject temp = ObjectPool.Instance.GetGrassPlatform();


                if ( i == 0)//生成正确的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    if (spikeSpawnLeft)//钉子在左  平台在右
                    {
                   
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, vars.ZPos);
                    }
                    else
                    {
                    
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, vars.ZPos);
                    }

                }
                else
                {
                    temp.transform.position = spikeSpawnDirPlatformPos;
                    if (spikeSpawnLeft)//钉子在左  平台在右
                    {

                        spikeSpawnDirPlatformPos = new Vector3(spikeSpawnDirPlatformPos.x - vars.nextXPos, spikeSpawnDirPlatformPos.y + vars.nextYPos, vars.ZPos);
                    }
                    else
                    {

                        spikeSpawnDirPlatformPos = new Vector3(spikeSpawnDirPlatformPos.x + vars.nextXPos, spikeSpawnDirPlatformPos.y + vars.nextYPos, vars.ZPos);
                    }
                }
                temp.GetComponent<Platform>().Init(selectPlatformSprite, 1, fallTime);
                temp.SetActive(true);
            }
        }
        else //生成钉子方向平台完毕，生成前进方向的平台
        {
            isSpawnSpike = false;
            DecidePath();
        }

    }
}
