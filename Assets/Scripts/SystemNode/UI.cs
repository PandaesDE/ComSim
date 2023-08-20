using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text display_Day;
    [SerializeField] private TMP_Text display_Time;
    [SerializeField] private TMP_Text display_TicksToTime;
    [SerializeField] private TMP_Text display_TicksPerSecond;

    private void Start()
    {
        displayTicksToTime((int)((float)Gamevariables.MINUTES_PER_HOUR / (float)Gamevariables.TICKS_PER_HOUR));
        displayTicksPerSeconds(1 / Time.fixedDeltaTime);
    }

    public void displayTicksToTime(int min)
    {
        string time = min + " Minutes";
        if (min == 1) time = min + " Minute";
        if (min >= 60) time = min / 60 + " Hour";
        display_TicksToTime.text = time + " = 1 Tick";
    }

    public void displayTicksPerSeconds(float tps)
    {
        display_TicksPerSecond.text = tps + " Ticks/Second";
    }


    #region Day & Night Cycle
    public void displayDay(int day)
    {
        display_Day.text = "Day: " + day;
    }

    public void displayTime(string formattedTime)
    {
        display_Time.text = formattedTime;
    }
    #endregion
}
