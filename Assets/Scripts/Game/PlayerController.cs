using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour {

    private bool isMoveLeft = false;
    private bool isJumping = false;  //是否在跳跃
    private Vector3 nextPlatformPositionLeft;  //决定向左向右跳的位置
    private Vector3 nextPlatformPositionRight;
    private ManagerVars vars; //获取资源池
 
        private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
    }
    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
            return;

        if (Input.GetMouseButtonDown(0) && isJumping == false)
        {
            isJumping = true;
            EventCenter.Broadcast(EventType.DecidePath);
            Vector3 mousePos = Input.mousePosition;
            //点击左边
            if(mousePos.x <  Screen.width / 2)
            {
                isMoveLeft = true;
            }//点击右边
            else if(mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }

            Jump();
        }
    }

    //向左边跳需要转向
    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformPositionLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformPositionLeft.y + 0.8f, 0.15f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOMoveX(nextPlatformPositionRight.x, 0.2f);
            transform.DOMoveY(nextPlatformPositionRight.y + 0.8f, 0.15f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            isJumping = false; //跳跃结束
            //当前平台位置
            Vector3 currentPlatformPosition = collision.gameObject.transform.position;
            nextPlatformPositionLeft = new Vector3(currentPlatformPosition.x - vars.nextXPos, currentPlatformPosition.y + vars.nextYPos, vars.ZPos);
            nextPlatformPositionRight = new Vector3(currentPlatformPosition.x + vars.nextXPos, currentPlatformPosition.y + vars.nextYPos, vars.ZPos);
        }
    }
}
