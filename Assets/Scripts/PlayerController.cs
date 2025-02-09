using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Animator playerAnim;
    Rigidbody playerRigid;
    CapsuleCollider playerCollider;

    private bool isJumping = false;
    private bool isHit = false;
    private bool isRun = false;
    private int jumpCount = 0;


    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpPower;

    Vector3 playerMovement = new Vector3(0, 0, 0);
    Quaternion playerRotation = Quaternion.identity;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        TryJump();
        IsGround();
        TryRun();
    }

    private void FixedUpdate()
    {
        /*if (isHit)
        {
            return;
        }*/
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");



        playerMovement.Set(horizontal, 0f, vertical);
        playerMovement.Normalize();

        bool hasHorziontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorziontalInput || hasVerticalInput;

        if (!isWalking)
        {
            isRun = false;
            playerAnim.SetBool("isRun", isRun);
        }
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, playerMovement, turnSpeed, 0f);
        playerRotation = Quaternion.LookRotation(desiredForward);

        playerAnim.SetBool("isWalking", isWalking);
    }

    private void OnAnimatorMove()
    {
        if (isHit)
            return;
        if (jumpCount == 0)
        {
            playerRigid.MovePosition(playerRigid.position + playerMovement * playerAnim.deltaPosition.magnitude);
            playerRigid.MoveRotation(playerRotation);
        }
        else if (jumpCount == 1)
        {
            playerRigid.MovePosition(playerRigid.position + playerMovement * Time.fixedDeltaTime * 5);
            playerRigid.MoveRotation(playerRotation);
        }
        else
        {
            playerRigid.MovePosition(playerRigid.position + transform.forward * Time.fixedDeltaTime * 5);
            
        }
         
    }
    private void TryRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = !isRun;
            playerAnim.SetBool("isRun", isRun);
        }
            
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1)
        {
            if (jumpCount != 0)
            {
                playerAnim.SetTrigger("isTumbling");
            }
            else
            {
                   
                playerAnim.SetTrigger("isJumping");   
            }
            Vector3 jumpDirection = Vector3.up * jumpPower;
            playerRigid.AddForce(jumpDirection, ForceMode.Impulse);
            
            jumpCount++;

            
        }

    }

    private void IsGround()
    {
        if (isJumping)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 0.1f))
            {
                if (hitInfo.collider.CompareTag("Ground"))
                    jumpCount = 2;
            }
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isJumping)
        {
            Vector3 slideVelocity = new Vector3(0, playerRigid.velocity.y, 0);
            playerRigid.velocity = slideVelocity;
            jumpCount = 0;
            isJumping = false;
        }


        if (collision.gameObject.CompareTag("Ground") && isJumping)
        {
            jumpCount = 0;
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            StartCoroutine(Hit());
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            return;
        }
    }

    private IEnumerator Hit()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("Â÷·®°ú ºÎµúÈû!!");
        playerAnim.SetTrigger("isHit");
        isHit = true;
        gameObject.layer = 6;
        
        yield return new WaitForSeconds(4f);

        isHit = false;
        gameObject.layer = 8;
    }

}
