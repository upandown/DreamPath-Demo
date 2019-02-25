using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public Vector3 startSpawnPos;
    

    private ManagerVars vars; //获取资源池
    private Vector3 platformSpawnPosition;//获取生成位置
    private bool isLeftSpawn = false; //控制生成方向

    private int spawnPlatformCount;

    private void Awake()
    {
        EventCenter.AddListener(EventType.DecidePath, DecidePath);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListenter(EventType.DecidePath, DecidePath);
    }


    private void Start()
    {
        platformSpawnPosition = startSpawnPos; //初始化位置
        vars = ManagerVars.GetManagerVars();
        for (int i = 0; i < 5; i++) {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go = Instantiate(vars.Charactor1);
    }
    //决定路径
    private void DecidePath() {
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
    //生成平台
    private void SpawnPlatform() {

        SpawnGrassPlatform();

        if (isLeftSpawn)//向左
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, startSpawnPos.z);
        }
        else  //向右
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, startSpawnPos.z);
        }
    }

    private void SpawnGrassPlatform()
    {
        GameObject go = Instantiate(vars.grassPlatform, transform);
        go.transform.position = platformSpawnPosition;

    }
}
