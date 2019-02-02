using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleController : NetworkBehaviour
{
    public string HorizontalAxis = "Horizontal";
   
    Rigidbody2D rb;
    public float velocity=5f;
    public float ProjectileVelocity = 7f;
    GameObject camera;
    public float smoothTime = 0.3f;
    private Vector3 velocity3 = Vector3.zero;


    [ReadOnly]

    public Rigidbody2D bullet;
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        if (!isLocalPlayer)
        {
            return;
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
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI);

            }
        }
        
        if (Input.touchCount > 0)
        {

            Touch touch;
            if (Input.touchCount == 1)
                touch = Input.GetTouch(0);
            else
                touch = Input.GetTouch(1);

            if (touch.position.x > Screen.width / 2 || (touch.position.x < Screen.width / 2 - 100 && touch.position.y > Screen.height/2))
            {
                Vector3 diff = Camera.main.ScreenToWorldPoint(touch.position) - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

                Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, transform.localEulerAngles.z))) as Rigidbody2D;
                bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.right * ProjectileVelocity);
                Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

        

    }
}
