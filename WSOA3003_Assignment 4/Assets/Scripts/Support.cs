using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{

    public bool isDummy;
    public MiscScript misc;
    public int clicks;


    public bool canfollow;
    public SpriteRenderer sprite;
    void Start()
    {
        clicks = 0;
    }

    
    void Update()
    {



        if (canfollow)
        { sprite.enabled = true;
            canfollow = false;
        }


        if (isDummy)

        {
            if (Input.GetKeyDown("e"))
            {
                misc.ToggleInstructions(1);
                misc.prompt.SetActive(false);
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (clicks == 0)
                {
                    clicks += 1;
                    misc.ToggleInstructions(2);

                }
                else if (clicks == 1)
                {
                    misc.ToggleInstructions(2);
                    misc.support.canfollow = true;
                Destroy(misc.firstwalls1.gameObject);
                Destroy(this.gameObject);

                }

                    

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isDummy)
            {
                misc.TogglePrompt(1);
            }
        }



    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isDummy)
        {
            misc.TogglePrompt(2);
        }
    }
}
