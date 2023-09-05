using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    private Creature target;

    [SerializeField] private TMP_Text display_Name;

    [SerializeField] private Slider sdr_Health;
    [SerializeField] private Slider sdr_Hunger;
    [SerializeField] private Slider sdr_Thirst;

    [SerializeField] private Button btn_close;

    private void Awake()
    {
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
        updateHealthBar(target.health);
        updateHungerBar(target.hunger);
        updateThirstBar(target.thirst);
    }

    private void updateHealthBar(int h)
    {
        sdr_Health.value = (float)h / (float)target.MAX_HEALTH;
    }

    private void updateHungerBar(float h)
    {
        sdr_Hunger.value = h / Creature.MAX_HUNGER;
    }

    private void updateThirstBar(float t)
    {
        sdr_Thirst.value = t / Creature.MAX_THIRST;
    }

    private void setActive(bool state) 
    {
        gameObject.SetActive(state);

    }
}
