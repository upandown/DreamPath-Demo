using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool Instance;
    public int initSpawnCount = 5;
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> SpawnWinterGroupPlatformList = new List<GameObject>();
    private List<GameObject> SpawnGrassGroupPlatformList = new List<GameObject>();
    private List<GameObject> SpawnCommonGroupPlatformList = new List<GameObject>();
    private List<GameObject> SpikeLeftGroupPlatformList = new List<GameObject>();
    private List<GameObject> SpikeRighttGroupPlatformList = new List<GameObject>();
    private List<GameObject> DeathEffectList = new List<GameObject>();
    public List<GameObject> DiamondList = new List<GameObject>();

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
        
    }

    private void Init()
    {
        for(int i = 0;  i < initSpawnCount; i++)
        {
            InstantiateObject(vars.grassPlatform, ref grassPlatformList);

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for(int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j], ref SpawnCommonGroupPlatformList);
            }
            
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref SpawnGrassGroupPlatformList);
            }

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref SpawnWinterGroupPlatformList);
            }

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            
                InstantiateObject(vars.SpikePlatformGroup[0], ref SpikeRighttGroupPlatformList);
          
        }
        for (int i = 0; i < initSpawnCount; i++)
        {

            InstantiateObject(vars.SpikePlatformGroup[1], ref SpikeLeftGroupPlatformList);

        }
        for (int i = 0; i < initSpawnCount; i++)
        {

            InstantiateObject(vars.SpikePlatformGroup[1], ref SpikeLeftGroupPlatformList);

        }
        for (int i = 0; i < initSpawnCount; i++)
        {

            InstantiateObject(vars.deathEffect, ref DeathEffectList);

        }
        for (int i = 0; i < initSpawnCount; i++)
        {

            InstantiateObject(vars.diamond, ref DiamondList);

        }
        //for (int i = 0; i < initSpawnCount; i++)
        //{
        //    for (int j = 0; j < vars.SpikePlatformGroup.Count; j++)
        //    {
        //        InstantiateObject(vars.SpikePlatformGroup[j], ref SpikeGroupPlatformList);
        //    }

        //}
    }

    private GameObject InstantiateObject(GameObject prefab, ref List<GameObject> addList)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }

    //获得普通平台
    public GameObject GetGrassPlatform()
    {
        for(int i = 0; i < grassPlatformList.Count; i++)
        {
            if(grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        return InstantiateObject(vars.grassPlatform, ref grassPlatformList);
    }
    //通用组合平台
    public GameObject GetCommonPlatformGroup()
    {
        for (int i = 0; i < SpawnCommonGroupPlatformList.Count; i++)
        {
            if (SpawnCommonGroupPlatformList[i].activeInHierarchy == false)
            {
                return SpawnCommonGroupPlatformList[i];
            }
        }
        int rand = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObject(vars.commonPlatformGroup[rand], ref SpawnCommonGroupPlatformList);
    }

    //通用组合平台
    public GameObject GetSpawnWinterGroupPlatform()
    {
        for (int i = 0; i < SpawnWinterGroupPlatformList.Count; i++)
        {
            if (SpawnWinterGroupPlatformList[i].activeInHierarchy == false)
            {
                return SpawnWinterGroupPlatformList[i];
            }
        }
        int rand = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObject(vars.winterPlatformGroup[rand], ref SpawnWinterGroupPlatformList);
    }

    public GameObject GetSpawnGrassGroupPlatform()
    {
        for (int i = 0; i < SpawnGrassGroupPlatformList.Count; i++)
        {
            if (SpawnGrassGroupPlatformList[i].activeInHierarchy == false)
            {
                return SpawnGrassGroupPlatformList[i];
            }
        }
        int rand = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObject(vars.grassPlatformGroup[rand], ref SpawnGrassGroupPlatformList);
    }
    public GameObject GetSpikeLeftGroupPlatform()
    {
        for (int i = 0; i < SpikeLeftGroupPlatformList.Count; i++)
        {
            if (SpikeLeftGroupPlatformList[i].activeInHierarchy == false)
            {
                return SpikeLeftGroupPlatformList[i];
            }
        }
   
        return InstantiateObject(vars.SpikePlatformGroup[1], ref SpikeLeftGroupPlatformList);
    }
    public GameObject GetSpikeRightGroupPlatform()
    {
        for (int i = 0; i < SpikeRighttGroupPlatformList.Count; i++)
        {
            if (SpikeRighttGroupPlatformList[i].activeInHierarchy == false)
            {
                return SpikeRighttGroupPlatformList[i];
            }
        }

        return InstantiateObject(vars.SpikePlatformGroup[0], ref SpikeRighttGroupPlatformList);
    }

    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < DeathEffectList.Count; i++)
        {
            if (DeathEffectList[i].activeInHierarchy == false)
            {
                return DeathEffectList[i];
            }
        }
      
        return InstantiateObject(vars.deathEffect, ref DeathEffectList);
    }

    public GameObject GetDiamond()
    {
        for (int i = 0; i < DiamondList.Count; i++)
        {
            if (DiamondList[i].activeInHierarchy == false)
            {
                return DiamondList[i];
            }
        }

        return InstantiateObject(vars.diamond, ref DiamondList);
    }
}
