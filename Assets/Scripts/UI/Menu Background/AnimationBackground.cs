/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - animates background
 *  
 *  References:
 *      Scene:
 *          - Main Menu
 *          - Settings Menu
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

public class AnimationBackground : MonoBehaviour
{
    // Width and height of the texture in pixels.
    private int _cellsHorizontal;
    private int _cellsVertical;
    private readonly int MAP_SCALER = 3;

    private Vector2 _speed = new(3f, 3f);

    private CameraManager _cameraManager;

    private Texture2D _noiseTex;

    private NoiseTextureGenerator _ntg;
    private SpriteRenderer _rend;


    private void Awake()
    {
        _cellsHorizontal = Screen.width * MAP_SCALER;
        _cellsVertical = Screen.height * MAP_SCALER;

        _cameraManager = GameObject.Find("SystemNode").GetComponent<CameraManager>();
        _cameraManager.SetPlaygroundWidth(_cellsHorizontal);
        _cameraManager.SetPlaygroundHeight(_cellsVertical);


        _noiseTex = new Texture2D(_cellsHorizontal, _cellsVertical);

        _rend = GetComponent<SpriteRenderer>();
        _ntg = GetComponent<NoiseTextureGenerator>();

        _rend.sprite = Sprite.Create(_noiseTex, new Rect(0, 0, _cellsHorizontal, _cellsVertical), new Vector2(0.5f, 0.5f));
        _ntg.CalcNoise(_noiseTex, _cellsHorizontal, _cellsVertical);

    }

    private void Update()
    {
        if (!_cameraManager.CanMoveHorizontalBy(_speed.x))
            _speed.x = -_speed.x;
        if (!_cameraManager.CanMoveVerticalBy(_speed.y))
            _speed.y = -_speed.y;
        _cameraManager.MoveBy(_speed * Time.deltaTime);
    }
}
