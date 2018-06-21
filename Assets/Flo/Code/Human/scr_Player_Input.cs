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

    private bool selectHumanUp = false;
    private bool selectHumanRight = false;
    private bool selectHumanDown = false;
    private bool selectHumanLeft = false;
    private bool selectHuman = false;
    private bool stopSelectHuman = false;

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

    public bool IsSelectHumanUp()
    {
        return selectHumanUp;
    }

    public bool IsSelectHumanRight()
    {
        return selectHumanRight;
    }

    public bool IsSelectHumanDown()
    {
        return selectHumanDown;
    }

    public bool IsSelectHumanLeft()
    {
        return selectHumanLeft;
    }

    public bool IsSelectHuman()
    {
        return selectHuman;
    }

    public bool IsStopSelectHuman()
    {
        return stopSelectHuman;
    }

    void Update ()
    {
        moveRight = Input.GetAxisRaw("Horizontal") >= 0.5f;
        moveLeft = Input.GetAxisRaw("Horizontal") <= -0.5f;
        moveUp = Input.GetAxisRaw("Vertical") >= 0.5f;
        moveDown = Input.GetAxisRaw("Vertical") <= -0.5f;
        interacting = Input.GetButtonDown("Interact");

        selectHumanUp = Input.GetButtonDown("HumanUp");
        selectHumanRight = Input.GetButtonDown("HumanRight");
        selectHumanDown = Input.GetButtonDown("HumanDown");
        selectHumanLeft = Input.GetButtonDown("HumanLeft");
        selectHuman = Input.GetButtonDown("SelectHuman");
        stopSelectHuman = Input.GetButtonDown("StopSelectHuman");
    }

    
}
