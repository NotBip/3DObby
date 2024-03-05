using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private TrailRenderer tr; 
    
    private float movementSpeed = 6f; 
    private float jumpPower = 5f; 
    private float movementX = 0f; 
    private float movementY = 0f; 
    private float movementZ = 0f; 


    private bool canDash = true; 
    private bool isDashing = false; 
    private float dashTime = 0.2f; 
    private float dashingPower = 24f; 
    private float dashCooldown = 0.5f; 


    private void Update()
    { 
        if(isDashing)
            return; 

        movementX = Input.GetAxisRaw("Horizontal") * movementSpeed; 
        movementY = rb.velocity.y; 
        movementZ = Input.GetAxisRaw("Vertical") * movementSpeed;

        if(Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z); 

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(Dash()); 
    }

    private void FixedUpdate()  
    { 
        if(isDashing)
            return;

        rb.velocity = new Vector3(movementX, movementY, movementZ); 
    }   

      private IEnumerator Dash()
    { 
        canDash = false; 
        isDashing = true; 
        rb.useGravity = false; 
        rb.velocity = new Vector3(0.0f, 0.0f, transform.localScale.z * dashingPower);
        tr.emitting = true; 
        yield return new WaitForSeconds(dashTime); 
        tr.emitting = false; 
        rb.useGravity = true; 
        isDashing = false; 
        yield return new WaitForSeconds(dashCooldown); 
        canDash = true; 

    }

}
