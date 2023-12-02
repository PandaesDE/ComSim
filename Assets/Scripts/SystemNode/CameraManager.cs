/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Enables user camera control functionality
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

using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;

    private GameObject _target;

    private readonly int ZOOM_MAX = 100;
    private readonly int ZOOM_MIN = 10;
    private readonly int ZOOM_FACTOR = 5;

    [SerializeField] private Nullable<float> _playgroundWidth;
    [SerializeField] private Nullable<float> _playgroundHeight;

    private void Awake()
    {
        _camera = Camera.main;
        if (_playgroundWidth == null)
            _playgroundWidth = (float)Gamevariables.PLAYGROUND_SIZE.x;
        if (_playgroundHeight == null)
            _playgroundHeight = (float)Gamevariables.PLAYGROUND_SIZE.y;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 vect = new Vector2(_target.transform.position.x, _target.transform.position.y) - new Vector2(_camera.transform.position.x, _camera.transform.position.y);
            MoveVerticalBy(vect.y);
            MoveHorizontalBy(vect.x);
        }
    }

    public void MoveHorizontalBy(float x)
    {
        if (!IsOutOfHorizontalBound(x))
            _camera.transform.position += new Vector3(x, 0, 0);
    }

    public void MoveVerticalBy(float y)
    {
        if (!IsOutOfVerticalBound(y))
            _camera.transform.position += new Vector3(0, y, 0);
    }

    public void MoveBy(Vector2 vect)
    {
        bool lookedBounds = IsOutOfBound(vect);
        
        if(!lookedBounds) 
            _camera.transform.position += new Vector3(vect.x, vect.y, 0);
    }

    public bool CanMoveHorizontalBy(float xMove)
    {
        if (IsOutOfHorizontalBound(xMove)) 
            return false;
        return true;
    }

    public bool CanMoveVerticalBy(float yMove)
    {
        if (IsOutOfVerticalBound(yMove))
            return false;
        return true;
    }

    public void Zoom()
    {
        float zoom = _camera.orthographicSize - Input.mouseScrollDelta.y * ZOOM_FACTOR;
        _camera.orthographicSize = Mathf.Clamp(zoom, ZOOM_MIN, ZOOM_MAX);
        IsOutOfBound(Vector2.zero);
        //camera.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10); //wonky
    }

    public void SetPlaygroundWidth(float width)
    {
        _playgroundWidth = width;
    }

    public void SetPlaygroundHeight(float height)
    {
        _playgroundHeight = height;
    }

    

    private bool IsOutOfBound(Vector2 moveVect)
    {
        if (IsOutOfVerticalBound(moveVect.y))
            return true;
        if (IsOutOfHorizontalBound(moveVect.x))
            return true;
        return false;
    }

    private bool IsOutOfHorizontalBound(float moveX)
    {
        float playgroundHalfWidth = _playgroundWidth.Value / 2;

        //Lock to Horizontal bounds
        if (_camera.transform.position.x + moveX + GetWidth() / 2 > playgroundHalfWidth)
        {
            _camera.transform.position = new Vector3(playgroundHalfWidth - GetWidth() / 2, _camera.transform.position.y, _camera.transform.position.z);
            return true;
        }
        
        if (_camera.transform.position.x + moveX - GetWidth() / 2 < -playgroundHalfWidth)
        {
            _camera.transform.position = new Vector3(-playgroundHalfWidth + GetWidth() / 2, _camera.transform.position.y, _camera.transform.position.z);
            return true;
        }
        return false;
    }

    private bool IsOutOfVerticalBound(float moveY)
    {
        float playgroundHalfHeight = _playgroundHeight.Value / 2;

        if (_camera.transform.position.y + moveY + GetHeight() / 2 > playgroundHalfHeight)
        {
            _camera.transform.position = new Vector3(_camera.transform.position.x, playgroundHalfHeight - GetHeight() / 2, _camera.transform.position.z);
            return true;
        }
        
        if (_camera.transform.position.y + moveY - GetHeight() / 2 < -playgroundHalfHeight)
        {
            _camera.transform.position = new Vector3(_camera.transform.position.x, -playgroundHalfHeight + GetHeight() / 2, _camera.transform.position.z);
            return true;
        }
        return false;
    }

    public void FollowTarget(bool follow, GameObject target)
    {
        if (!follow)
        {
            this._target = null;
            return;
        }

        this._target = target;
        _camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, _camera.transform.position.z);
    }

    //https://discussions.unity.com/t/find-width-and-height-of-world-space-in-2d/218103/2
    private float GetHeight()
    {
        return _camera.orthographicSize * 2;
    }

    private float GetWidth()
    {
        return GetHeight() * _camera.aspect;
    }
}
