﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//创建管理整个游戏资源的资源容器
//在asset菜单中显示创建这个文件的按钮
//[CreateAssetMenu(menuName ="CreateManagerContainer")]
public class ManagerVars : ScriptableObject {

    public static ManagerVars GetManagerVars() {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");//加载容器 一定要把ass文件放在Resources文件夹下面
    }

	public List<Sprite> bgThemeList = new List<Sprite>();

    public GameObject grassPlatform;
    public GameObject Charactor1;

    public float nextXPos = 0.554f;

    public float nextYPos = 0.645f;

    public float ZPos = -1197.16f;
}
