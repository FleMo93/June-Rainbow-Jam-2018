using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Player_Input : MonoBehaviour, i_Player_Input
{
    private bool interacting = false;
    private bool moveRight = false;
    private bool moveDown = false;
    private bool moveLeft = false;
    private bool moveUp = false;

    public bool IsInteracting()
    {
        return interacting;
    }

    public bool IsMovingRight()
    {
        return moveRight;
    }
     

    public bool IsMovingDown()
    {
        return moveDown;
    }

    public bool IsMovingLeft()
    {
        return moveLeft;
    }

    public bool IsMovingUp()
    {
        return moveUp;
    }

	
	void Update ()
    {
        interacting = false;
        moveRight = false;
        moveLeft = false;
        moveUp = false;
        moveDown = false;

        if(Input.GetAxisRaw("Horizontal") >= 0.5f)
        {
            moveRight = true;
        }
        else if(Input.GetAxisRaw("Horizontal") <= -0.5f)
        {
            moveLeft = true;
        }
        else if(Input.GetAxisRaw("Vertical") >= 0.5f)
        {
            moveUp = true;
        }
        else if(Input.GetAxisRaw("Vertical") <= -0.5f)
        {
            moveDown = true;
        }
        else if(Input.GetButtonDown("Fire1"))
        {
            interacting = true;
        }
    }
}
