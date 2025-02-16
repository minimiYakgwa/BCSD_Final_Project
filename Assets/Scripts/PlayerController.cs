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

    private bool isWalking;
    private bool isJumping;
    private bool isHit;
    private bool isRun;
    private bool isParry;

    
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float moveSpeed;
    private int jumpCount = 0;


    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private GameObject cameraLocation;
    
    Vector3 playerMovement = new Vector3(0, 0, 0);
    Quaternion playerRotation = Quaternion.identity;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        Time.timeScale = 1f;

        isWalking = false;
        isJumping = false;
        isHit = false;
        isRun = false;
        isParry = false;
}

    private void Update()
    {
        TryJump();
        IsGround();
        TryRun();
        TryParry();
    }

    private void FixedUpdate()
    {
        TryWalk();
    }

    private void TryWalk()
    {
        if (isHit)
            return;

        if (jumpCount < 2)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = cameraLocation.transform.forward;
            Vector3 right = cameraLocation.transform.right;

            forward.y = 0f;
            right.y = 0f;

            playerMovement = forward * horizontal * -1 + right * vertical;

            //playerMovement.Set(horizontal, 0f, vertical);
            playerMovement.Normalize();

            bool hasHorziontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

            isWalking = hasHorziontalInput || hasVerticalInput;

            if (!isWalking)
            {
                if (isRun)
                    moveSpeed /= 1.5f;
                isRun = false;
                playerAnim.SetBool("isRun", isRun);
                
            }

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, playerMovement, turnSpeed, 0f);
            playerRotation = Quaternion.LookRotation(desiredForward);


            playerRigid.MoveRotation(playerRotation);

            playerAnim.SetBool("isWalking", isWalking);
        }
        
        playerRigid.MovePosition(playerRigid.position + playerMovement * Time.fixedDeltaTime * moveSpeed);
    }

    private void TryRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) & !isHit)
        {
            if (isRun == false)
                moveSpeed *= 1.5f;
            else
                moveSpeed /= 1.5f;
            isRun = !isRun;
            playerAnim.SetBool("isRun", isRun);
        }
            
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1 && !isHit)
        {
            if (jumpCount != 0)
            {
                playerAnim.SetTrigger("isTumbling");
            }
            else
            {
                   
                playerAnim.SetTrigger("isJumping");   
            }
            playerRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            
            jumpCount++;

            
        }

    }

    private void TryParry()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isParry)
        {
            playerRigid.AddForce(transform.forward * -1, ForceMode.Impulse);
            playerAnim.SetTrigger("isGaurd");
            isParry = false;
        }
            
    }

    public IEnumerator ParryTiming()
    {
        Debug.Log("패링 타이밍 시작");
        isParry = true;
        yield return new WaitForSeconds(1f);
        //yield return new WaitForSeconds(0.18f);
        if (!isParry)
        {
            Debug.Log("패링 성공!!");
        }
            
        else
        {
            StartCoroutine(Hit());
            Debug.Log("패링 타이밍 종료");
            isParry = false;
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
            StartCoroutine(HitWall());
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

        if (collision.gameObject.CompareTag("BehindEnemy"))
        {
            StartCoroutine(HitAndGameOver());
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TurnPoint"))
        {
            StartCoroutine(cameraController.IsTurnCamera(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TurnPoint"))
        {
            StartCoroutine(ExitTurnPoint(other));
            
        }
    }

    private IEnumerator ExitTurnPoint(Collider other)
    {
        other.enabled = false;
        yield return new WaitForSeconds(5f);
        other.enabled = true;
    }

    private IEnumerator Hit()
    {
        Debug.Log("차량과 부딪힘!!");
        playerAnim.SetTrigger("isHit");
        isHit = true;
        gameObject.layer = 6;
        
        yield return new WaitForSeconds(3f);

        isHit = false;
        gameObject.layer = 8;
    }

    private IEnumerator HitWall()
    {
        isHit = true;
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    private IEnumerator HitAndGameOver()
    {
        isHit = true;
        playerAnim.SetTrigger("GameOver");
        gameObject.layer = 6;

        yield return new WaitForSeconds(3f);

        Time.timeScale = 0f;
    }


}
