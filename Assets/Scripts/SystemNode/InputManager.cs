using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.mouseScrollDelta.y != 0) //zoom
        {
            Camera.main.orthographicSize += Input.mouseScrollDelta.y; // clamp to above 0
            Camera.main.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10); //wonky
        }
    }
}
