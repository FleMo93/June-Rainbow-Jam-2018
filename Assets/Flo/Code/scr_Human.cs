using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Human : MonoBehaviour, i_Interactable {
    [SerializeField]
    private GameObject _Model;
    [SerializeField]
    private float _MinRandomRotationTime = 0;
    [SerializeField]
    private float _MaxRandomRotationTime = 0;


    private scr_Stats stats;
    private scr_Stats.Directions rotateTowardsDirection = scr_Stats.Directions.None;
    private Vector3 rotateTowardsVector;
    private float timeToNextRandom = 0;

    void Start()
    {
        stats = GetComponent<scr_Stats>();
        timeToNextRandom = Random.Range(_MinRandomRotationTime, _MaxRandomRotationTime);

        _Model.transform.LookAt(_Model.transform.position + scr_Tilemap.Get.DirectionToVector(GetRandomDirection()));
        
    }

    public scr_Stats.Interaction Interact(GameObject trigger)
    {
        rotateTowardsDirection = scr_Tilemap.Get.GetDirectionFromTo(trigger.transform.position, this.gameObject.transform.position);
        rotateTowardsVector = scr_Tilemap.Get.DirectionToVector(rotateTowardsDirection);
        timeToNextRandom = _MaxRandomRotationTime;

        return scr_Stats.Interaction.TalkToHuman;   
    }

	void Update () {
        Rotate();
        SetRandomRotation();
	}

    void Rotate()
    {
        if(rotateTowardsDirection == scr_Stats.Directions.None)
        {
            return;
        }

        _Model.transform.rotation = Quaternion.Slerp(
            _Model.transform.rotation,
            Quaternion.LookRotation(rotateTowardsVector),
            stats.RotationSpeed * Time.deltaTime);
    }

    void SetRandomRotation()
    {
        timeToNextRandom -= Time.deltaTime;

        if(timeToNextRandom <= 0)
        {
            timeToNextRandom = Random.Range(_MinRandomRotationTime, _MaxRandomRotationTime);

            rotateTowardsDirection = GetRandomDirection();
            rotateTowardsVector = scr_Tilemap.Get.DirectionToVector(rotateTowardsDirection);
        }
    }

    private scr_Stats.Directions GetRandomDirection()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                return scr_Stats.Directions.None;
            case 1:
                return scr_Stats.Directions.Up;
            case 2:
                return scr_Stats.Directions.Right;
            case 3:
                return scr_Stats.Directions.Down;
            case 4:
                return scr_Stats.Directions.Left;
            default:
                return scr_Stats.Directions.None;
        }
    }
}
