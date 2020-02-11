using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : MonoBehaviour
{
    public GameObject topSpawn;
    public GameObject currentTop;

    public static bool pickedUp = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        //Debug.Log("col");
    }

    private void OnTriggerExit(Collider col)
    {
        if (pickedUp == false)
        {
            pickedUp = true;
            
            StartCoroutine(nameof(WaitSpawn));
        }
    }

    IEnumerator WaitSpawn()
    {
        yield return new WaitForSeconds(.15f);
        GameObject instance = Instantiate(topSpawn, gameObject.transform.position, Quaternion.identity);
        currentTop = instance;
        
        pickedUp = false;
    }
}
