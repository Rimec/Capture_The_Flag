using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FrogController : NetworkBehaviour {
    Rigidbody rb;

    float jump_t;
    float jumpMaxTime = 3.0f;

    float jumpForce = 5.0f;
    float rotationSpeed = 100.0f;
    float diagonalRay = 1.5f; 

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        if(IsServer){
            rb = GetComponent<Rigidbody>();
        }
    }

    public void Update(){
        Move();
    }

    void Move(){
        // Lança raios à frente e nas diagonais
        Ray rayFrente = new Ray(transform.position + transform.up * 0.5f, transform.forward);
        Ray rayDiagonalEsquerda = new Ray(transform.position + transform.up * 0.5f, (transform.forward + transform.right).normalized);
        Ray rayDiagonalDireita = new Ray(transform.position + transform.up * 0.5f, (transform.forward - transform.right).normalized);

        RaycastHit hitFrente, hitDiagonalEsquerda, hitDiagonalDireita;

        // Verifica se há uma parede à frente ou nas diagonais
        bool hitFrenteWall = Physics.Raycast(rayFrente, out hitFrente, 1f);
        bool hitDiagonalEsquerdaWall = Physics.Raycast(rayDiagonalEsquerda, out hitDiagonalEsquerda, diagonalRay);
        bool hitDiagonalDireitaWall = Physics.Raycast(rayDiagonalDireita, out hitDiagonalDireita, diagonalRay);
        Debug.DrawRay(transform.position + transform.up * 0.5f, (transform.forward - transform.right).normalized, Color.cyan);
        Debug.DrawRay(transform.position + transform.up * 0.5f, (transform.forward + transform.right).normalized, Color.white);
        Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward, Color.red);

        if(hitFrenteWall)
            RotateFrog(1f);
        else if(hitDiagonalEsquerdaWall)
            RotateFrog(-1f);
        else if(hitDiagonalDireitaWall)
            RotateFrog(1f);
        else{
            Jump(Time.deltaTime);
        }
    }

    void RotateFrog(float dir)
    {
        transform.Rotate(Vector3.up, dir * rotationSpeed * Time.deltaTime);
    }

    void Jump(float dt){
        jump_t += dt;
        if(jump_t >= jumpMaxTime){
            if(rb)
                rb.AddForce((transform.forward + transform.up) * jumpForce, ForceMode.Impulse);
            jump_t = 0.0f;
        }
    }
}
