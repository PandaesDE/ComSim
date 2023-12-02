/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Creates Sprite to visualize generated World
 *  
 *  References:
 *      Scene: 
 *          - This script is only for the EditorMapGeneration Scene
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

public class EditorMapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    private int _cellsHorizontal;
    private int _cellsVertical;

    private Texture2D _noiseTex;

    private NoiseTextureGenerator _ntg;
    private SpriteRenderer _rend;


    private void Awake()
    {
        _cellsHorizontal = Screen.width;
        _cellsVertical = Screen.height;

        _noiseTex = new Texture2D(_cellsHorizontal, _cellsVertical);

        _rend = GetComponent<SpriteRenderer>();
        _ntg = GetComponent<NoiseTextureGenerator>();

        _rend.sprite = Sprite.Create(_noiseTex, new Rect(0, 0, _cellsHorizontal, _cellsVertical), new Vector2(0.5f, 0.5f));
    }

    private void Start()
    {
        RenderTexture();
    }

    public void RenderTexture()
    {
        _ntg.CalcNoise(_noiseTex, _cellsHorizontal, _cellsVertical);
    }


}
