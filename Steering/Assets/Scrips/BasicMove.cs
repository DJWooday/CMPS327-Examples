using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    float x, z;

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        Vector3 movement = Vector3.forward * z * speed + Vector3.right * x * speed;
        rb.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
        Debug.DrawRay(transform.position, rb.velocity);
    }
}
