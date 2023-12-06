/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - defines all UI and handles it within the top Navigation during a simulation
 *  
 *  References:
 *      Scene:
 *          - simulation navigation(s)
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Simulation_Navigation : MonoBehaviour
{

    private InputManager im;
    UI_Simulation_Popup_Information ui_spi;

    //Functionality
    [SerializeField] private Button _btn_Pause;
    [SerializeField] private Button _btn_Home;

    //Context Menu
    [SerializeField] private Button _btn_to_HomeCm;

        //Head
    [SerializeField] private ContextMenu _head;
    [SerializeField] private Button _btn_to_VisualizeCM;
    [SerializeField] private Button _btn_to_EntitiesCM;
    [SerializeField] private Button _btn_to_StatisticsCM;
    [SerializeField] private GameObject _go_head_content;

        //Visualization
    [SerializeField] private ContextMenu _visualize;
    [SerializeField] private GameObject _go_visualize_content;

    [SerializeField] private Toggle _tgl_Trails;
    [SerializeField] private Slider _sdr_Trail_Length;
    [SerializeField] private TMP_Text _display_Trail_Length;
    [SerializeField] private TMP_Dropdown _drd_Trail_Color;
        //Entities
    [SerializeField] private ContextMenu _entities;
    [SerializeField] private GameObject _go_entities_content;

    [SerializeField] private TMP_InputField _ipt_Human_Adder;
    [SerializeField] private TMP_InputField _ipt_Lion_Adder;
    [SerializeField] private TMP_InputField _ipt_Boar_Adder;
    [SerializeField] private TMP_InputField _ipt_Rabbit_Adder;

    private bool _creatures_normalized = false;
    [SerializeField] private Button _btn_Creature_Normalizer;
    [SerializeField] private Button _btn_All_Adder;
    [SerializeField] private Button _btn_All_Remover;
    [SerializeField] private Button _btn_Human_Adder;
    [SerializeField] private Button _btn_Lion_Adder;
    [SerializeField] private Button _btn_Boar_Adder;
    [SerializeField] private Button _btn_Rabbit_Adder;

    //Statistics
    [SerializeField] private ContextMenu _statistics;
    [SerializeField] private GameObject _go_statistics_content;

    private class StatisticDisplayBatch
    {
        public TMP_Text Males;
        public TMP_Text Females;
        public TMP_Text DeathsBy;
    }

    [SerializeField] private TMP_Text _display_Human_Males;
    [SerializeField] private TMP_Text _display_Human_Females;
    [SerializeField] private TMP_Text _display_Human_DeathsBy;

    [SerializeField] private TMP_Text _display_Lion_Males;
    [SerializeField] private TMP_Text _display_Lion_Females;
    [SerializeField] private TMP_Text _display_Lion_DeathsBy;

    [SerializeField] private TMP_Text _display_Boar_Males;
    [SerializeField] private TMP_Text _display_Boar_Females;
    [SerializeField] private TMP_Text _display_Boar_DeathsBy;

    [SerializeField] private TMP_Text _display_Rabbit_Males;
    [SerializeField] private TMP_Text _display_Rabbit_Females;
    [SerializeField] private TMP_Text _display_Rabbit_DeathsBy;

    //Time
    [SerializeField] private Slider _sdr_TicksPerSecond;
    [SerializeField] private Slider _sdr_TicksToTime;

    [SerializeField] private TMP_Text _display_Day;
    [SerializeField] private TMP_Text _display_Time;
    [SerializeField] private TMP_Text _display_TicksToTime;
    [SerializeField] private TMP_Text _display_TicksPerSecond;


    private void Awake()
    {
        im = GetComponent<InputManager>();
        ui_spi = GameObject.Find("InfoMenu").GetComponent<UI_Simulation_Popup_Information>();

        _btn_Pause.onClick.AddListener(delegate
        {
            GameManager.PauseGame();
            if (Gamevariables.GamePaused)
            {
                DisplayPauseButtonText("R");
            }
            else
            {
                DisplayPauseButtonText("P");
            }
        });
        _btn_Home.onClick.AddListener(delegate {
            im.ToMainMenu();
        });
        InitializeTicksPerSecondSlider();
        InitializeTicksToTimeSlider();
        InitializeContextMenus();

        void InitializeTicksPerSecondSlider()
        {
            _sdr_TicksPerSecond.onValueChanged.AddListener( delegate {
                float tps = _sdr_TicksPerSecond.value;
                im.ChangeTicksPerSecond(tps);
                _display_TicksPerSecond.text = $"{(1 / tps)} Ticks/Second";
            });
            //initial value
            _sdr_TicksPerSecond.value = 1 / Time.fixedDeltaTime;
        }

        void InitializeTicksToTimeSlider()
        {
            _sdr_TicksToTime.onValueChanged.AddListener( delegate {
                float ttt = _sdr_TicksToTime.value;
                im.ChangeTicksToTime(ttt);
                DisplayTicksToTime(Gamevariables.MinutesPerTick);
            });

            //initial value (1) -> 5min/Tick
            _sdr_TicksToTime.value = 1;
        }

        void InitializeContextMenus()
        {
            InitializeContextMenuNavigation();
            InitializeVisualizationContextMenu();
            InitializeEntitiesContextMenu();
            InitializeStatisticsContextMenu();

            void InitializeContextMenuNavigation()
            {
                _head = new ContextMenu(_go_head_content);
                _visualize = new ContextMenu(_go_visualize_content);
                _entities = new ContextMenu(_go_entities_content);
                _statistics = new ContextMenu(_go_statistics_content);

                _head.SetNext(_btn_to_VisualizeCM, _visualize);
                _head.SetNext(_btn_to_EntitiesCM, _entities);
                _head.SetNext(_btn_to_StatisticsCM, _statistics);

                _visualize.SetPrevious(_btn_to_HomeCm, _head);
                _entities.SetPrevious(_btn_to_HomeCm, _head);
                _statistics.SetPrevious(_btn_to_HomeCm, _head);
            }

            void InitializeVisualizationContextMenu()
            {
                _tgl_Trails.onValueChanged.AddListener(delegate
                {
                    Gamevariables.ShowTrail = _tgl_Trails.isOn;
                });
                _tgl_Trails.isOn = Gamevariables.ShowTrail;

                _sdr_Trail_Length.onValueChanged.AddListener(delegate
                {
                    Gamevariables.TrailLength = (int)_sdr_Trail_Length.value;
                    _display_Trail_Length.text = $"Length = {Gamevariables.TrailLength}";
                });
                _sdr_Trail_Length.value = Gamevariables.TrailLength;

                _drd_Trail_Color.onValueChanged.AddListener(delegate
                {
                    string val = _drd_Trail_Color.options[_drd_Trail_Color.value].text;

                    if (val.Equals("Dietary"))
                    {
                        Gamevariables.TrailColor = Trail.ColorScheme.dietary;
                    } else
                    {
                        Gamevariables.TrailColor = Trail.ColorScheme.@default;
                    }

                    ObjectManager.ChangeTrailColor();
                });
            }

            void InitializeEntitiesContextMenu()
            {
                _btn_Creature_Normalizer.onClick.AddListener(delegate
                {
                    _creatures_normalized = !_creatures_normalized;
                    if (_creatures_normalized ) { _btn_Creature_Normalizer.GetComponentInChildren<TMP_Text>().text = "Unequal Values"; }
                    else { _btn_Creature_Normalizer.GetComponentInChildren<TMP_Text>().text = "Equal Values"; }
                    ObjectManager.SetCreatureAttributes(_creatures_normalized);
                    ui_spi.FixedUpdate();
                });

                _btn_All_Remover.onClick.AddListener(delegate
                {
                    ObjectManager.DeleteAllCreatures();
                    ObjectManager.DeleteAllCorpses();
                });

                _btn_All_Adder.onClick.AddListener(delegate
                {
                    SpawnHumans();
                    SpawnLions();
                    SpawnBoars();
                    SpawnRabbits();
                });

                _btn_Human_Adder.onClick.AddListener(delegate
                {
                    SpawnHumans();
                });

                _btn_Lion_Adder.onClick.AddListener(delegate
                {
                    SpawnLions();
                });

                _btn_Boar_Adder.onClick.AddListener(delegate
                {
                    SpawnBoars();
                });

                _btn_Rabbit_Adder.onClick.AddListener(delegate
                {
                    SpawnRabbits();
                });

                void SpawnHumans()
                {
                    int amount = int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Human_Adder.text));
                    Spawner.SpawnHumans(new SpawnOptions(amount, true, true));
                }

                void SpawnLions()
                {
                    int amount = int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Lion_Adder.text));
                    Spawner.SpawnLions(new SpawnOptions(amount, true, true));
                }

                void SpawnBoars()
                {
                    int amount = int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Boar_Adder.text));
                    Spawner.SpawnBoars(new SpawnOptions(amount, true, true));
                }

                void SpawnRabbits()
                {
                    int amount = int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Rabbit_Adder.text));
                    Spawner.SpawnRabbits(new SpawnOptions(amount, true, true));
                }
            }

            void InitializeStatisticsContextMenu()
            {
                //subscribe event
                GetComponent<Statistics>().StatisticUpdateEvent += UpdateStatistics;

                void UpdateStatistics() {
                    UpdateSpecies(new StatisticDisplayBatch()
                    {
                        Males = _display_Human_Males,
                        Females = _display_Human_Females,
                        DeathsBy = _display_Human_DeathsBy
                    }, Statistics.HumanCounts, Statistics.HumanDeathReasons);

                    UpdateSpecies(new StatisticDisplayBatch()
                    {
                        Males = _display_Lion_Males,
                        Females = _display_Lion_Females,
                        DeathsBy = _display_Lion_DeathsBy
                    }, Statistics.LionCounts, Statistics.LionDeathReasons);

                    UpdateSpecies(new StatisticDisplayBatch()
                    {
                        Males = _display_Boar_Males,
                        Females = _display_Boar_Females,
                        DeathsBy = _display_Boar_DeathsBy
                    }, Statistics.BoarCounts, Statistics.BoarDeathReasons);

                    UpdateSpecies(new StatisticDisplayBatch()
                    {
                        Males = _display_Rabbit_Males,
                        Females = _display_Rabbit_Females,
                        DeathsBy = _display_Rabbit_DeathsBy
                    }, Statistics.RabbitCounts, Statistics.RabbitDeathReasons);
                }

                void UpdateSpecies(StatisticDisplayBatch batch, List<Statistics.CountData> cd, Dictionary<Creature.DeathReason, int> dr)
                {
                    string deathsBy = "";
                    if (dr.Count > 0)
                        deathsBy = $"{dr.OrderBy(x => x.Value).First()}";
                    batch.Males.text = $"({cd[^1].Males}/{cd[^1].Count})";
                    batch.Females.text = $"({cd[^1].Females}/{cd[^1].Count})";
                    batch.DeathsBy.text = deathsBy;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        DisplayDay(1 + Gamevariables.MinutesPassed / (Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY));
        DisplayTime(FormatTime());
    }

    private void DisplayTicksToTime(int min)
    {
        string time = min + " Minutes";
        string exp = "";
        if (min == 1) time = min + " Minute";
        if (min >= Gamevariables.MINUTES_PER_HOUR) time = min / Gamevariables.MINUTES_PER_HOUR + " Hour";
        if (min > 5)
        {
            _display_TicksToTime.color = Color.red;
            exp = "(experimental)";
        }
        else _display_TicksToTime.color= Color.white;
        _display_TicksToTime.text = $"{time} = 1 Tick {exp}";
    }

    private void DisplayPauseButtonText(string txt)
    {
        _btn_Pause.transform.GetChild(0).GetComponent<TMP_Text>().text = txt;
    }


    #region Day & Night Cycle
    private void DisplayDay(int day)
    {
        _display_Day.text = "Day: " + day;
    }

    private void DisplayTime(string formattedTime)
    {
        _display_Time.text = formattedTime;
    }

    private string FormatTime()
    {
        int minutes_passed_today = Gamevariables.MinutesPassed % (Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);

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
