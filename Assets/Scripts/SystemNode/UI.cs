using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    InputManager im;

    [SerializeField] private TMP_Text display_Day;
    [SerializeField] private TMP_Text display_Time;
    [SerializeField] private TMP_Text display_TicksToTime;
    [SerializeField] private TMP_Text display_TicksPerSecond;

    [SerializeField] private Button btn_PAUSE;
    [SerializeField] private Button btn_HOME;

    [SerializeField] private Slider sdr_TicksPerSecond;
    [SerializeField] private Slider sdr_TicksToTime;

    private void Awake()
    {
        im = GetComponent<InputManager>();
        btn_PAUSE.onClick.AddListener(im.pauseGame);
        btn_HOME.onClick.AddListener(im.toMainMenu);
        initializeTicksPerSecondSlider();
        initializeTicksToTimeSlider();

        void initializeTicksPerSecondSlider()
        {
            sdr_TicksPerSecond.onValueChanged.AddListener(im.changeTicksPerSecond);
            float tps = 1 / Time.fixedDeltaTime;
            displayTicksPerSeconds(tps);
            sdr_TicksPerSecond.value = tps;
        }

        void initializeTicksToTimeSlider()
        {
            sdr_TicksToTime.onValueChanged.AddListener(im.changeTicksToTime);
            int ttt = (int)((float)Gamevariables.MINUTES_PER_HOUR / (float)Gamevariables.TICKS_PER_HOUR);
            displayTicksToTime(ttt);
            sdr_TicksToTime.value = ttt;
        }
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

    public void displayPauseButtonText(string txt)
    {
        btn_PAUSE.transform.GetChild(0).GetComponent<TMP_Text>().text = txt;
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
