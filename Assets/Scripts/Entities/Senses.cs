using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{
    //https://www.youtube.com/watch?v=xp37Hz1t1Q8
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger-Enter:" + collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger-Exit:" + collision.name);
    }
}
