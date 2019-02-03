using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class BorderControl : MonoBehaviour
{
    CircleCollider2D myCollider;

    [Range(0, 150)]
    public int segments = 150;
    [Range(0, 500)]
    public float xradius = 10;
    [Range(0, 500)]
    public float yradius = 10;
    LineRenderer line;
    bool endGame = false;

    void Start()
    {
        //Assigns the attached CircleCollider to myCollider
        myCollider = GetComponent<CircleCollider2D>();

        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, z, -3));

            angle += (360f / segments);
        }
    }

    void Update()
    {
        if (endGame == true)
            return;

        float rad = 0;
        if (myCollider.radius > 0.05)
        {
            myCollider.radius -= 0.001f;
            xradius -= 0.001f;
            yradius -= 0.001f;
            rad = myCollider.radius;
            CreatePoints();
        }

        if (rad <= 0.05)
        {
            myCollider.radius = 0;
            xradius = 0;
            yradius = 0;
            CreatePoints();
            endGame = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("TEST EXIT");
        GameObject hit = other.gameObject;
        PlayerHealth health = hit.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(20);
            health.SetOutsideZone(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TEST ENTER");
        GameObject hit = other.gameObject;
        PlayerHealth health = hit.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetOutsideZone(false);
        }
    }
}//END CLASS FALLING_GAME_BLOCK