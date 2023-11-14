using UnityEngine;

public class SpawnOptions
{
    public int amount = 1;
    public bool isMale = Util.Random.isMale();
    public bool isRandom = false;

    private Vector2 _position = Vector2.zero;
    public Vector2 position
    {
        get
        {
            if (isRandom)
            {
                return randomPosition;
            }
            else
            {
                return _position;
            }
        }
        set
        {
            _position = value;
        }
    }

    public Vector2 randomPosition
    {
        get
        {
            return Util.Random.CoordinateInPlayground();
        }
    }

    public SpawnOptions() { }
    public SpawnOptions(int amount, bool isRandom)
    {
        this.amount = amount;
        this.isRandom = isRandom;
    }

    public SpawnOptions set_Amount(int amount)
    {
        this.amount = amount;
        return this;
    }

    public SpawnOptions set_IsMale(bool isMale)
    {
        this.isMale = isMale;   
        return this;
    }

    public SpawnOptions set_IsRandom(bool isRandom)
    {
        this.isRandom = isRandom;
        return this;
    }

    public SpawnOptions set_Position(Vector2 position)
    {
        this._position = position;
        return this;
    }
}
