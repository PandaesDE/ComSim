using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private Vector2 target;
    [SerializeField] private float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        target = Util.getRandomValueInPlayground();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }
}
