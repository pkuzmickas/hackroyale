using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollider : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        if (collision.tag == "Player")
        {
            Debug.Log(collision.tag + "hit");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            //CmdDestroyBullet(collision.gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
        }
    }
}
