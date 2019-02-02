using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour {
    public string HorizontalAxis = "Horizontal";
   
    public GameObject Projectile;
    Rigidbody2D rb;
    public float velocity=5f;
    public float ProjectileVelocity=7f;
    [ReadOnly]
    public bool grounded;

    public Rigidbody2D bullet;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update() {

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, Input.GetAxis("Vertical") * velocity);
        //transform.rotation = transform.Rotate(0, 0, -(Input.GetAxis("Mouse X")) Time.deltaTime speed);

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI);
                //Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, transform.localEulerAngles.z))) as Rigidbody2D;
                //bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.right * ProjectileVelocity);

            }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //    diff.Normalize();

        //    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        //}
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

        //if (Input.touchCount > 0)
        //{
        //    Debug.Log("Left click");
        //    var touch = Input.GetTouch(0);
        //    if (touch.position.x < Screen.width / 2)
        //    {
        //        Debug.Log("Left click");
        //    }
        //    else if (touch.position.x > Screen.width / 2)
        //    {
        //        Vector3 touchPos = Input.GetTouch(0).position;
        //        touchPos.z = 0.0f;
        //        transform.LookAt(touchPos);
        //    }
        //}


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        grounded = true;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        grounded = true;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        grounded = false;
    }
}
