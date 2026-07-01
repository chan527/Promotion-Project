using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpPower = 5f;

    // 중복 점프를 막기위한 현재 ground에 있는지 체크하는 변수
    [SerializeField]
    private bool groundCheck;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Animator playerAnimator;

    [SerializeField]
    Transform camera;

    Vector3 moveDir;

    bool canMove;

    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();

        SetMoveDirection();

        // 스페이스바를 누르고 현재 ground 라면 점프 실행
        if (Keyboard.current.spaceKey.wasPressedThisFrame && groundCheck && canMove)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            StartCoroutine(LandingStopCoroutine());
        }
    }

    /// <summary>
    /// groundLayer 만 검사하는 OverlapSphere 를 발 위치에 생성하여 groundCheck 정보 업데이트하는 함수
    /// </summary>
    private void CheckGround()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f, groundLayer);

        groundCheck = hits.Length == 0 ? false : true;

        playerAnimator.SetBool("isGround", groundCheck);
    }

    /// <summary>
    /// wasd 입력을 받아 움직임 방향을 설정
    /// 방향은 camera 기준
    /// </summary>
    private void SetMoveDirection()
    {
        moveDir = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            moveDir += camera.forward;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            moveDir -= camera.forward;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            moveDir += camera.right;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            moveDir -= camera.right;
        }

        // y축 방향은 0로 다시 설정
        moveDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;
    }

    /// <summary>
    /// Rigidbody의 속도를 조절하는 함수
    /// </summary>
    private void Move()
    {
        // 이동 입력이 있으면 isRun true로 설정
        if (moveDir != Vector3.zero)
        {
            playerAnimator.SetBool("isRun", true);
        }
        // 이동 입력이 없으면 isRun false로 설정
        else
        {
            playerAnimator.SetBool("isRun", false);
        }

        // xz 축 속도만 구하고 y 축은 기존 속도 유지
        Vector3 move = rigidbody.linearVelocity;
        move.x = moveDir.x * speed;
        move.z = moveDir.z * speed;

        rigidbody.linearVelocity = move;

        // 이동방향으로 시선 돌리기
        transform.LookAt(transform.position + moveDir);
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        playerAnimator.SetTrigger("doJump");
    }

    /// <summary>
    /// 착지 중에 움직이지 못하도록 애니메이터 ApplyRootMotion을 true로 설정
    /// 대기 시간은 착지 애니메이션 진입, 탈출 transition의 TransitionDuration의 합으로 설정
    /// </summary>
    /// <returns></returns>
    IEnumerator LandingStopCoroutine()
    {
        canMove = false;

        yield return new WaitForSeconds(0.4f);

        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
