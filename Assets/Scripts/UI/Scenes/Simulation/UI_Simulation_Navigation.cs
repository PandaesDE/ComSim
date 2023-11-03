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
    Spawner spawner;

    //Functionality
    [SerializeField] private Button btn_Pause;
    [SerializeField] private Button btn_Home;

    //Context Menu
    [SerializeField] private Button btn_to_HomeCm;

        //Head
    [SerializeField] private ContextMenu head;
    [SerializeField] private Button btn_to_VisualizeCM;
    [SerializeField] private Button btn_to_EntitiesCM;
    [SerializeField] private Button btn_to_StatisticsCM;
    [SerializeField] private GameObject go_head_content;

        //Visualization
    [SerializeField] private ContextMenu visualize;
    [SerializeField] private GameObject go_visualize_content;

    [SerializeField] private Toggle tgl_Trails;
    [SerializeField] private Slider sdr_Trail_Length;
    [SerializeField] private TMP_Text display_Trail_Length;
    [SerializeField] private TMP_Dropdown drd_Trail_Color;
        //Entities
    [SerializeField] private ContextMenu entities;
    [SerializeField] private GameObject go_entities_content;

    [SerializeField] private TMP_InputField ipt_Human_Adder;
    [SerializeField] private TMP_InputField ipt_Lion_Adder;
    [SerializeField] private TMP_InputField ipt_Boar_Adder;
    [SerializeField] private TMP_InputField ipt_Rabbit_Adder;

    [SerializeField] private Button btn_All_Adder;
    [SerializeField] private Button btn_All_Remover;
    [SerializeField] private Button btn_Human_Adder;
    [SerializeField] private Button btn_Lion_Adder;
    [SerializeField] private Button btn_Boar_Adder;
    [SerializeField] private Button btn_Rabbit_Adder;

    //Statistics
    [SerializeField] private ContextMenu statistics;
    [SerializeField] private GameObject go_statistics_content;

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
        spawner = GetComponent<Spawner>();

        btn_Pause.onClick.AddListener(delegate
        {
            im.pauseGame();
            if (Gamevariables.GAME_PAUSED)
            {
                displayPauseButtonText("R");
            }
            else
            {
                displayPauseButtonText("P");
            }
        });
        btn_Home.onClick.AddListener(delegate {
            im.toMainMenu();
        });
        initializeTicksPerSecondSlider();
        initializeTicksToTimeSlider();
        initializeContextMenus();

        void initializeTicksPerSecondSlider()
        {
            sdr_TicksPerSecond.onValueChanged.AddListener( delegate {
                float tps = sdr_TicksPerSecond.value;
                im.changeTicksPerSecond(tps);
                display_TicksPerSecond.text = $"{(1 / tps)} Ticks/Second";
            });
            sdr_TicksPerSecond.value = 1 / Time.fixedDeltaTime;
        }

        void initializeTicksToTimeSlider()
        {
            sdr_TicksToTime.onValueChanged.AddListener( delegate {
                float ttt = sdr_TicksToTime.value;
                im.changeTicksToTime(ttt);
                displayTicksToTime(Gamevariables.MINUTES_PER_TICK);
            });
            sdr_TicksToTime.value = Gamevariables.MINUTES_PER_TICK;
        }

        void initializeContextMenus()
        {
            initializeContextMenuNavigation();
            initializeVisualizationContextMenu();
            initializeEntitiesContextMenu();

            void initializeContextMenuNavigation()
            {
                head = new ContextMenu(go_head_content);
                visualize = new ContextMenu(go_visualize_content);
                entities = new ContextMenu(go_entities_content);
                statistics = new ContextMenu(go_statistics_content);

                head.setNext(btn_to_VisualizeCM, visualize);
                head.setNext(btn_to_EntitiesCM, entities);
                head.setNext(btn_to_StatisticsCM, statistics);

                visualize.setPrevious(btn_to_HomeCm, head);
                entities.setPrevious(btn_to_HomeCm, head);
                statistics.setPrevious(btn_to_HomeCm, head);
            }

            void initializeVisualizationContextMenu()
            {
                tgl_Trails.onValueChanged.AddListener(delegate
                {
                    Gamevariables.SHOW_TRAIL = tgl_Trails.isOn;
                });
                tgl_Trails.isOn = Gamevariables.SHOW_TRAIL;

                sdr_Trail_Length.onValueChanged.AddListener(delegate
                {
                    Gamevariables.TRAIL_LENGTH = (int)sdr_Trail_Length.value;
                    display_Trail_Length.text = $"Length = {Gamevariables.TRAIL_LENGTH}";
                });
                sdr_Trail_Length.value = Gamevariables.TRAIL_LENGTH;

                drd_Trail_Color.onValueChanged.AddListener(delegate
                {
                    string val = drd_Trail_Color.options[drd_Trail_Color.value].text;

                    if (val.Equals("Dietary"))
                    {
                        Gamevariables.TRAIL_COLOR = Trail.ColorScheme.DIETARY;
                    } else
                    {
                        Gamevariables.TRAIL_COLOR = Trail.ColorScheme.DEFAULT;
                    }

                    ObjectManager.changeTrailColor();
                });
            }

            void initializeEntitiesContextMenu()
            {
                btn_All_Remover.onClick.AddListener(delegate
                {
                    ObjectManager.deleteAllCreatures();
                });

                btn_All_Adder.onClick.AddListener(delegate
                {
                    spawnHumans();
                    spawnLions();
                    spawnBoars();
                    spawnRabbits();
                });

                btn_Human_Adder.onClick.AddListener(delegate
                {
                    spawnHumans();
                });

                btn_Lion_Adder.onClick.AddListener(delegate
                {
                    spawnLions();
                });

                btn_Boar_Adder.onClick.AddListener(delegate
                {
                    spawnBoars();
                });

                btn_Rabbit_Adder.onClick.AddListener(delegate
                {
                    spawnRabbits();
                });

                void spawnHumans()
                {
                    int amount = int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Human_Adder.text));
                    spawner.spawnHumans(amount);
                }

                void spawnLions()
                {
                    int amount = int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Lion_Adder.text));
                    spawner.spawnLions(amount);
                }

                void spawnBoars()
                {
                    int amount = int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Boar_Adder.text));
                    spawner.spawnBoars(amount);
                }

                void spawnRabbits()
                {
                    int amount = int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Rabbit_Adder.text));
                    spawner.spawnRabbits(amount);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        displayDay(1 + Gamevariables.MINUTES_PASSED / (Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY));
        displayTime(formatTime());
    }

    private void displayTicksToTime(int min)
    {
        string time = min + " Minutes";
        if (min == 1) time = min + " Minute";
        if (min >= 60) time = min / 60 + " Hour";
        display_TicksToTime.text = time + " = 1 Tick";
    }

    private void displayPauseButtonText(string txt)
    {
        btn_Pause.transform.GetChild(0).GetComponent<TMP_Text>().text = txt;
    }


    #region Day & Night Cycle
    private void displayDay(int day)
    {
        display_Day.text = "Day: " + day;
    }

    private void displayTime(string formattedTime)
    {
        display_Time.text = formattedTime;
    }

    private string formatTime()
    {
        int minutes_passed_today = Gamevariables.MINUTES_PASSED % (Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);

        int display_hours = minutes_passed_today / Gamevariables.MINUTES_PER_HOUR;
        float display_minutes = minutes_passed_today % Gamevariables.MINUTES_PER_HOUR;
        string hourPrefix = "";
        string minutesPrefix = "";


        if (display_hours < 10) hourPrefix = "0";
        if (display_minutes < 10) minutesPrefix = "0";

        return hourPrefix + display_hours + ":" + minutesPrefix + display_minutes;
    }
    #endregion
}
