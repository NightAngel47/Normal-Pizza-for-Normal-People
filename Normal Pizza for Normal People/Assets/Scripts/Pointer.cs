using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5;
    public GameObject dot;
    public VRInputModule im;

    private LineRenderer lr = null; 

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //use default of distance 
        PointerEventData data = im.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

        //raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //default 
        Vector3 endPos = transform.position + (transform.forward * targetLength);

        //or based on hit
        if(hit.collider !=null)
        {
            endPos = hit.point;
        }

        //set pos of dot
        dot.transform.position = endPos;

        //set line renderer
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, endPos);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
