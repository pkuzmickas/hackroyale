using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip deathSound;
    static AudioSource audiosrc;

	// Use this for initialization
	void Start () {
        deathSound = Resources.Load<AudioClip>("neck");
        audiosrc = GetComponent<AudioSource>();
	}
	
    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "death":
                audiosrc.PlayOneShot(deathSound);
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
