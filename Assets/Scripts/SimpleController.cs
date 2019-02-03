using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SimpleController : NetworkBehaviour
{
    public string HorizontalAxis = "Horizontal";
    Rigidbody2D rb;
    public float velocity = 5f;
    public float ProjectileVelocity = 7f;
    GameObject camera;
    public float smoothTime = 0.3f;
    private Vector3 velocity3 = Vector3.zero;
   

    [ReadOnly]

    public Rigidbody2D bullet;
    // Use this for initialization
    void Start()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}
        GameObject.Find("Gas").GetComponent<BorderControl>().enabled = true;
        rb = GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera");
        GameObject.Find("Knob").GetComponent<Canvas>().enabled = true;
        GameObject.Find("KnobBackground").GetComponent<Canvas>().enabled = true;
        //var count = GameObject.Find("Players").GetComponent<count>();
        count.players++;
        Debug.Log("players:" + count.players);
        if (count.players > 1)
        {
            count.gameStarted = true;
            Debug.Log("GaME STARTED ");
        }
        else
        {
            count.gameStarted = false;
        }
    }


    //void OnGUI()
    //{

    //    GUI.Label(new Rect(200, 200, 200, 200), count.players.ToString());

    //}

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);

        var ph = GetComponent<PlayerHealth>();


        if (count.players <= 1 && count.gameStarted && ph.curHealth > 0)
        {
            Debug.Log("WE HAVE A WINNER");
            GameObject.Find("victory").GetComponent<Image>().enabled = true;
        }

        Vector3 goalPos = transform.position;
        //goalPos.y = transform.position.y;
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, goalPos, ref velocity3, smoothTime);
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10);

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, Input.GetAxis("Vertical") * velocity);
        //transform.rotation = transform.Rotate(0, 0, -(Input.GetAxis("Mouse X")) Time.deltaTime speed);

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                CmdMoveRotate(gameObject);
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI);
            }
        }

        if (Input.touchCount > 0)
        {

            Touch touch;
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
            }
            else
            {
                touch = Input.GetTouch(1);
                if (touch.position.x > Screen.width / 2 - 100 || (touch.position.x < Screen.width / 2 - 100 && touch.position.y > Screen.height / 2 - 30)) { 
                    //touch = Input.GetTouch(1);
                }
                else
                {
                    touch = Input.GetTouch(0);
                }
            }
            if (touch.position.x > Screen.width / 2 - 100 || (touch.position.x < Screen.width / 2 - 100 && touch.position.y > Screen.height / 2 - 30))
            {
                Vector3 diff = Camera.main.ScreenToWorldPoint(touch.position) - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

                GameObject bulletO = Instantiate(bullet.gameObject, transform.GetChild(1).gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, transform.localEulerAngles.z))) as GameObject;
                Rigidbody2D bulletInstance = bulletO.GetComponent<Rigidbody2D>();
                //bulletInstance.AddForce(transform.right * ProjectileVelocity);
                bulletInstance.velocity = transform.right * ProjectileVelocity;
                Physics2D.IgnoreCollision(bulletO.GetComponent<Collider2D>(), GetComponent<Collider2D>());

                Destroy(bulletO, 3);

                //CmdRotate(touch.position, gameObject);


                CmdFire(touch.position, gameObject);
            }
        }
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > Screen.width / 2 - 100 || (Input.mousePosition.x < Screen.width / 2 - 100 && Input.mousePosition.y > Screen.height / 2 - 30))
            {
                //CmdRotate(Input.mousePosition, gameObject);

                CmdFire(Input.mousePosition, gameObject);
            }
        }
#endif



    }

    [Command]
    void CmdRotate(Vector2 touchPos, GameObject player)
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(touchPos) - player.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

    }
    [Command]
    void CmdMoveRotate(GameObject player)
    {
        player.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI);

    }

    [Command]
    void CmdFire(Vector2 touchPos, GameObject player)
    {
        GameObject bulletO = Instantiate(bullet.gameObject, player.transform.GetChild(1).gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, player.transform.localEulerAngles.z))) as GameObject;
        Rigidbody2D bulletInstance = bulletO.GetComponent<Rigidbody2D>();
        //bulletInstance.AddForce(player.transform.right * ProjectileVelocity);
        bulletInstance.velocity = player.transform.right * ProjectileVelocity;

        Physics2D.IgnoreCollision(bulletO.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        NetworkServer.Spawn(bulletO);
        
        Destroy(bulletO, 3);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision.tag);
        //if (collision.tag == "bullet")
        //{
        //    GetComponent<PlayerHealth>().TakeDamage(1);
        //    CmdDestroyBullet(collision.gameObject);
        //    Destroy(collision.gameObject);
        //}
    }

    [Command]
    void CmdDestroyBullet(GameObject bullet)
    {
        //Destroy(collision);
        //NetworkServer.Destroy(bullet);
    }


    private void OnDestroy()
    {
        GameObject.Find("Knob").GetComponent<Canvas>().enabled = false;
        GameObject.Find("KnobBackground").GetComponent<Canvas>().enabled = false;
    }
}
