using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    private Creature target;

    [SerializeField] private TMP_Text display_Name;

    [SerializeField] private GameObject image_Object;
    private Image image;

    [SerializeField] private Slider sdr_Health;
    [SerializeField] private Slider sdr_Hunger;
    [SerializeField] private Slider sdr_Thirst;
    [SerializeField] private Slider sdr_Energy;

    [SerializeField] private Button btn_close;

    private void Awake()
    {
        image = image_Object.GetComponent<Image>();
        btn_close.onClick.AddListener(delegate { setActive(false); });
    }

    private void FixedUpdate()
    {
        updateInfo();
    }

    public void setTarget(Creature t)
    {
        this.target = t;
        setActive(true);
        updateInfo();
    }

    private void updateInfo()
    {
        if (target == null)
        {
            setActive(false);
            return;
        }

        display_Name.text = target.name;
        updateImage();
        updateHealthBar();
        updateHungerBar();
        updateThirstBar();
        updateEnergyBar();
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

    private void setActive(bool state) 
    {
        gameObject.SetActive(state);

    }
}
