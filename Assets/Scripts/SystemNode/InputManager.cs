/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - this class handles all the User Inputs, including:
 *          - Mouse
 *          - Keyboard
 *          - UI Inputs
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *          - EditorMapGeneration
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private Scene _curScene;

    [SerializeField] private GameObject _infoMenuObject;
    private UI_Simulation_Popup_Information _infoMenu;
    private CameraManager _cameraManager;



    private void Awake()
    {
        _curScene = SceneManager.GetActiveScene();

        if (_curScene.name == "Simulation")
            _infoMenu = _infoMenuObject.GetComponent<UI_Simulation_Popup_Information>();
        _cameraManager = GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGeneralInputs();

        if (_curScene.name == "Simulation")
        {
            HandleSimulationInputs();
            return;
        }
        if (_curScene.name == "EditorMapGeneration")
        {
            HandleMapEditorInputs();
            return;
        }
    }

    private void HandleGeneralInputs()
    {
        //Middle Click Hold
        if (Input.GetMouseButton(2))
        {
            //https://discussions.unity.com/t/how-to-detect-mouse-movement-as-an-input/22062/4
            if (Input.GetAxis("Mouse X") != 0)
            {
                _cameraManager.MoveHorizontalBy(Input.GetAxis("Mouse X"));
            }
            if (Input.GetAxis("Mouse Y") != 0)
            {
                _cameraManager.MoveVerticalBy(Input.GetAxis("Mouse Y"));
            }
        }

        //on scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            _cameraManager.Zoom(Input.mouseScrollDelta.y);
        }
    }

    private void HandleSimulationInputs()
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
                    _infoMenu.SetTarget(hits[i].transform.GetComponent<Creature>());
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    private void HandleMapEditorInputs()
    {
        //so far empty because there is no need
        //for structural puproses this is a placeholder
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Gamevariables.GamePaused = !Gamevariables.GamePaused;
        if (Gamevariables.GamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ChangeTicksPerSecond(float val)
    {
        Time.fixedDeltaTime = val;
    }

    /* val is between inclusive 0 and 9*/
    public void ChangeTicksToTime(float index)
    {
        int[] values = { 1 , 5, 10, 15, 30, 45, 60, 90, 120, 240};
        index = Mathf.Clamp(index, 0, values.Length - 1);

        Gamevariables.MinutesPerTick = values[(int)index];
    }
}
