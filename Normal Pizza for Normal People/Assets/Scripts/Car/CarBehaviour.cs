using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private float speed = 10;
    public bool isGoingLeft = true; //true left false right
    public bool isCar = false;

    void Start()
    {
        if (isCar == true)
        {
            speed = Random.Range(7, 13);
        }
        else
        {
            speed = Random.Range(1, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (isGoingLeft == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 20, transform.position.y, transform.position.z), step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 20, transform.position.y, transform.position.z), step);
        }

        if (isCar == true)
        {
            Destroy(gameObject, 5);
        }
        else
        {
            Destroy(gameObject, 15);
        }
    }
}
