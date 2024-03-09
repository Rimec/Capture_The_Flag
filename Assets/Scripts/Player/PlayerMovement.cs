using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 300f;
    public int playerId = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if(moveDir != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        rb.velocity +=  moveDir * speed * Time.deltaTime;

        // float mov = Input.GetAxis("Vertical");
        // float rot = Input.GetAxis("Horizontal");
        // transform.Rotate(0, rot * rotationSpeed * Time.deltaTime, 0);
       
       

        // rb.velocity += transform.forward * mov * speed * Time.deltaTime;
    }
}
