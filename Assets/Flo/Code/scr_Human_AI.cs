using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Human_AI : MonoBehaviour {
    public enum States { Wait, Walk }

    [SerializeField]
    private Vector3[] _PathToEnd;
    [SerializeField]
    private States _State = States.Wait;

    i_Human_Motor motor;
    List<KeyValuePair<int, Vector3>> pathToNextTarget;

	void Start () {
        motor = GetComponent<i_Human_Motor>();
        
	}

    int pathIndex = 0;
    Vector3? nextPoint = null;
	void Update () {
		if(_State == States.Walk)
        {
            if((pathToNextTarget == null || pathToNextTarget.Count == 0) && 
                pathIndex < _PathToEnd.Length)
            {
                pathToNextTarget = scr_Tilemap.Get.GetPath(this.transform.position, _PathToEnd[pathIndex], motor.GetSize() * 0.5f);
                pathIndex++;
            }

            if(nextPoint == null)
            {
                nextPoint = _PathToEnd[0];
            }
        }

        Debug.Log(nextPoint);
	}
}
