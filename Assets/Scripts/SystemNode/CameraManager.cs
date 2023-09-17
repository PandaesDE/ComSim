using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private new Camera camera;
    private readonly int ZOOM_MAX = 100;
    private readonly int ZOOM_MIN = 10;
    private readonly int ZOOM_FACTOR = 5;

    /*
     * Ideas:
     * - Camera Follow Entity functionality
     * - zoom to where mouse is
     */



    private void Awake()
    {
        camera = Camera.main;
    }

    public void moveBy(Vector2 vect)
    {
        bool lookedBounds = checkAndLockBounds(vect);
        
        if(!lookedBounds) 
            camera.transform.position += new Vector3(vect.x, vect.y, 0);
    }

    public void zoom()
    {
        float zoom = camera.orthographicSize - Input.mouseScrollDelta.y * ZOOM_FACTOR;
        camera.orthographicSize = Mathf.Clamp(zoom, ZOOM_MIN, ZOOM_MAX);
        checkAndLockBounds(Vector2.zero);
        //camera.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10); //wonky

    }

    private bool checkAndLockBounds(Vector2 vect)
    {
        bool setBack = false;
        //Lock to Horizontal bounds
        if (camera.transform.position.x + vect.x + getWidth() / 2 > (float)Gamevariables.playgroundSize.x / 2)
        {
            setBack = true;
            camera.transform.position = new Vector3((float)Gamevariables.playgroundSize.x / 2 - getWidth() / 2, camera.transform.position.y, camera.transform.position.z);
        }
        else if (camera.transform.position.x + vect.x - getWidth() / 2 < -(float)Gamevariables.playgroundSize.x / 2)
        {
            setBack = true;
            camera.transform.position = new Vector3(-(float)Gamevariables.playgroundSize.x / 2 + getWidth() / 2, camera.transform.position.y, camera.transform.position.z);
        }

        //Lock to vertical bounds
        if (camera.transform.position.y + vect.y + getHeight() / 2 > (float)Gamevariables.playgroundSize.y / 2)
        {
            setBack = true;
            camera.transform.position = new Vector3(camera.transform.position.x, (float)Gamevariables.playgroundSize.y / 2 - getHeight() / 2, camera.transform.position.z);
        }
        else if (camera.transform.position.y + vect.y - getHeight() / 2 < -(float)Gamevariables.playgroundSize.y / 2)
        {
            setBack = true;
            camera.transform.position = new Vector3(camera.transform.position.x, -(float)Gamevariables.playgroundSize.y / 2 + getHeight() / 2, camera.transform.position.z);
        }
        return setBack;
    }

    //https://discussions.unity.com/t/find-width-and-height-of-world-space-in-2d/218103/2
    private float getHeight()
    {
        return camera.orthographicSize * 2;
    }

    private float getWidth()
    {
        return getHeight() * camera.aspect;
    }
}
