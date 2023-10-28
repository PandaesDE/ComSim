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

using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private Scene curScene;

    [SerializeField] private UI_Simulation_Navigation ui;
    [SerializeField] private GameObject infoMenuObject;
    private UI_Simulation_Popup_Information infoMenu;
    private CameraManager cameraManager;



    private void Awake()
    {
        curScene = SceneManager.GetActiveScene();

        if (curScene.name == "Simulation")
            infoMenu = infoMenuObject.GetComponent<UI_Simulation_Popup_Information>();
        cameraManager = GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        handleGeneralInputs();

        if (curScene.name == "Simulation")
        {
            handleSimulationInputs();
            return;
        }
        if (curScene.name == "EditorMapGeneration")
        {
            handleMapEditorInputs();
            return;
        }
    }

    private void handleGeneralInputs()
    {
        //Middle Click Hold
        if (Input.GetMouseButton(2))
        {
            //https://discussions.unity.com/t/how-to-detect-mouse-movement-as-an-input/22062/4
            if (Input.GetAxis("Mouse X") != 0)
            {
                cameraManager.moveHorizontalBy(Input.GetAxis("Mouse X"));
            }
            if (Input.GetAxis("Mouse Y") != 0)
            {
                cameraManager.moveVerticalBy(Input.GetAxis("Mouse Y"));
            }
        }

        //on scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraManager.zoom();
        }
    }

    private void handleSimulationInputs()
    {
        //Left Click
        if (Input.GetMouseButtonDown(0))
        {

            int allLayers = ~0;
            int visonLayers = LayerMask.NameToLayer("Vision");
            int layerMask = allLayers & ~(1 << visonLayers); //Chat-GPT

            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray, Vector2.zero, Mathf.Infinity, layerMask);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.GetComponent<Creature>() != null)
                {
                    infoMenu.setTarget(hits[i].transform.GetComponent<Creature>());
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseGame();
        }
    }

    private void handleMapEditorInputs()
    {

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
