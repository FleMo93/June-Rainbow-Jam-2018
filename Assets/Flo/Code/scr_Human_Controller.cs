using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Human_Controller : MonoBehaviour {

    i_Player_Input input;
    i_Human_Motor motor;
    scr_Stats.Directions activeDirection = scr_Stats.Directions.None;

	// Use this for initialization
	void Start () {
        input = GetComponent<i_Player_Input>();
        motor = GetComponent<i_Human_Motor>();
	}
	
	// Update is called once per frame
	void Update () {

        //mc: removed NullEx
        if (input == null)
        {
            return;
        }

        if(activeDirection != scr_Stats.Directions.None)
        {
            if ((activeDirection == scr_Stats.Directions.Up && !input.IsMovingUp()) ||
                (activeDirection == scr_Stats.Directions.Right && !input.IsMovingRight()) ||
                (activeDirection == scr_Stats.Directions.Down && !input.IsMovingDown()) ||
                (activeDirection == scr_Stats.Directions.Left && !input.IsMovingLeft())
                )
            {
                activeDirection = scr_Stats.Directions.None;
            }
        }

        if (activeDirection == scr_Stats.Directions.None)
        {
            if (input.IsMovingUp())
            {
                activeDirection = scr_Stats.Directions.Up;
            }
            else if (input.IsMovingRight())
            {
                activeDirection = scr_Stats.Directions.Right;
            }
            else if (input.IsMovingDown())
            {
                activeDirection = scr_Stats.Directions.Down;
            }
            else if (input.IsMovingLeft())
            {
                activeDirection = scr_Stats.Directions.Left;
            }
            else
            {
                activeDirection = scr_Stats.Directions.None;
            }
        }

        if (activeDirection == scr_Stats.Directions.None)
        {
            if (input.IsInteracting())
            {
                motor.Interact();
            }
        }
        else
        {
            switch(activeDirection)
            {
                case scr_Stats.Directions.Up:
                    motor.MoveUp();
                    break;
                case scr_Stats.Directions.Right:
                    motor.MoveRight();
                    break;
                case scr_Stats.Directions.Down:
                    motor.MoveDown();
                    break;
                case scr_Stats.Directions.Left:
                    motor.MoveLeft();
                    break;
            }
        }
	}
}
