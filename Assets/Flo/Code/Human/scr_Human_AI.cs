using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Human_AI : MonoBehaviour, i_Interactable
{
    public enum States { Wait, Walk }

    [SerializeField]
    private Vector3[] _PathToEnd;
    [SerializeField]
    private States _State = States.Wait;
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

    i_Human_Motor motor;
    List<Vector3> pathToTarget;

	void Start () {
        motor = GetComponent<i_Human_Motor>();
        stats = GetComponent<scr_Stats>();

        pathToTarget = GetPath();
        timeToNextRandom = Random.Range(_MinRandomRotationTime, _MaxRandomRotationTime);

        _Model.transform.LookAt(_Model.transform.position + scr_Tilemap.Get.DirectionToVector(GetRandomDirection()));
    }

    public scr_Interactable_Result Interact(GameObject trigger, scr_Stats.ObjectType itemInInventory)
    {
        rotateTowardsDirection = scr_Tilemap.Get.GetDirectionFromTo(this.gameObject.transform.position, trigger.transform.position);
        rotateTowardsVector = scr_Tilemap.Get.DirectionToVector(rotateTowardsDirection);
        timeToNextRandom = _MaxRandomRotationTime;

        return new scr_Interactable_Result(scr_Stats.Interaction.TalkToHuman, true);
    }

    Vector3? nextPoint = null;
	void Update () {
		if(_State == States.Walk)
        {
            Walk();
        }
        else if(_State == States.Wait)
        {
            Wait();
        }
	}

    void Walk()
    {
        if (!nextPoint.HasValue || this.transform.position == nextPoint)
        {
            if (nextPoint.HasValue)
            {
                pathToTarget.Remove(nextPoint.Value);
            }

            if (pathToTarget.Count == 0)
            {
                _State = States.Wait;
                return;
            }

            nextPoint = pathToTarget[0];
        }

        switch (scr_Tilemap.Get.GetDirectionFromTo(this.transform.position, nextPoint.Value))
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

    void Wait()
    {
        Rotate();
        SetRandomRotation();
    }

    private List<Vector3> GetPath()
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 lastPoint = this.transform.position;

        foreach(Vector3 wp in _PathToEnd)
        {
            foreach(Vector3 sp in scr_Tilemap.Get.GetPath(lastPoint, wp, motor.GetSize() * 0.5f))
            {
                path.Add(sp);
            }

            lastPoint = path[path.Count - 1];
        }

        return path;
    }




    void Rotate()
    {
        if (rotateTowardsDirection == scr_Stats.Directions.None)
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

        if (timeToNextRandom <= 0)
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
