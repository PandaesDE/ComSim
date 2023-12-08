/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - defines all UI and handles it within the Entity popup information window during a simulation
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Simulation_Popup_Information : MonoBehaviour
{
    private Creature _target;
    private CameraManager _cameraManager;

    [SerializeField] private TMP_Text _display_Name;
    [SerializeField] private TMP_Text _display_Status;
    [SerializeField] private TMP_Text _display_Position;
    [SerializeField] private TMP_Text _display_Target;
    [SerializeField] private TMP_Text _display_Age;
    [SerializeField] private TMP_Text _display_AgeStage;
    [SerializeField] private TMP_Text _display_Damage;
    [SerializeField] private TMP_Text _display_Children;

    [SerializeField] private Image _img_Species;
    [SerializeField] private Image _img_Gender;

    [SerializeField] private Sprite _spr_Male;
    [SerializeField] private Sprite _spr_Female;
    [SerializeField] private Sprite _spr_Pregnant;

    [SerializeField] private Slider _sdr_Desire;
    [SerializeField] private Slider _sdr_Health;
    [SerializeField] private TMP_Text _display_Health;
    [SerializeField] private Slider _sdr_Hunger;
    [SerializeField] private Slider _sdr_Thirst;
    [SerializeField] private Slider _sdr_Energy;

    [SerializeField] private Button _btn_close;

    [SerializeField] private Toggle _tgl_Follow;

    private bool _wasPregnant;

    private void Awake()
    {
        _cameraManager = GameObject.Find("SystemNode").GetComponent<CameraManager>();

        _btn_close.onClick.AddListener(delegate { SetActive(false); });
        _tgl_Follow.onValueChanged.AddListener(delegate { FollowTarget(_tgl_Follow.isOn); });
    }

    public void FixedUpdate()
    {
        if (_target == null)
        {
            /*Gets only called once since the class is in inactive state afterwards*/
            ResetInputs();
            SetActive(false);
            return;
        }

        UpdateChangingInfo(true);
    }

    public void SetTarget(Creature t)
    {
        this._target = t;
        FollowTarget(_tgl_Follow.isOn);
        SetActive(true);
        UpdateAllInfo();
    }

    private void FollowTarget(bool follow)
    {
        if (_target == null)
        {
            _cameraManager.FollowTarget(follow, null);
            return;
        }
        _cameraManager.FollowTarget(follow, _target.gameObject);
    }


    //called once upon initialization
    private void UpdateAllInfo()
    {
        _display_Name.text = _target.name;
        _wasPregnant = _target.Gender.IsPregnant;

        UpdateSpecies();
        UpdateChangingInfo(false);
    }

    //called every FixedUpdate
    private void UpdateChangingInfo(bool initialized)
    {
        //Details
        UpdateGender(initialized);
        UpdateAge();
        UpdateDamage();
        UpdateChildren();
        //Debug
        UpdatePosition();
        UpdateTarget();
        //Stats
        UpdateDesireBar(initialized);
        UpdateHealthBar();
        UpdateHungerBar();
        UpdateThirstBar();
        UpdateEnergyBar();
        UpdateStatus();
    }


    private void ResetInputs()
    {
        _tgl_Follow.isOn = false;
    }

    private void UpdateSpecies()
    {
        _img_Species.sprite = _target.GetComponent<SpriteRenderer>().sprite;
    }

    private void UpdateGender(bool initialized)
    {
        //do nothing if state didn't change
        if (_wasPregnant == _target.Gender.IsPregnant && !initialized)
            return;

        if (_target.Gender.IsMale)
        {
            _img_Gender.sprite = _spr_Male;
            return;
        } 

        if (_target.Gender.IsPregnant)
        {
            _img_Gender.sprite= _spr_Pregnant;
            _wasPregnant = true;
        } 
        else
        {
            _img_Gender.sprite = _spr_Female;
            _wasPregnant = false;
        }
    }

    private void UpdateAge()
    {
        string ageStage = "Adult";
        Color fertile = Color.green;
        
        if (_target.GrowthFactor < 1f)
        {
            ageStage = "Child";
            fertile = Color.red;
        }

        _display_Age.text = $"{(int)_target.Age}/{CutFloatString(_target.MaxAge+"", 4)}";
        _display_AgeStage.text = ageStage;
        _display_AgeStage.color = fertile;
    }

    private void UpdatePosition()
    {
        _display_Position.text = $"{(Vector2)_target.transform.position}";
    }

    private void UpdateTarget()
    {
        _display_Target.text = $"{_target.Movement.Target}";
    }

    private void UpdateDesireBar(bool initialize)
    {
        if (initialize)
        {
            if (!_target.Gender.IsMale)
            {
                _sdr_Desire.gameObject.SetActive(false);
            } 
            else
            {
                _sdr_Desire.gameObject.SetActive(true);
            }
        }
        if (!_target.Gender.IsMale) return;

        _sdr_Desire.value = _target.Gender.Desire / IGender.MAX_DESIRE;
    }

    private void UpdateChildren()
    {
        _display_Children.text = $"{_target.Gender.Children}";
    }

    private void UpdateDamage()
    {
        _display_Damage.text = CutFloatString($"{_target.Damage}", 4);
    }

    private void UpdateHealthBar()
    {
        _sdr_Health.value = _target.Health / _target.MaxHealth;
        _display_Health.text = CutFloatString($"{_target.MaxHealth}", 4);
    }

    private void UpdateHungerBar()
    {
        _sdr_Hunger.value = _target.hunger / Creature.MAX_HUNGER;
    }

    private void UpdateThirstBar()
    {
        _sdr_Thirst.value = _target.thirst / Creature.MAX_THIRST;
    }

    private void UpdateEnergyBar()
    {
        _sdr_Energy.value = _target.Energy / Creature.MAX_ENERGY;
    }

    private void UpdateStatus()
    {
        _display_Status.text = $"{_target.StatusManager.Status}";
    }

    private void SetActive(bool state) 
    {
        gameObject.SetActive(state);
    }

    private string CutFloatString(string fl, int maxLength)
    {
        if (fl.Length < maxLength) maxLength = fl.Length;
        return fl[..maxLength];
    }
}
