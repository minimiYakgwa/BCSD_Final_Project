using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    Animator playerAnim;
    Rigidbody playerRigid;
    CapsuleCollider playerCollider;

    [SerializeField]
    Animator enemyAnim;
    [SerializeField]
    private StatusController statusController;

    private bool isWalking;
    private bool isJumping;
    private bool isHit;
    private bool isRun;
    private bool isParry;
    private bool isWall;

    
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float fallSpeed;
    private int jumpCount = 0;


    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private PlaySceneUIManager playSceneUIManager;

    [SerializeField]
    private GameObject cameraLocation;
    
    Vector3 playerMovement = new Vector3(0, 0, 0);
    Quaternion playerRotation = Quaternion.identity;

    [SerializeField]
    private PostProcessVolume volume;
    private LensDistortion lens;

    private float startBoostIntensity = -70f;
    private float endBoostIntensity = 0f;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        isWalking = false;
        isJumping = false;
        isHit = false;
        isRun = false;
        isParry = false;
        isWall = false;

        moveSpeed = GameManager.instance.level * 2.5f;
        gameObject.layer = 8;

        if (volume.profile.TryGetSettings(out lens))
        {

        }

        StartCoroutine(StartBoost());
    }

    private void Update()
    {
        if (GameManager.instance.isStart)
        {
            TryJump();
            IsGround();
            TryRun();
            TryParry();
            IsFall();
        }
 
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isStart)
            return;
        TryWalk();
    }

    private void IsFall()
    {
        if (playerRigid.velocity.y <= 0 && transform.position.y > 0)
            playerRigid.drag = 0f;
        else
        {
            playerRigid.drag = 10f;
        }
    }
    private void TryWalk()
    {
        if (isHit || isWall)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraLocation.transform.forward;
        Vector3 right = cameraLocation.transform.right;

        forward.y = 0f;
        right.y = 0f;

        float y = playerRigid.velocity.y;
        playerMovement = forward * horizontal * -1 + right * vertical;
        playerMovement.Normalize();
        //playerMovement.Set(playerMovement.x, y, playerMovement.z);

        playerRigid.velocity = new Vector3(playerMovement.x * moveSpeed, y, playerMovement.z * moveSpeed);

        bool hasHorziontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        isWalking = hasHorziontalInput || hasVerticalInput;

        if (!isWalking)
        {
            if (isRun)
            {
                moveSpeed /= 1.5f;
                playSceneUIManager.BlinkRunKeyImage(false);
            }
                
            isRun = false;
            playerAnim.SetBool("isRun", false);
            playerAnim.SetBool("isWalking", false);
            SoundManager.instance.StopSE("Player_WalkSound");
            SoundManager.instance.StopSE("Player_RunSound");
            //StartCoroutine(StopRunning());
        }
        else
        {
            if (!isJumping)
                if (isRun)
                    SoundManager.instance.PlaySE("Player_RunSound");
                else
                    SoundManager.instance.PlaySE("Player_WalkSound");
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, playerMovement, turnSpeed, 0f);
            playerRotation = Quaternion.LookRotation(desiredForward);


            playerRigid.MoveRotation(playerRotation);
            if (!isJumping)
            {
                playerAnim.SetBool("isWalking", true);
            }
            else
            {
                if (isRun)
                    SoundManager.instance.StopSE("Player_RunSound");
                else {
                    SoundManager.instance.StopSE("Player_WalkSound");
                }

                
                playerAnim.SetBool("isWalking", false);
            }
            
        } 
            

        
        //playerRigid.AddForce(playerMovement * moveSpeed, ForceMode.Impulse);
        //playerRigid.velocity = new Vector3(playerMovement.x * moveSpeed, y, playerMovement.z * moveSpeed);
        //playerRigid.MovePosition(playerRigid.position + playerMovement * Time.fixedDeltaTime * moveSpeed);
    }

    private void TryRun()
    {
        if (isHit || isWall)
            return;

        if (isRun)
        {

            statusController.DecreaseSp(5);
            if (statusController.currentSp <= 0)
                StartCoroutine(Tired());
        }
            

        

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isHit && !isWall)
        {
            if (isRun == false)
            {
                if (!isJumping)
                    SoundManager.instance.PlaySE("Player_RunSound");
                moveSpeed *= 1.5f;
            }  
            else
            {
                SoundManager.instance.StopSE("Player_RunSound");
                moveSpeed /= 1.5f;
            }
                
            isRun = !isRun;
            playerAnim.SetBool("isRun", isRun);
            playSceneUIManager.BlinkRunKeyImage(isRun);
        }
            
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1 && !isHit)
        {
            playSceneUIManager.BlinkJumpKeyImage();

            SoundManager.instance.StopSE("Player_RunSound");
            SoundManager.instance.StopSE("Player_WalkSound");
            if (jumpCount != 0)
            {
                playerAnim.SetTrigger("isJumping");
                //playerAnim.SetTrigger("isTumbling");
                playerRigid.velocity = new Vector3(playerRigid.velocity.x, jumpPower * 1.5f, playerRigid.velocity.z);
            }
            else
            {
                playerAnim.SetTrigger("isTumbling");
                //playerAnim.SetTrigger("isJumping");
                playerRigid.velocity = new Vector3(playerRigid.velocity.x, jumpPower, playerRigid.velocity.z);
            }
            //playerRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            
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
            enemyAnim.SetTrigger("Hit");
            SoundManager.instance.PlaySE("Player_GaurdSound");
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
                {

                    jumpCount = 2;
                    playerAnim.SetBool("isGround", false);
                }
                    
            }
        } 
    }

    private IEnumerator StopRunning()
    {
        isHit = true;
        yield return new WaitForSeconds(0.27f);
        isHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isJumping)
        {
            Debug.Log("점프 상태로 충돌!!");
            playerRigid.velocity = new Vector3(0f, playerRigid.velocity.y, 0f);
            isWall = true;
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isRun", false);

        }


        if (collision.gameObject.CompareTag("Ground") && isJumping)
        {
            jumpCount = 0;
            isJumping = false;
            if (isWall)
                StartCoroutine(HitWall());
            SoundManager.instance.PlaySE("Player_LandingSound");
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(Hit());
        }

        if (collision.gameObject.CompareTag("BehindEnemy"))
        {
            HitAndGameOver();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            SoundManager.instance.PlaySE("Player_JumpSound");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TurnPoint"))
        {
            StartCoroutine(cameraController.IsTurnCamera(other));
        }

        if (other.gameObject.CompareTag("LapPoint"))
        {
            StartCoroutine(cameraController.IsTurnCamera(other));
            playSceneUIManager.UpdateCurrentLapCount();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TurnPoint") || other.gameObject.CompareTag("LapPoint"))
        {
            StartCoroutine(ExitTurnPoint(other));
            
        }
    }

    private IEnumerator ExitTurnPoint(Collider other)
    {
        other.enabled = false;
        yield return new WaitForSeconds(10f);
        other.enabled = true;
    }

    private IEnumerator Hit()
    {
        Debug.Log("차량과 부딪힘!!");
        playerAnim.SetTrigger("isHit");
        isHit = true;
        
        if (isRun)
        {
            moveSpeed /= 1.5f;
            isRun = false;
        }  
        playSceneUIManager.BlinkRunKeyImage(false);
        gameObject.layer = 6;
        
        yield return new WaitForSeconds(3f);

        isHit = false;
        gameObject.layer = 8;
    }

    private IEnumerator HitWall()
    {
        playerRigid.velocity = new Vector3(0f, playerRigid.velocity.y, 0f);
        yield return null;
        Debug.Log("충돌 해제!!");
        isWall = false;
        playerAnim.SetBool("isRun", isRun);
    }

    private IEnumerator Tired()
    {
        isHit = true;
        playerAnim.SetTrigger("Tired");

        playerRigid.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);

        isHit = false;
    }

    private void HitAndGameOver()
    {
        isHit = true;
        playerAnim.SetTrigger("GameOver");
        gameObject.layer = 6;
        playSceneUIManager.ShowGameOverUI();
        StartCoroutine(GameManager.instance.FailGame());
    }

    private IEnumerator StartBoost()
    {
        yield return new WaitForSeconds(3.5f);
        moveSpeed *= 1.5f;
        lens.intensity.Override(startBoostIntensity);
        yield return new WaitForSeconds(3f);
        moveSpeed /= 1.5f;
        lens.intensity.Override(endBoostIntensity);
    }
}
