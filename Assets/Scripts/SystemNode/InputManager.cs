using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    UI ui;

    private void Awake()
    {
        ui = GetComponent<UI>();
    }


    #region initialize input Elements
    /*  UI Elements are all within the UI.cs File
     *  The functionality is handled in this File
     */

    public void initializePauseButton(Button btn)
    {
        btn.onClick.AddListener(pauseGame);
    }

    public void initializeHomeButton(Button btn)
    {
        btn.onClick.AddListener(toMainMenu);
    }

    public void initializeTicksPerSecondSlider(Slider sdr)
    {
        sdr.onValueChanged.AddListener(changeTicksPerSecond);
        float tps = 1 / Time.fixedDeltaTime;
        ui.displayTicksPerSeconds(tps);
        sdr.value = tps;
    }

    public void initializeTicksToTimeSlider(Slider sdr)
    {
        sdr.onValueChanged.AddListener(changeTicksToTime);
        int ttt = (int)((float)Gamevariables.MINUTES_PER_HOUR / (float)Gamevariables.TICKS_PER_HOUR);
        ui.displayTicksToTime(ttt);
        sdr.value = ttt;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        #region Menu Game Inputs which are accessible during pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseGame();
        }
        //on scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            //zoom
            float zoom = Camera.main.orthographicSize - Input.mouseScrollDelta.y;
            zoom = Mathf.Clamp(zoom, 10, 75);
            Camera.main.orthographicSize = zoom;
            //IDEA: zoom to where mouse is
            //Camera.main.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10); //wonky
        }
        #endregion
        #region In Game Inputs which are unavailable during pause
        if (Gamevariables.GAME_PAUSED) return;
        
        #endregion
    }

    private void changeTicksPerSecond(float val)
    {
        Time.fixedDeltaTime = val;
        ui.displayTicksPerSeconds(1 / val);
    }

    private void changeTicksToTime(float val)
    {
        val *= 5;
        Gamevariables.TICKS_PER_HOUR = (int)(Gamevariables.MINUTES_PER_HOUR / val);
        ui.displayTicksToTime((int)val);
    }

    private void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void pauseGame()
    {
        Gamevariables.GAME_PAUSED = !Gamevariables.GAME_PAUSED;
        if (Gamevariables.GAME_PAUSED)
        {
            Time.timeScale = 0;
            ui.displayPauseButtonText("R");
        } else
        {
            Time.timeScale = 1;
            ui.displayPauseButtonText("P");
        }
    }
}
