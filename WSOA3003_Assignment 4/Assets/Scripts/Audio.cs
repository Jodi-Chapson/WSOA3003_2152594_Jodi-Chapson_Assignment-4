using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //this script manages the background music of the game

    public static Audio song;


    private void Awake()
    {
        this.GetComponent<AudioSource>().Play();

        if (song != null)
        {
            //if there is indeed a version of song1 in the scene
            Destroy(gameObject);

        }
        else
        {
            //there is no other instance
            song = this;
            DontDestroyOnLoad(transform.gameObject);

        }
    }
}
