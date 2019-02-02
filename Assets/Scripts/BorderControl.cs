using UnityEngine;
using System.Collections;

public class BorderControl : MonoBehaviour
{
    CircleCollider2D myCollider;

    void Start()
    {
        //Assigns the attached CircleCollider to myCollider
        myCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (myCollider.radius > 0)
            myCollider.radius -= 0.001f;
        else if (myCollider.radius != 0)
            myCollider.radius = 0;
    }
}//END CLASS FALLING_GAME_BLOCK