using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

    public const int maxHP = 100;
    [SyncVar (hook = "OnChangeHealth")] public int curHealth = maxHP;
    private bool outsideZone = false;
    private float timer = 0.0f;
    [SyncVar] public bool bloodEnabled = false;
    public GameObject zjbs;

    public RectTransform hpBar;

	// Use this for initialization
	void Start () {
		
	}

    public void TakeDamage(int nr)
    {
        if(!isServer)
        {
            return;
        }

        curHealth -= nr;
    }

    public void SetOutsideZone(bool isOut)
    {
        outsideZone = isOut;
        timer = 0.0f;
        if(isOut == false)
        {
            bloodEnabled = false;
            GameObject.Find("blood").GetComponent<Image>().enabled = bloodEnabled;
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (bloodEnabled == true && timer > 1.0f)
        {
            bloodEnabled = false;
            GameObject.Find("blood").GetComponent<Image>().enabled = bloodEnabled;
        }
        if (outsideZone == true)
        {
            
            if (timer > 2.0f)
            {
                timer = 0.0f;
                TakeDamage(20);
            }
        }
        

	}

    void OnChangeHealth(int health)
    {
        curHealth = health;
        if (curHealth <= 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("death");
            curHealth = 0;
            Debug.Log("DED");
            outsideZone = false;
            zjbs.SetActive(false);
            SoundManagerScript.PlaySound("death");
            count.players--;
        }

        if (isLocalPlayer)
        {
            bloodEnabled = true;
            timer = 0.0f;
            GameObject.Find("blood").GetComponent<Image>().enabled = bloodEnabled;
            if (curHealth <= 0)
            {
                GameObject.Find("gameover").GetComponent<Image>().enabled = bloodEnabled;
                Destroy(gameObject, 1.5f);
                
            }
        }
        hpBar.sizeDelta = new Vector2(curHealth * 2, hpBar.sizeDelta.y);
    }
}
