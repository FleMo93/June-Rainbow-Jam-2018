using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public scr_Stats.Directions GetDirectionFromTo(Vector3 from, Vector3 to)
    {
        Vector3 dir = (from - to).normalized;
        if(dir.x == 0 && dir.z == 0)
        {
            return scr_Stats.Directions.None;
        }
        else if(dir.z > 0 && Mathf.Abs(dir.z) > Mathf.Abs(dir.x))
        {
            return scr_Stats.Directions.Up;
        }
        else if(dir.z < 0 && Mathf.Abs(dir.z) > Mathf.Abs(dir.x))
        {
            return scr_Stats.Directions.Down;
        }
        else if(dir.x > 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            return scr_Stats.Directions.Right;
        }
        else if(dir.x < 0 && Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            return scr_Stats.Directions.Left;
        }

        return scr_Stats.Directions.None;
    }

    public Vector3 DirectionToVector(scr_Stats.Directions direction)
    {
        switch(direction)
        {
            case scr_Stats.Directions.Up:
                return Vector3.forward;
            case scr_Stats.Directions.Right:
                return Vector3.right;
            case scr_Stats.Directions.Down:
                return Vector3.back;
            case scr_Stats.Directions.Left:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }

    #region pathfinding
    private class PathfindingTile
    {
        public Vector3 Position { get; private set; }
        public int EstimatedCosts { get; private set; }
        public PathfindingTile LastTile { get; private set; }

        public PathfindingTile(Vector3 tile, int estimatedCosts, PathfindingTile lastTile)
        {
            Position = tile;
            EstimatedCosts = estimatedCosts;
            LastTile = lastTile;
        }
    }

    public List<KeyValuePair<int, Vector3>> GetPath(Vector3 from, Vector3 to, Vector3 halfExtends)
    {
        List<PathfindingTile> closedList = new List<PathfindingTile>();
        List<PathfindingTile> openList = new List<PathfindingTile>();


        openList.Add(new PathfindingTile(from, EstimateCosts(from, to), null));
        


        while(openList.Where(x => x.EstimatedCosts == 0).Count() == 0)
        {
            PathfindingTile tileToCheck = openList.OrderBy(x => x.EstimatedCosts).First();
            openList.AddRange(GetNeighbors(tileToCheck, to, halfExtends));
            openList.Remove(tileToCheck);
        }

        List<Vector3> points = new List<Vector3>();
        PathfindingTile nextTile = openList.OrderBy(x => x.EstimatedCosts).First();
        bool pathFound = false;

        while (!pathFound)
        {

            points.Add(nextTile.Position);

            if (nextTile.LastTile == null || nextTile.LastTile.Position == from)
            {
                pathFound = true;
                break;
            }

            nextTile = nextTile.LastTile;
        }

        int counter = 0;
        List<KeyValuePair<int, Vector3>> pointsWithOrder = new List<KeyValuePair<int, Vector3>>();

        for(var i = points.Count - 1; i >= 0; i--)
        {
            pointsWithOrder.Add(new KeyValuePair<int, Vector3>(counter, points[i]));
            counter++;
        }

        return pointsWithOrder.OrderBy(x => x.Key).ToList();
    }

    

    
    private PathfindingTile[] GetNeighbors(PathfindingTile tile, Vector3 target, Vector3 halfExtends)
    {
        List<PathfindingTile> neighbors = new List<PathfindingTile>();

        Vector3 pos = new Vector3(tile.Position.x, tile.Position.y, tile.Position.z + 1);
        List<Collider> colliders = GetCollidersOnTile(pos, halfExtends).ToList();
        colliders.RemoveAll(x => x.gameObject.tag == scr_Tags.Human || x.gameObject.tag == scr_Tags.Player);
        if (colliders.Count == 0)
        {
            neighbors.Add(new PathfindingTile(pos, EstimateCosts(pos, target), tile));
        }



        pos = new Vector3(tile.Position.x + 1, tile.Position.y, tile.Position.z);
        colliders = GetCollidersOnTile(pos, halfExtends).ToList();
        colliders.RemoveAll(x => x.gameObject.tag == scr_Tags.Human || x.gameObject.tag == scr_Tags.Player);
        if (colliders.Count == 0)
        {
            neighbors.Add(new PathfindingTile(pos, EstimateCosts(pos, target), tile));
        }



        pos = new Vector3(tile.Position.x, tile.Position.y, tile.Position.z - 1);
        colliders = GetCollidersOnTile(pos, halfExtends).ToList();
        colliders.RemoveAll(x => x.gameObject.tag == scr_Tags.Human || x.gameObject.tag == scr_Tags.Player);
        if (colliders.Count == 0)
        {
            neighbors.Add(new PathfindingTile(pos, EstimateCosts(pos, target), tile));
        }



        pos = new Vector3(tile.Position.x - 1, tile.Position.y, tile.Position.z);
        colliders = GetCollidersOnTile(pos, halfExtends).ToList();
        colliders.RemoveAll(x => x.gameObject.tag == scr_Tags.Human || x.gameObject.tag == scr_Tags.Player);
        if (colliders.Count == 0)
        {
            neighbors.Add(new PathfindingTile(pos, EstimateCosts(pos, target), tile));
        }

        return neighbors.ToArray();
    }

    private int EstimateCosts(Vector3 from, Vector3 to)
    {
        return Mathf.Abs((int)(from.x * 10 - to.x * 10) / 10) + Mathf.Abs((int)(from.z * 10 - to.z * 10) / 10);
    }
    #endregion
}
