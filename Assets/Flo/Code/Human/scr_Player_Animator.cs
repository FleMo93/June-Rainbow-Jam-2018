using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Player_Animator : MonoBehaviour {

    [SerializeField]
    private Animator antr;
    [SerializeField]
    private float _MinTimeToIdle2 = 2f;
    [SerializeField]
    private float _MaxTimeToIdle2 = 5f;

    i_Human_Motor motor;
    private float timeToIdle2;

	// Use this for initialization
	void Start () {
        motor = GetComponent<i_Human_Motor>();
	}
	
	// Update is called once per frame
	void Update () {
        var state = motor.GetState();

        if (state == scr_Human_Motor.MotorStates.Idle)
        {
            timeToIdle2 -= Time.deltaTime;
            if (timeToIdle2 <= 0)
            {
                antr.SetBool("Idle2", true);
                timeToIdle2 = Random.Range(_MinTimeToIdle2, _MaxTimeToIdle2);
            }
            else
            {
                antr.SetBool("Idle2", false);
            }
        }

		switch(motor.GetState())
        {
            case scr_Human_Motor.MotorStates.Walk:
                antr.SetBool("Walk", true);
                break;
            case scr_Human_Motor.MotorStates.Idle:
                antr.SetBool("Walk", false);
                break;
        }
	}
}
