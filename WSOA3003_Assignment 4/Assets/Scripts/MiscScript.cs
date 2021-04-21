using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscScript : MonoBehaviour
{
    public StateMachine state;
    public Support dummy, support;
    public GameObject prompt;
    public CharacterMovement player;

    public GameObject firstwalls1;

    public GameObject instruction1, instruction2;
    public bool InstructionUp;

    public GameObject purplekey;
    public bool upgraded;
    void Start()
    {

    }

    
    void Update()
    {
        //if (Input.GetKeyDown("e"))
        //{
        //    TogglePrompt(2);
        //}

        //if (InstructionUp == true)
        //{
        //    if(Input.GetMouseButtonDown(0))
        //    {
        //        ToggleInstructions(1);
        //    }
        //}

    }

    public void UpgradeWater()
    {
        if (!upgraded)
        { dummy.gameObject.SetActive(true);
            dummy.transform.position = new Vector3(state.enemylastPos.x, state.enemylastPos.y, state.enemylastPos.z);
            upgraded = true;
        }


    }

    public void TogglePrompt(int type)
    {
        if (type == 1)
        {
            if (prompt.activeSelf == false)
            {
                prompt.SetActive(true);
            }
        }
        else if (type == 2)
        {
            if (prompt.activeSelf == true)
            {
                prompt.SetActive(false);
            }
        }



    }


    public void ToggleInstructions(int type)
    {
        if (type == 1)
        {
            if (instruction1.activeSelf == false)
            {
                instruction1.gameObject.SetActive(true);
                InstructionUp = true;
                player.canmove = false;
            }
            else if (instruction1.activeSelf == true)
            {
                instruction1.gameObject.SetActive(false);
                InstructionUp = false;
                Destroy(instruction1);
                player.canmove = true;
            }

        }

        if (type == 2)
        {
            if (instruction2.activeSelf == false)
            {
                instruction2.gameObject.SetActive(true);
                InstructionUp = true;
                player.canmove = false;
                Destroy(instruction1);
            }
            else if (instruction2.activeSelf == true)
            {
                instruction2.gameObject.SetActive(false);
                InstructionUp = false;
                Destroy(instruction2);
                player.canmove = true;
            }

        }
    }

    public void ToggleKey()
    {
        purplekey.SetActive(true);
    }
}
