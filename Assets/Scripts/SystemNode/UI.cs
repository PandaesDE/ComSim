using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text display_Day;
    [SerializeField] private TMP_Text display_Time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayDay(int day)
    {
        display_Day.text = "Day: " + day;
    }

    public void displayTime(string formattedTime)
    {
        display_Time.text = formattedTime;
    }
}
