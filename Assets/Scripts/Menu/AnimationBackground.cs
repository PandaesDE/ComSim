using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBackground : MonoBehaviour
{
    // Width and height of the texture in pixels.
    private int CELLS_HORIZONTAL;
    private int CELLS_VERTICAL;
    private readonly int MAP_SCALER = 3;

    private Vector2 speed = new Vector2(100f, 100f);

    private CameraManager cameraManager;

    private Texture2D noiseTex;

    private NoiseTextureGenerator ntg;
    private SpriteRenderer rend;


    private void Awake()
    {
        CELLS_HORIZONTAL = Screen.width * MAP_SCALER;
        CELLS_VERTICAL = Screen.height * MAP_SCALER;

        cameraManager = GameObject.Find("SystemNode").GetComponent<CameraManager>();
        cameraManager.setPlaygroundWidth(CELLS_HORIZONTAL);
        cameraManager.setPlaygroundHeight(CELLS_VERTICAL);


        noiseTex = new Texture2D(CELLS_HORIZONTAL, CELLS_VERTICAL);

        rend = GetComponent<SpriteRenderer>();
        ntg = GetComponent<NoiseTextureGenerator>();

        rend.sprite = Sprite.Create(noiseTex, new Rect(0, 0, CELLS_HORIZONTAL, CELLS_VERTICAL), new Vector2(0.5f, 0.5f));
        ntg.CalcNoise(noiseTex, CELLS_HORIZONTAL, CELLS_VERTICAL);

    }

    private void Update()
    {
        if (!cameraManager.canMoveHorizontalBy(speed.x))
            speed.x = -speed.x;
        if (!cameraManager.canMoveVerticalBy(speed.y))
            speed.y = -speed.y;
        cameraManager.moveBy(speed * Time.deltaTime);
    }
}
