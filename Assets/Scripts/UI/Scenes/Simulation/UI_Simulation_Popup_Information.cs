/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
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

    [SerializeField] private GameObject _image_Object;
    private Image _image;

    [SerializeField] private Slider _sdr_Health;
    [SerializeField] private Slider _sdr_Hunger;
    [SerializeField] private Slider _sdr_Thirst;
    [SerializeField] private Slider _sdr_Energy;

    [SerializeField] private Button _btn_close;

    [SerializeField] private Toggle _tgl_Follow;

    private void Awake()
    {
        _cameraManager = GameObject.Find("SystemNode").GetComponent<CameraManager>();

        _image = _image_Object.GetComponent<Image>();
        _btn_close.onClick.AddListener(delegate { SetActive(false); });
        _tgl_Follow.onValueChanged.AddListener(delegate { FollowTarget(_tgl_Follow.isOn); });
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            /*Gets only called once since the class is in inactive state afterwards*/
            ResetInputs();
            SetActive(false);
            return;
        }

        UpdateChangingInfo();
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

    private void UpdateAllInfo()
    {
        UpdateImage();
        UpdateChangingInfo();
    }

    private void UpdateChangingInfo()
    {
        _display_Name.text = _target.name;
        //Debug
        UpdatePosition();
        UpdateTarget();
        //Stats
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

    private void UpdateImage()
    {
        _image.sprite = _target.GetComponent<SpriteRenderer>().sprite;
    }

    private void UpdatePosition()
    {
        _display_Position.text = "" + (Vector2)_target.transform.position;
    }

    private void UpdateTarget()
    {
        _display_Target.text = "" + _target.Movement.Target;
    }

    private void UpdateHealthBar()
    {
        _sdr_Health.value = (float)_target.Health / (float)_target.maxHealth;
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
        _display_Status.text = (_target.StatusManager.Status + "").ToLower();
    }

    private void SetActive(bool state) 
    {
        gameObject.SetActive(state);
    }
}
