using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> loc = new List<GameObject>();

    private bool spawnCar = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCar == true)
        {
            StartCoroutine(Car());
        }
    }

    public IEnumerator Car()
    {
        spawnCar = false;
        int time = Random.Range(10, 25);
        int carNum = Random.Range(0, cars.Count);
        int locNum = Random.Range(0, 2);

        yield return new WaitForSeconds(time);
        GameObject g = Instantiate(cars[carNum], loc[locNum].transform.position, Quaternion.identity);
        if (locNum == 0)
        {
            g.GetComponent<CarBehaviour>().direction = true;
            g.transform.Rotate(0, -90, 0);
        }

        if (locNum == 1)
        {
            g.GetComponent<CarBehaviour>().direction = false;
            g.transform.Rotate(0, 90, 0);
        }

        spawnCar = true;
    }
}
