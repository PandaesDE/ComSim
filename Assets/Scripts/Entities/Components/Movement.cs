/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Movement component of a creature
 *  
 *  References:
 *      Scene:
 *          - Indirectly (Component of Creature.cs) for simulation scene(s)
 *      Script:
 *          - One instance per creature
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Movement
{
    private Creature _creature;
    public float Speed { get; set; } = .2f;      //moves per Minute

    private bool _isMoving = false;
    private GameObject _movingTarget = null;
    [SerializeField] public Vector2 Target
    {
        get;
        protected set;
    }
    [SerializeField] public Direction Facing
    { 
        get;
        protected set;
    }

    [SerializeField] private Vector2Int _nextSteps = Vector2Int.zero;
    [SerializeField] private float _leftOverSteps = 0;   //in between moves | Subtick


    public enum Direction
    {
        north,
        east,
        south,
        west
    }

    public Movement(Creature creature)
    {
        this._creature = creature;
        Target = Util.Random.CoordinateInPlayground();
    }

    public void SetRandomTarget()
    {
        SetTarget(Util.Random.CoordinateInPlayground());
    }

    public void SetRandomTargetIfReached()
    {
        if (TargetReached()) SetRandomTarget();
    }

    public void SetMovingTarget(GameObject g)
    {
        _isMoving = true;
        _movingTarget = g;
    }

    public void SetTarget(Vector2 destination)
    {
        _isMoving = false;
        float x = Mathf.Clamp(destination.x, -Gamevariables.PLAYGROUND_SIZE.x / 2, Gamevariables.PLAYGROUND_SIZE.x / 2);
        float y = Mathf.Clamp(destination.y, -Gamevariables.PLAYGROUND_SIZE.y / 2, Gamevariables.PLAYGROUND_SIZE.y / 2);

        destination = new Vector2(x, y);
        Target = destination;
        _nextSteps = Vector2Int.zero;
    }

    public bool TargetReached()
    {
        return Util.InRange(_creature.transform.position, Target);
    }

    public void MoveToTarget()
    {
        if (_isMoving)
        {
            Target = _movingTarget.transform.position;
        }
        if (Util.InRange(_creature.transform.position, Target))
            return;
        float theoreticalMoves = Speed * Gamevariables.MinutesPerTick + _leftOverSteps;
        int moves = (int)theoreticalMoves;
        _leftOverSteps = theoreticalMoves - moves;

        for (int i = 0; i < moves; i++)
        {
            //chance to not make a move based on health
            if (Util.Random.Float(0f, 1f) > _creature.Health / _creature.maxHealth)
                continue;

            if (_nextSteps == Vector2.zero)
                CalculateNextSteps(Target);

            if (Mathf.Abs(_nextSteps.x) > Mathf.Abs(_nextSteps.y))
            {
                if (_nextSteps.x > 0)
                {
                    Facing = Direction.east;
                    MakeStep();
                }
                else
                {
                    Facing = Direction.west;
                    MakeStep();
                }
            }
            else
            {
                if (_nextSteps.y > 0)
                {
                    Facing = Direction.north;
                    MakeStep();
                }
                else
                {
                    Facing = Direction.south;
                    MakeStep();
                }
            }
        }
    }

    private void CalculateNextSteps(Vector3 destination)
    {
        Vector2 vect = Util.Conversion.Vector3ToVector2(destination - _creature.transform.position);

        if (vect.x == 0 || vect.y == 0)
        {
            _nextSteps = new Vector2Int((int)vect.x, (int)vect.y);
            return;
        }

        if (Mathf.Abs(vect.x) >= Mathf.Abs(vect.y))
        {
            int x = Util.RoundFloatUpPositiveDownNegative(vect.x / Mathf.Abs(vect.y));
            int y = 1;
            if (vect.y < 0) y = -1;
            _nextSteps = new Vector2Int(x, y);
        }
        else
        {
            int y = Util.RoundFloatUpPositiveDownNegative(vect.y / Mathf.Abs(vect.x));
            int x = 1;
            if (vect.x < 0) x = -1;
            _nextSteps = new Vector2Int(x, y);
        }
    }

    private void MakeStep()
    {
        if (Facing == Direction.north)
        {
            _nextSteps -= Vector2Int.up;
            _creature.transform.position += Vector3.up;
            _creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            return;
        }
        if (Facing == Direction.east)
        {
            _nextSteps -= Vector2Int.right;
            _creature.transform.position += Vector3.right;
            _creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            return;
        }
        if (Facing == Direction.south)
        {
            _nextSteps -= Vector2Int.down;
            _creature.transform.position += Vector3.down;
            _creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            return;
        }
        if (Facing == Direction.west)
        {
            _nextSteps -= Vector2Int.left;
            _creature.transform.position += Vector3.left;
            _creature.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
            return;
        }
    }
}
