using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public const int maxHP = 100;
    public int curHealth = maxHP;
    private bool outsideZone = false;
    private float timer = 0.0f;
    private bool bloodEnabled = false;
    public GameObject zjbs;

    public RectTransform hpBar;

	// Use this for initialization
	void Start () {
		
	}

    public void TakeDamage(int nr)
    {
        curHealth -= nr;
        if (curHealth <= 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("death");
            curHealth = 0;
            Debug.Log("DED");
            outsideZone = false;
            zjbs.SetActive(false);
        }

        bloodEnabled = true;
        GameObject.Find("blood").GetComponent<Image>().enabled = bloodEnabled;

        hpBar.sizeDelta = new Vector2(curHealth*2, hpBar.sizeDelta.y);
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
        if(outsideZone == true)
        {
            timer += Time.deltaTime;

            if (bloodEnabled == true && timer > 1.0f)
            {
                bloodEnabled = false;
                GameObject.Find("blood").GetComponent<Image>().enabled = bloodEnabled;
            }

            if (timer > 2.0f)
            {
                timer = 0.0f;
                TakeDamage(20);
            }
        }
        

	}
}
