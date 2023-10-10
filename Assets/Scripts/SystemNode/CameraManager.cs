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

using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private new Camera camera;

    private GameObject target;

    private readonly int ZOOM_MAX = 100;
    private readonly int ZOOM_MIN = 10;
    private readonly int ZOOM_FACTOR = 5;

    [SerializeField] private Nullable<float> playgroundWidth;
    [SerializeField] private Nullable<float> playgroundHeight;

    private void Awake()
    {
        camera = Camera.main;
        if (playgroundWidth == null)
            playgroundWidth = (float)Gamevariables.playgroundSize.x;
        if (playgroundHeight == null)
            playgroundHeight = (float)Gamevariables.playgroundSize.y;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 vect = new Vector2(target.transform.position.x, target.transform.position.y) - new Vector2(camera.transform.position.x, camera.transform.position.y);
            moveBy(vect);
        }
    }



    public void moveBy(Vector2 vect)
    {
        bool lookedBounds = isOutOfBound(vect);
        
        if(!lookedBounds) 
            camera.transform.position += new Vector3(vect.x, vect.y, 0);
    }

    public bool canMoveHorizontalBy(float xMove)
    {
        if (isOutOfHorizontalBound(xMove)) 
            return false;
        return true;
    }

    public bool canMoveVerticalBy(float yMove)
    {
        if (isOutOfVerticalBound(yMove))
            return false;
        return true;
    }

    public void zoom()
    {
        float zoom = camera.orthographicSize - Input.mouseScrollDelta.y * ZOOM_FACTOR;
        camera.orthographicSize = Mathf.Clamp(zoom, ZOOM_MIN, ZOOM_MAX);
        isOutOfBound(Vector2.zero);
        //camera.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10); //wonky
    }

    public void setPlaygroundWidth(float width)
    {
        playgroundWidth = width;
    }

    public void setPlaygroundHeight(float height)
    {
        playgroundHeight = height;
    }

    

    private bool isOutOfBound(Vector2 moveVect)
    {
        if (isOutOfVerticalBound(moveVect.y))
            return true;
        if (isOutOfHorizontalBound(moveVect.x))
            return true;
        return false;
    }

    private bool isOutOfHorizontalBound(float moveX)
    {
        float playgroundHalfWidth = playgroundWidth.Value / 2;

        //Lock to Horizontal bounds
        if (camera.transform.position.x + moveX + getWidth() / 2 > playgroundHalfWidth)
        {
            camera.transform.position = new Vector3(playgroundHalfWidth - getWidth() / 2, camera.transform.position.y, camera.transform.position.z);
            return true;
        }
        
        if (camera.transform.position.x + moveX - getWidth() / 2 < -playgroundHalfWidth)
        {
            camera.transform.position = new Vector3(-playgroundHalfWidth + getWidth() / 2, camera.transform.position.y, camera.transform.position.z);
            return true;
        }
        return false;
    }

    private bool isOutOfVerticalBound(float moveY)
    {
        float playgroundHalfHeight = playgroundHeight.Value / 2;

        if (camera.transform.position.y + moveY + getHeight() / 2 > playgroundHalfHeight)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, playgroundHalfHeight - getHeight() / 2, camera.transform.position.z);
            return true;
        }
        
        if (camera.transform.position.y + moveY - getHeight() / 2 < -playgroundHalfHeight)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, -playgroundHalfHeight + getHeight() / 2, camera.transform.position.z);
            return true;
        }
        return false;
    }

    public void followTarget(bool follow, GameObject target)
    {
        if (!follow)
        {
            this.target = null;
            return;
        }

        this.target = target;
        camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, camera.transform.position.z);
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
