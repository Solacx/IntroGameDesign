using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //移动相关
    private float moveX;
    private float moveY;
    public float moveSpeed=5;
    public float jumpSpeed = 5;

    public bool isJump = false;
    private bool isClimb = false;
    private bool isCanLa = false;
    private Transform box;

    private bool isHaveCanLa = false;

    public Text text1;
    void Start()
    {
        
    }
    public float x = 0.45f;
    public void RayTest()
    {
        RaycastHit2D[] rs = Physics2D.RaycastAll(transform.position, -transform.up, x);
        Debug.DrawRay(transform.position, -transform.up, Color.red, x);

        for (int i = 0; i < rs.Length; i++)
        {
            if(rs[i].transform.name == "road")
            {
                isJump = false;
                isTest = false;
                break;
            }
        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
       

        if (other.gameObject.name == "Box" && !isCanLa&&isHaveCanLa)
        {
            isCanLa = true;
            box = other.transform;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Box" && isCanLa&&!isClimb)
        {

            isCanLa = false;
            box = null;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Hand")
        {
            //GetComponent<SpriteRenderer>().color = Color.write;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("3");
            Destroy(other.gameObject);
            isHaveCanLa = true;
        }
        if (other.gameObject.name== "ladder")
        {
            isClimb = true;
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name== "Set")
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = Vector3.zero - Vector3.forward;
            Camera.main.orthographicSize = 10f;
            text1.enabled = false;
            //Camera.main.gameObject.GetComponent<Animation>().Play();
            //isCameraPlay = true;
        }
        if (other.gameObject.name == "ladder")
        {
            isClimb = false;
            GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }


    public float jumpCdTime = 1f;
    private float timer = 0;

    private bool isTest = false;
    private bool isCameraPlay = false;

    void FixedUpdate()
    {
        if(isCameraPlay &&! Camera.main.gameObject.GetComponent<Animation>().isPlaying)
        {

            isCameraPlay = false;
        }



        if(isTest)
            RayTest();
        if (box != null)
        {
            if (Vector2.Distance(transform.position, box.position) > 1.1f)
            {
                box.SetParent(null);
            }
        }

        if (isJump)
        {
            timer += Time.deltaTime;
            if (timer > jumpCdTime)
            {
                timer = 0;
                isTest = true;
            }
        }
    }

    void Update()
    {

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        if (moveX!=0)
        {
            transform.Translate(Vector2.right * Time.deltaTime * moveX * moveSpeed);
        }
        if(Input.GetButtonDown("Jump")&&!isJump&&!isClimb)
        {
          if(!isJump)
            {
                isJump = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed * 100);
            }
        }

        if(isClimb)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            transform.Translate(Vector2.up * Time.deltaTime * moveY * moveSpeed);
        }

        if(isHaveCanLa)
        {
            if (isCanLa && box != null)
            {
                Debug.Log(Vector2.Distance(transform.position, box.position));
                if (Input.GetKey(KeyCode.F))
                {
                    if (Vector2.Distance(transform.position, box.position) > 1.6f)
                    {
                        box.SetParent(null);
                    }
                    else
                    {
                        box.SetParent(transform);
                    }
                }
                else
                {
                    box.SetParent(null);
                }
            }
        }
      
    }
}
