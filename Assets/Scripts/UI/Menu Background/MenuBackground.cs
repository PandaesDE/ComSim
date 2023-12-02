/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - creates background texture for menu scenes
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

public class MenuBackground : MonoBehaviour
{
    // Width and height of the texture in pixels.
    [SerializeField] private int _cellsHorizontal;
    [SerializeField] private int _cellsVertical;

    private Texture2D _noiseTex;

    private NoiseTextureGenerator _ntg;
    private SpriteRenderer _rend;


    private void Awake()
    {
        _cellsHorizontal = Gamevariables.PLAYGROUND_SIZE.x;
        _cellsVertical = Gamevariables.PLAYGROUND_SIZE.y;

        _noiseTex = new Texture2D(_cellsHorizontal, _cellsVertical);

        _rend = GetComponent<SpriteRenderer>();
        _ntg = GetComponent<NoiseTextureGenerator>();

        _rend.sprite = Sprite.Create(_noiseTex, new Rect(0, 0, _cellsHorizontal, _cellsVertical), new Vector2(0.5f, 0.5f));
    }

    private void Update()
    {
        _ntg.CalcNoise(_noiseTex, _cellsHorizontal, _cellsVertical);
    }
}
