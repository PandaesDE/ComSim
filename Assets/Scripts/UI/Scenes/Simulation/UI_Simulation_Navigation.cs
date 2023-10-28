/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Simulation_Navigation : MonoBehaviour
{

    InputManager im;

    //Functionality
    [SerializeField] private Button btn_Pause;
    [SerializeField] private Button btn_Home;

    //Context Menu
    [SerializeField] private Button btn_to_HomeCm;

    [SerializeField] private UI_Simulation_ContextMenu head;
    [SerializeField] private Button btn_to_VisualizeCM;
    [SerializeField] private GameObject go_head_content;

    [SerializeField] private UI_Simulation_ContextMenu visualize;
    [SerializeField] private GameObject go_visualize_content;

    //Time
    [SerializeField] private Slider sdr_TicksPerSecond;
    [SerializeField] private Slider sdr_TicksToTime;

    [SerializeField] private TMP_Text display_Day;
    [SerializeField] private TMP_Text display_Time;
    [SerializeField] private TMP_Text display_TicksToTime;
    [SerializeField] private TMP_Text display_TicksPerSecond;


    private void Awake()
    {
        im = GetComponent<InputManager>();
        btn_Pause.onClick.AddListener(im.pauseGame);
        btn_Home.onClick.AddListener(im.toMainMenu);
        initializeTicksPerSecondSlider();
        initializeTicksToTimeSlider();
        initializeContextMenu();

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
            displayTicksToTime(Gamevariables.MINUTES_PER_TICK);
            sdr_TicksToTime.value = Gamevariables.MINUTES_PER_TICK;
        }

        void initializeContextMenu()
        {
            head = new UI_Simulation_ContextMenu(go_head_content);
            visualize = new UI_Simulation_ContextMenu(go_visualize_content);

            head.setNext(btn_to_VisualizeCM, visualize);
            visualize.setPrevious(btn_to_HomeCm, head);
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
        btn_Pause.transform.GetChild(0).GetComponent<TMP_Text>().text = txt;
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
