using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlTest : MonoBehaviour
{ 
    bool isWalking = false;
    bool isJumping = false;
    Vector3 velocity;
    float hAxis;
    float vAxis;
    Vector3 moveVec;

    private bool chatenter = false;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [Header("Camera")]
    [SerializeField] private new Transform camera;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private float camSens = 2f;

    [Header("Jump Settings")]
    [SerializeField] float Gravity = 9.81f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float jumpForwardAppliedForce = .5f;
    [SerializeField] float airControl = .5f;


    [Header("Player Name")]
    [SerializeField] private TextMesh player_name;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { chatenter = !chatenter; }
        CameraLookAt();

    }

    void FixedUpdate()
	{

        if (!chatenter)
        {
            Jump();
            Translate();
        }
    }

    void Translate()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        if (isJumping)
        {
            velocity.y -= Gravity * Time.deltaTime;

            if (hAxis != 0 || vAxis != 0)
            {
                Vector3 forward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 right = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                moveVec = forward * vAxis + right * hAxis;
                controller.Move(velocity * Time.deltaTime + moveVec * speed * Time.deltaTime * airControl);

                Quaternion LookAt = Quaternion.LookRotation(moveVec);
                player.rotation = Quaternion.Slerp(player.rotation, LookAt, Time.deltaTime * rotationSpeed);
            }
            else
            {
                controller.Move(velocity * Time.deltaTime);
            }


            if (controller.isGrounded)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
                animator.SetTrigger("jumpLoop");
            }
        }

        else
        {
            if (hAxis != 0 || vAxis != 0)
            {
                Vector3 forward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 right = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                moveVec = forward * vAxis + right * hAxis;
                controller.Move(moveVec * speed * Time.deltaTime);

                Quaternion LookAt = Quaternion.LookRotation(moveVec);
                player.rotation = Quaternion.Slerp(player.rotation, LookAt, Time.deltaTime * rotationSpeed);

                if (!isWalking)
                {
                    isWalking = true;
                    animator.SetBool("isWalking", true);
                }
                
            }
            else
            {
                if (isWalking)
                {
                    isWalking = false;
                    animator.SetBool("isWalking", false);
                }
            }

            if (!controller.isGrounded)
            {
                isJumping = true;
                velocity = controller.velocity * jumpForwardAppliedForce;
                velocity.y = 0f;
            }

        }
    }

    void CameraLookAt()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseDeltaX = camSens * Input.GetAxis("Mouse X");
            float mouseDeltaY = camSens * Input.GetAxis("Mouse Y");

            Vector3 camAngle = cameraArm.rotation.eulerAngles;
            cameraArm.rotation = Quaternion.Euler(camAngle.x - mouseDeltaY, camAngle.y + mouseDeltaX, camAngle.z);
        }

    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
            velocity = controller.velocity * jumpForwardAppliedForce;
            velocity.y = Mathf.Sqrt(2 * Gravity * jumpHeight);
        }
    }


}
