using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgTheme : MonoBehaviour {
    private SpriteRenderer m_spriteRenderer;
    private ManagerVars vars;

    public Sprite[] Sprites;
    private void Awake()
    {
        //获取资源池
        vars = ManagerVars.GetManagerVars();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        //生成随机数
        int ranValue = Random.Range(0, vars.bgThemeList.Count);
        //制定随机的背景
        m_spriteRenderer.sprite = vars.bgThemeList[ranValue];
    }

}
