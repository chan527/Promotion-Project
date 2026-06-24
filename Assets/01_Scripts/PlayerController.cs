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
    Animator playerAnimator;

    Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        groundCheck = true;
    }

    void Update()
    {
        Vector3 dir = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            dir += Vector3.forward;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            dir += Vector3.back;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            dir += Vector3.left;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            dir += Vector3.right;
        }

        // 움직이는 중이면 isRun true로 이동 애니메이션 재생
        if (dir != Vector3.zero)
        {
            playerAnimator.SetBool("isRun", true);
        }
        // 움직이는 중이 아니면 isRun false로 idle 애니메이션 재생
        else
        {
            playerAnimator.SetBool("isRun", false);
        }

        // xz 축 이동만 구함
        Vector3 move = rigidbody.linearVelocity;
        move.x = dir.x * speed;
        move.z = dir.z * speed;


        transform.LookAt(transform.position + dir);

        rigidbody.linearVelocity = move;
        
        // 스페이스바를 누르고 현재 ground 라면 점프 실행
        if (Keyboard.current.spaceKey.wasPressedThisFrame && groundCheck)
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            groundCheck = false;
            playerAnimator.SetTrigger("doJump");
            playerAnimator.ResetTrigger("doLand");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            groundCheck = true;
            playerAnimator.SetTrigger("doLand");
        }
    }
}
