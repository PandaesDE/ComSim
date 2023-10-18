using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private Creature creature;
    public float speed { get; set; }       //moves per Minute

    [SerializeField] public  Vector2 target
    {
        protected set;
        get;
    }
    [SerializeField] public Direction facing
    { 
        get;
        protected set;
    }

    [SerializeField] private Vector2Int nextSteps = Vector2Int.zero;
    [SerializeField] private float leftOverSteps = 0;   //in between moves


    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public Movement(Creature creature)
    {
        this.creature = creature;
    }

    public void MoveToTarget()
    {
        float theoreticalMoves = speed * Gamevariables.MINUTES_PER_TICK + leftOverSteps;
        int moves = (int)theoreticalMoves;
        leftOverSteps = theoreticalMoves - moves;

        for (int i = 0; i < moves; i++)
        {
            //chance to not make a move based on health
            if (Util.Random.Float(0f, 1f) > creature.health / creature.MAX_HEALTH)
                continue;

            //calculate new destination if reached in between ticks
            if (Util.isDestinationReached(creature.transform.position, target))
                setRandomTarget();

            if (nextSteps == Vector2.zero)
                CalculateNextSteps(target);

            if (Mathf.Abs(nextSteps.x) > Mathf.Abs(nextSteps.y))
            {
                if (nextSteps.x > 0)
                {
                    facing = Direction.EAST;
                    MakeStep();
                }
                else
                {
                    facing = Direction.WEST;
                    MakeStep();
                }
            }
            else
            {
                if (nextSteps.y > 0)
                {
                    facing = Direction.NORTH;
                    MakeStep();
                }
                else
                {
                    facing = Direction.SOUTH;
                    MakeStep();
                }
            }
        }
    }

    public void CalculateNextSteps(Vector3 destination)
    {
        Vector2 vect = Util.Conversion.Vector3ToVector2(destination - creature.transform.position);

        if (vect.x == 0 || vect.y == 0)
        {
            nextSteps = new Vector2Int((int)vect.x, (int)vect.y);
            return;
        }

        if (Mathf.Abs(vect.x) >= Mathf.Abs(vect.y))
        {
            int x = Util.roundFloatUpPositiveDownNegative(vect.x / Mathf.Abs(vect.y));
            int y = 1;
            if (vect.y < 0) y = -1;
            nextSteps = new Vector2Int(x, y);
        }
        else
        {
            int y = Util.roundFloatUpPositiveDownNegative(vect.y / Mathf.Abs(vect.x));
            int x = 1;
            if (vect.x < 0) x = -1;
            nextSteps = new Vector2Int(x, y);
        }
    }

    public void MakeStep()
    {
        if (facing == Direction.NORTH)
        {
            nextSteps -= Vector2Int.up;
            creature.transform.position += Vector3.up;
            creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            return;
        }
        if (facing == Direction.EAST)
        {
            nextSteps -= Vector2Int.right;
            creature.transform.position += Vector3.right;
            creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            return;
        }
        if (facing == Direction.SOUTH)
        {
            nextSteps -= Vector2Int.down;
            creature.transform.position += Vector3.down;
            creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            return;
        }
        if (facing == Direction.WEST)
        {
            nextSteps -= Vector2Int.left;
            creature.transform.position += Vector3.left;
            creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
            return;
        }
    }

    public void setRandomTarget()
    {
        target = Util.Random.CoordinateInPlayground();
        nextSteps = Vector2Int.zero;
    }

    public void setTarget(Vector2 destination)
    {
        target = destination;
        nextSteps = Vector2Int.zero;
    }
}
