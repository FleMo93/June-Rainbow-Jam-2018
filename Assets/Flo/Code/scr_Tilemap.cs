﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tilemap : MonoBehaviour {

    private static scr_Tilemap tilemapScript = null;
    public static scr_Tilemap Get
    {
        get
        {
            if(tilemapScript == null)
            {
                tilemapScript = GameObject.FindObjectOfType<scr_Tilemap>();
            }

            return tilemapScript;
        }
    }

    public Vector3 GetNextTile(scr_Stats.Directions direction, Vector3 position)
    {
        Vector3 target = position;

        switch (direction)
        {
            case scr_Stats.Directions.Up:
                target = new Vector3(
                    target.x,
                    target.y,
                    RoundToDecimal5(target.z + 1)
                    );
                break;

            case scr_Stats.Directions.Right:
                target = new Vector3(
                    RoundToDecimal5(target.x + 1),
                    target.y,
                    target.z
                    );
                break;

            case scr_Stats.Directions.Down:
                target = new Vector3(
                    target.x,
                    target.y,
                    RoundToDecimal5(target.z - 1)
                    );
                break;

            case scr_Stats.Directions.Left:
                target = new Vector3(
                    RoundToDecimal5(target.x - 1),
                    target.y,
                    target.z
                    );
                break;
        }

        return target;
    }

    public Collider[] GetCollidersOnTile(Vector3 position, Vector3 halfExtents)
    {
        Vector3 center = new Vector3(
            position.x,
            position.y + halfExtents.y / 2,
            position.z
            );

        halfExtents = new Vector3(
            halfExtents.x - 0.001f,
            halfExtents.y - 0.001f,
            halfExtents.z - 0.001f
            );

        return Physics.OverlapBox(center, halfExtents);
    }

    public bool IsTileFree(Vector3 position, Vector3 halfExtents)
    {
        return GetCollidersOnTile(position, halfExtents).Length == 0;
    }

    private float RoundToDecimal5(float val)
    {
        val = val * 10;
        val = Mathf.Round(val / 5.0f) * 5;
        return val / 10;
    }
}