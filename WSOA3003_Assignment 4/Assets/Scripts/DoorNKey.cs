using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNKey : MonoBehaviour
{
    public bool isKey, isDoor;
    public bool unlocked;
    public GameObject keyUI;
    public DoorNKey targetdoor;
    public MiscScript misc;

    




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(isKey)
            {
                targetdoor.unlocked = true;
                keyUI.SetActive(true);
                Destroy(this.gameObject);
            }


            if(isDoor)
            {
                if (unlocked)
                {
                    misc.TogglePrompt(1);
                    
                }
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDoor)
        { 
            misc.TogglePrompt(2); 
        }
    }

    public void Update()
    {
        if (unlocked)
        {
            if (Input.GetKeyDown("e"))
            {
                if (misc.prompt.activeSelf == true)
                { Destroy(keyUI);
                    misc.prompt.SetActive(false);
                    Destroy(this.gameObject);
                    Debug.Log("nani2");
                }

                Debug.Log("nani");
            }
        }
    }
}
