using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float hoverSpeed = 1f;
    public float hoverHeight = 0.2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Floor"))
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}

}
