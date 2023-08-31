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

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void pauseGame()
    {
        Gamevariables.GAME_PAUSED = !Gamevariables.GAME_PAUSED;
        if (Gamevariables.GAME_PAUSED)
        {
            Time.timeScale = 0;
            ui.displayPauseButtonText("R");
        }
        else
        {
            Time.timeScale = 1;
            ui.displayPauseButtonText("P");
        }
    }

    public void changeTicksPerSecond(float val)
    {
        Time.fixedDeltaTime = val;
        ui.displayTicksPerSeconds(1 / val);
    }

    /* val is between 0 and 9*/
    public void changeTicksToTime(float index)
    {
        int[] values = { 1 , 5, 10, 15, 30, 45, 60, 90, 120, 240};
        if (index < 0) index = 0;
        if (index > values.Length-1) index = values.Length-1;

        Gamevariables.MINUTES_PER_TICK = values[(int)index];
        ui.displayTicksToTime(values[(int)index]);
    }
}
