/*
 * 
 * Comment: This script is only for the EditorMapGeneration Scene
 */

using UnityEngine;

public class EditorMapGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    [SerializeField] private int CELLS_HORIZONTAL;
    [SerializeField] private int CELLS_VERTICAL;

    private Texture2D noiseTex;

    private NoiseTextureGenerator ntg;
    private SpriteRenderer rend;


    private void Awake()
    {
        CELLS_HORIZONTAL = Screen.width;
        CELLS_VERTICAL = Screen.height;

        noiseTex = new Texture2D(CELLS_HORIZONTAL, CELLS_VERTICAL);

        rend = GetComponent<SpriteRenderer>();
        ntg = GetComponent<NoiseTextureGenerator>();

        rend.sprite = Sprite.Create(noiseTex, new Rect(0, 0, CELLS_HORIZONTAL, CELLS_VERTICAL), new Vector2(0.5f, 0.5f));
    }

    private void Update()
    {
        ntg.CalcNoise(noiseTex, CELLS_HORIZONTAL, CELLS_VERTICAL);
    }


}
