using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class BorderControl : NetworkBehaviour
{
    public CircleCollider2D myCollider;

    public int segments = 150;

    [SyncVar] public float xradius = 10;
    public float yradius = 10;
    LineRenderer line;
    bool endGame = false;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameObject.Find("gameover").GetComponent<Image>().enabled = false;
        GameObject.Find("victory").GetComponent<Image>().enabled = false;
        count.players = 0;
        yradius = 10;
        xradius = 10;
        //Assigns the attached CircleCollider to myCollider
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.radius = xradius;

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

        yradius = xradius;
        myCollider.radius = xradius;
        CreatePoints();

        if (!isServer)
            return;

        float rad = 0;
        if (xradius > 0.05)
        {
            xradius -= 0.002f;
            yradius = xradius;
            myCollider.radius = xradius;
            
            rad = xradius;
        }

        if (rad <= 0.05)
        {
            xradius = 0;
            yradius = xradius;
            myCollider.radius = xradius;
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