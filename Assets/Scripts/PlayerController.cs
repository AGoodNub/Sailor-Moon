using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    //References
    private CharacterController characterController;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3 (moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        
        characterController.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetFloat("F and B", 0, 0.1f, Time.deltaTime);
        animator.SetFloat("L and R", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetFloat("F and B", 0.5f, 0.1f ,Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("L and R", 0.5f, 0.1f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetFloat("F and B", -0.5f, 0.1f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("L and R", -0.5f, 0.1f, Time.deltaTime);
        }
        //animator.SetFloat("Speed", 0.5f, 0.1f ,Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("F and B", 1, 0.1f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("L and R", 1, 0.1f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("F and B", -1, 0.1f, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("L and R", -1, 0.1f, Time.deltaTime);
        }

        //animator.SetFloat("Speed", 1, 0.1f ,Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //animator.SetFloat("Jump", 1, 0.1f, Time.deltaTime);
            animator.SetTrigger("f");
        }
        if (Input.GetKey(KeyCode.S))
        {
            //animator.SetFloat("Jump", -1, 0.1f, Time.deltaTime);
            animator.SetTrigger("b");
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
    }
}
