using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float pushPower = 2.0f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // 자식 오브젝트에 있는 애니메이터를 가져옴
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            animator.SetBool("isJumping", false);
        }

        // read WASD input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        controller.Move(moveDirection * speed * Time.deltaTime);

        // 애니메이션 파라미터 설정
        bool isRunning = moveDirection.magnitude > 0;
        animator.SetBool("isRunning", isRunning);

        // Set blend tree parameters
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", z);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Pin"))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            { // the other object has a rigidbody

                // we don't push objects that are below us
                if (hit.moveDirection.y < -0.3f)
                {
                    return;
                }

                Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

                // apply force to push by changing velociity
                rb.velocity = pushDir * pushPower;
            }

        }
    }
}
