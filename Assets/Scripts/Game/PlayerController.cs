using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    private bool isMoveLeft = false;
    public bool isJumping = false;  //是否在跳跃
    private Vector3 nextPlatformPositionLeft;  //决定向左向右跳的位置
    private Vector3 nextPlatformPositionRight;
    private SpriteRenderer spriteRenderer;
    private ManagerVars vars; //获取资源池
    public LayerMask platformLayer;
    public LayerMask obstacleLayer;
    private Rigidbody2D rigidbody; //角色的刚体
    private bool isMove = false; //玩家是否移动

    public Transform rayDown;
    public Transform rayLeft, rayRight;

    public float clickTime = 0.1f; //最小点击时间间隔
 
    private bool clickEnable = true;

        private void Awake()
    {
        EventCenter.AddListener<int>(EventType.ChangeSkin, ChangeSkin);

        vars = ManagerVars.GetManagerVars();
        rigidbody = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    //更换皮肤
    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetSelectSkin());
    }


    private void ChangeSkin(int index)
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = vars.charactorSkinSpriteList[index];
    }

    private void Update()
    {
        //绘制射线
        //Debug.DrawRay(rayDown.position, Vector2.down * 1f, Color.red);
        //Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        //Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);
        //检测是否是点击的是UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true || GameManager.Instance.IsPause == true)
            return;

        if (Input.GetMouseButtonDown(0) && isJumping == false && clickEnable)
        {
            if(isMove == false)
            {
                EventCenter.Broadcast(EventType.PlayerMove);//广播玩家开始移动
                isMove = true;

            }
            clickEnable = false;
            StartCoroutine(isClikeEnableI());
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
        //检测玩家落下
        //游戏结束
        if(rigidbody.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());

            //玩家掉了下去
        }

        if(isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false;
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }

        if (this.transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.IsGameOver = true;
            //gameObject.SetActive(false);
            //调用结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }
    }
    private GameObject lastHitGo = null;

    private bool IsRayPlatform()
    {
       RaycastHit2D hit =  Physics2D.Raycast(rayDown.position, Vector2.down, 0.8f, platformLayer);
        if(hit.collider != null)
        {
       
            if (hit.collider.tag == "Platform")
            {
                if(lastHitGo != hit.collider.gameObject)
                {
                    //第一次跳跃
                    if(lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventType.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
             
                return true;

            }
               
        }
       
        return false;
    }
    //检测障碍物
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
           
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }

        }
        return false;

    }

   
          

    //向左边跳需要转向
    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformPositionLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformPositionLeft.y + 0.6f, 0.15f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOMoveX(nextPlatformPositionRight.x, 0.2f);
            transform.DOMoveY(nextPlatformPositionRight.y + 0.6f, 0.15f);
        }
    }
    private GameObject lastHitPla = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if(collision.tag == "Platform")
        {
            if (isJumping != false&& collision.gameObject != lastHitPla)
            {
          
                isJumping = false; //跳跃结束
                lastHitPla = collision.gameObject;
                //collision.GetComponent<BoxCollider2D>().enabled = false;
                //Rigidbody2D go = collision.gameObject.GetComponent<Rigidbody2D>();
                //if(isMove)
                //StartCoroutine(fallPlatform(go));
            }
                
            //当前平台位置
            Vector3 currentPlatformPosition = collision.gameObject.transform.position;
            nextPlatformPositionLeft = new Vector3(currentPlatformPosition.x - vars.nextXPos, currentPlatformPosition.y + vars.nextYPos, vars.ZPos);
            nextPlatformPositionRight = new Vector3(currentPlatformPosition.x + vars.nextXPos, currentPlatformPosition.y + vars.nextYPos, vars.ZPos);
        }
    }
    //防止一下子跳多个台阶
    private IEnumerator isClikeEnableI()
    {
        yield return new WaitForSeconds(clickTime);
        clickEnable = true;
    }
    //游戏结束后延迟一段时间调用结束面板
    private IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);
        EventCenter.Broadcast(EventType.ShowGameOverPanel);

    }

    //private IEnumerator fallPlatform(Rigidbody2D go)
    //{
    //    yield return new WaitForSeconds(GameManager.Instance.fallTime);
    //    go.bodyType = RigidbodyType2D.Dynamic;

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //迟到钻石
        if(collision.collider.tag == "PickUp")
        {
            EventCenter.Broadcast(EventType.AddDiamond);
            collision.gameObject.SetActive(false);

        }
    }
}
 