using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;
    public bool startTimer ; //计时器
    public float fallTime; //掉落时间
    private Rigidbody2D rigidbody;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startTimer = true;
    }

    public void Init(Sprite sprite, int obstacleDir, float fallTime)
    {
        rigidbody.bodyType = RigidbodyType2D.Static;
        for (int i = 0; i < spriteRenderers.Length; i++) {

            spriteRenderers[i].sprite = sprite;//切换平台主题
        };


        if (obstacleDir == 0) //朝右边
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.position.x, obstacle.transform.position.y, obstacle.transform.position.z);
            }
        }
        
        this.fallTime = fallTime;
    }

    private void Update()
    {
        if (fallTime >= 0) //解决时间卡在2.7  2.43的bug 原因不明

        {
            startTimer = true;
        }

        if (GameManager.Instance.IsGameStarted == false ||GameManager.Instance.playerIsMove == false) return;
    
        if (startTimer)
        {
            fallTime -= Time.deltaTime;
            if(fallTime < 0)
            {
                startTimer = false;
                if(rigidbody.bodyType != RigidbodyType2D.Dynamic)
                {
                    rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    StartCoroutine(DealyHide());
                }
            }
        }
        //if (transform.position.y - Camera.main.transform.position.y < -6)
        //    StartCoroutine(DealyHide());
    }
    //一段时间后隐藏掉落的平台
    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    
    }

}
