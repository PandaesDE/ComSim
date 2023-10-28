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

public class UI_Simulation_Popup_Information : MonoBehaviour
{
    private Creature target;
    private CameraManager cameraManager;

    [SerializeField] private TMP_Text display_Name;
    [SerializeField] private TMP_Text display_Status;

    [SerializeField] private GameObject image_Object;
    private Image image;

    [SerializeField] private Slider sdr_Health;
    [SerializeField] private Slider sdr_Hunger;
    [SerializeField] private Slider sdr_Thirst;
    [SerializeField] private Slider sdr_Energy;

    [SerializeField] private Button btn_close;

    [SerializeField] private Toggle tgl_Follow;


    /*
     * Ideas:
     * - Follow Button (with cam)
     */

    private void Awake()
    {
        cameraManager = GameObject.Find("SystemNode").GetComponent<CameraManager>();

        image = image_Object.GetComponent<Image>();
        btn_close.onClick.AddListener(delegate { setActive(false); });
        tgl_Follow.onValueChanged.AddListener(delegate { followTarget(tgl_Follow.isOn); });
    }

    private void FixedUpdate()
    {
        updateChangingInfo();
    }

    public void setTarget(Creature t)
    {
        this.target = t;
        followTarget(tgl_Follow.isOn);
        setActive(true);
        updateAllInfo();
    }

    private void followTarget(bool follow)
    {
        if (target == null)
        {
            cameraManager.followTarget(follow, null);
            return;
        }
        cameraManager.followTarget(follow, target.gameObject);
    }

    private void updateAllInfo()
    {
        updateImage();
        updateChangingInfo();
    }

    private void updateChangingInfo()
    {
        if (target == null)
        {
            resetInputs();
            setActive(false);
            return;
        }

        display_Name.text = target.name;
        updateHealthBar();
        updateHungerBar();
        updateThirstBar();
        updateEnergyBar();
        updateStatus();
    }
    private void resetInputs()
    {
        tgl_Follow.isOn = false;
    }

    private void updateImage()
    {
        image.sprite = target.GetComponent<SpriteRenderer>().sprite;
    }
    private void updateHealthBar()
    {
        sdr_Health.value = (float)target.health / (float)target.MAX_HEALTH;
    }

    private void updateHungerBar()
    {
        sdr_Hunger.value = target.hunger / Creature.MAX_HUNGER;
    }

    private void updateThirstBar()
    {
        sdr_Thirst.value = target.thirst / Creature.MAX_THIRST;
    }

    private void updateEnergyBar()
    {
        sdr_Energy.value = target.energy / Creature.MAX_ENERGY;
    }

    private void updateStatus()
    {
        display_Status.text = (target.mission + "").ToLower();
    }

    private void setActive(bool state) 
    {
        gameObject.SetActive(state);
    }
}
