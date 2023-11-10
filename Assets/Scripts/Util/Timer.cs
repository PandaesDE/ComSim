using UnityEngine;

public class Timer
{
    private int minutes_max;
    private int minutes_left;
    public Timer(int minutes_max)
    {
        this.minutes_max = minutes_max;
        this.minutes_left = 0;
    }

    public void reset()
    {
        minutes_left = minutes_max;
    }

    public void tick()
    {
        minutes_left = Mathf.Clamp(minutes_left - Gamevariables.MINUTES_PER_TICK, 0, minutes_max);
    }

    public bool finished()
    {
        return minutes_left <= 0;
    }
    
}
