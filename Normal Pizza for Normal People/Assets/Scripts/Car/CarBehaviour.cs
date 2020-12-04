using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private float speed = 10;
    public bool direction = true; //true left false right

    void Start()
    {
        speed = Random.Range(7, 13);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (direction == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 20, transform.position.y, transform.position.z), step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 20, transform.position.y, transform.position.z), step);
        }

        Destroy(gameObject, 5);
    }
}
