using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollider : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        if (collision.tag == "Player")
        {
            Debug.Log(collision.tag + "hit");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(7);
            //CmdDestroyBullet(collision.gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
            if(collision.tag == "Player")
            {
                if(collision.gameObject.GetComponent<PlayerHealth>().curHealth <=0)
                {
                    collision.gameObject.GetComponent<Collider2D>().enabled = false;
                }
            }
            Destroy(gameObject, 1);
            
        }
    }
}
