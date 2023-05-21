using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public GameObject trackObject;
    public Transform distanceFromObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(trackObject == null)
        {
            return;
        }
        transform.rotation = distanceFromObject.localRotation;
        transform.position = trackObject.transform.position + distanceFromObject.localPosition;
    }
}
