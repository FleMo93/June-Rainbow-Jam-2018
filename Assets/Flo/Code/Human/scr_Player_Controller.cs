using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Player_Controller : MonoBehaviour {
    private enum States { FreeMovement, HumanSelection }

    [SerializeField]
    private States _State = States.FreeMovement;
    [SerializeField]
    private GameObject _Camera;
    [SerializeField]
    private float _CameraSpeed = 15;
    [SerializeField]
    private float _OutlineWidthOnSelect = 0.1f;


    i_Player_Input input;
    i_Human_Motor motor;
    scr_Stats.Directions activeDirection = scr_Stats.Directions.None;
    GameObject[] humans;
    Vector3 cameraOffset;
    bool cameraSwitchFinished = true;

	// Use this for initialization
	void Start () {
        input = GetComponent<i_Player_Input>();
        motor = GetComponent<i_Human_Motor>();
        humans = GameObject.FindGameObjectsWithTag(scr_Tags.Human);
        cameraOffset = _Camera.transform.position - this.transform.position;
	}
	
    void SetState(States state)
    {
        _State = state;
        cameraSwitchFinished = false;
    }

    // Update is called once per frame
    void Update ()
    {
        //mc: removed NullEx
        if (input == null)
        {
            return;
        }

        switch (_State)
        {
            case States.FreeMovement:
                if (cameraSwitchFinished)
                {
                    FreeMovement();
                }
                break;

            case States.HumanSelection:
                HumanSelection();
                break;
        }

        if(!cameraSwitchFinished)
        {
            MoveCamera();
        }
	}

    Vector3 cameraTargetPosition;
    void MoveCamera()
    {
        _Camera.transform.position = Vector3.MoveTowards(
            _Camera.transform.position,
            cameraTargetPosition,
            _CameraSpeed * Time.deltaTime);

        if(_Camera.transform.position == cameraTargetPosition)
        {
            cameraSwitchFinished = true;
        }
    }

    void FreeMovement()
    {
        if (activeDirection != scr_Stats.Directions.None)
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

        if (input.IsInteracting())
        {
            //TODO: create and talk to animation controller
            if(motor.Interact() == scr_Stats.Interaction.Altar)
            {
                SetState(States.HumanSelection);
            }
        }

        if (activeDirection != scr_Stats.Directions.None)
        {
            switch (activeDirection)
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

    #region human selection
    GameObject selectedHuman = null;

    void HumanSelection()
    {
        if(humans == null || humans.Length == 0)
        {
            return;
        }

        if(selectedHuman == null)
        {
            selectedHuman = humans[0];
            cameraTargetPosition = selectedHuman.transform.position + cameraOffset;

            foreach (Renderer rend in selectedHuman.GetComponentsInChildren<Renderer>())
            {
                rend.material.SetFloat("_OutlineWidth", _OutlineWidthOnSelect);
            }
        }


        GameObject newSelectedHuman = null;

        if (input.IsSelectHumanUp())
        {
            newSelectedHuman = GetNextHuman(selectedHuman, scr_Stats.Directions.Up, scr_Stats.Directions.Left, scr_Stats.Directions.Right);
        }
        else if(input.IsSelectHumanRight())
        {
            newSelectedHuman = GetNextHuman(selectedHuman, scr_Stats.Directions.Right, scr_Stats.Directions.Up, scr_Stats.Directions.Down);
        }
        else if(input.IsSelectHumanDown())
        {
            newSelectedHuman = GetNextHuman(selectedHuman, scr_Stats.Directions.Down, scr_Stats.Directions.Left, scr_Stats.Directions.Right);
        }
        else if(input.IsSelectHumanLeft())
        {
            newSelectedHuman = GetNextHuman(selectedHuman, scr_Stats.Directions.Left, scr_Stats.Directions.Up, scr_Stats.Directions.Down);
        }
        else if(input.IsSelectHuman())
        {
            foreach (Renderer rend in selectedHuman.GetComponentsInChildren<Renderer>())
            {
                rend.material.SetFloat("_OutlineWidth", 0.0f);
            }

            cameraTargetPosition = this.transform.position + cameraOffset;
            SetState(States.FreeMovement);
            return;
        }
        else if(input.IsStopSelectHuman())
        {
            foreach (Renderer rend in selectedHuman.GetComponentsInChildren<Renderer>())
            {
                rend.material.SetFloat("_OutlineWidth", 0.0f);
            }

            cameraTargetPosition = this.transform.position + cameraOffset;
            SetState(States.FreeMovement);
            selectedHuman = null;
            return;
        }

        if(newSelectedHuman != null && newSelectedHuman != selectedHuman)
        {
            foreach(Renderer rend in newSelectedHuman.GetComponentsInChildren<Renderer>())
            {
                rend.material.SetFloat("_OutlineWidth", _OutlineWidthOnSelect);
            }

            foreach (Renderer rend in selectedHuman.GetComponentsInChildren<Renderer>())
            {
                rend.material.SetFloat("_OutlineWidth", 0.0f);
            }

            selectedHuman = newSelectedHuman;
            cameraTargetPosition = selectedHuman.transform.position + cameraOffset;
        }

        if (cameraSwitchFinished)
        {
            MoveCamera();
        }
    }

    private GameObject GetNextHuman(GameObject selectedHuman, scr_Stats.Directions mainDirection, scr_Stats.Directions firstAltDir, scr_Stats.Directions secAltDir)
    {
        GameObject target = null;
        float rangeToTarget =  float.MaxValue;
        GameObject alternativeTarget = null;
        float alternativeRangeToTarget = float.MaxValue;

        foreach (GameObject human in humans)
        {
            scr_Stats.Directions dir = scr_Tilemap.Get.GetDirectionFromTo(selectedHuman.transform.position, human.transform.position);
            float range = Vector3.Distance(human.transform.position, selectedHuman.transform.position);

            if (dir == mainDirection)
            {
                if(range < rangeToTarget)
                {
                    target = human;
                    rangeToTarget = range;
                }
            }
            else if (target == null && (dir == firstAltDir || dir == secAltDir))
            {
                if(range < alternativeRangeToTarget)
                {
                    alternativeTarget = human;
                    alternativeRangeToTarget = range;
                }
            }
        }

        return target != null ? target : alternativeTarget;
    }
    #endregion
}
