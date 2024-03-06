using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private TrailRenderer tr; 
    [SerializeField] private  LayerMask ground; 
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private Transform orientation; 
    [SerializeField] private Transform playerObj; 
    [SerializeField] private Transform combatLookAt;

    private float movementSpeed = 6f; 
    private float jumpPower = 10f; 
    private float movementX = 0f; 
    private float movementY = 0f; 
    private float movementZ = 0f; 


    private bool canDash = true; 
    private bool isDashing = false; 
    private float dashTime = 0.2f; 
    private float dashingPower = 24f; 
    private float dashCooldown = 0.5f; 

    Vector3 moveDirection;

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {   

        movementX = Input.GetAxisRaw("Horizontal") * movementSpeed; 
        movementY = rb.velocity.y; 
        movementZ = Input.GetAxisRaw("Vertical") * movementSpeed;


        Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;
            playerObj.forward = dirToCombatLookAt.normalized;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);            


    }

    private void FixedUpdate()  
    { 
        MovePlayer();
    }   

    private void MovePlayer()
    {
        moveDirection = combatLookAt.forward * movementZ + combatLookAt.right * movementX;
        moveDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.z);
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);

    
    }

    private IEnumerator Dash()
    { 
        canDash = false; 
        isDashing = true; 
        rb.useGravity = false; 
        rb.velocity = new Vector3(0.0f, 0.0f, transform.localScale.z * dashingPower * Mathf.Sign(rb.velocity.z));
        tr.emitting = true; 
        yield return new WaitForSeconds(dashTime); 
        tr.emitting = false; 
        rb.useGravity = true; 
        isDashing = false; 
        yield return new WaitForSeconds(dashCooldown); 
        canDash = true; 

    }

    private bool isGrounded()
    { 
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground); 
    }

}
