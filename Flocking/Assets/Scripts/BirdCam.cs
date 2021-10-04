using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCam : MonoBehaviour
{
    public static Transform target;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, .1f);
    }
}
