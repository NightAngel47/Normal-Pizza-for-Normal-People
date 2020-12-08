using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> loc = new List<GameObject>();
    public int timeMin;
    public int timeMax;

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
        int time = Random.Range(timeMin, timeMax);
        int carNum = Random.Range(0, cars.Count);
        int locNum = Random.Range(0, loc.Count);

        yield return new WaitForSeconds(time);
        GameObject g = Instantiate(cars[carNum], loc[locNum].transform.position, Quaternion.identity);
        if (locNum == 0 || locNum == 2)
        {
            g.GetComponent<CarBehaviour>().isGoingLeft = true;
            g.transform.Rotate(0, -90, 0);
        }

        if (locNum == 1 || locNum == 3)
        {
            g.GetComponent<CarBehaviour>().isGoingLeft = false;
            g.transform.Rotate(0, 90, 0);
        }

        spawnCar = true;
    }
}
