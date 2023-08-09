using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button btn_PAUSE;
    [SerializeField] Button btn_HOME;

    private void Awake()
    {
        btn_PAUSE.onClick.AddListener(pauseGame);
        btn_HOME.onClick.AddListener(toMainMenu);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region Menu Game Inputs which are accessible during pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseGame();
        }
        #endregion
        #region In Game Inputs which are unavailable during pause
        if (Gamevariables.GAME_PAUSED) return;
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
            btn_PAUSE.transform.GetChild(0).GetComponent<TMP_Text>().text = "R";
        } else
        {
            Time.timeScale = 1;
            btn_PAUSE.transform.GetChild(0).GetComponent<TMP_Text>().text = "P";
        }
    }
}
