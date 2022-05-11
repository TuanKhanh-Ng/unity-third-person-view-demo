using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotateSpeed = 15.0f;

    public float groundMoveSpeed = 6.0f;

    private float vertMoveSpeed;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFallSpeed = -1.5f;

    private CharacterController _charController;
    private ControllerColliderHit _contact;

    private Animator _animator;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Movement on the ground
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * groundMoveSpeed;
            movement.z = vertInput * groundMoveSpeed;
            movement = Vector3.ClampMagnitude(movement, groundMoveSpeed);

            Quaternion initialRot = target.rotation;

            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);

            target.rotation = initialRot;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotateSpeed * Time.deltaTime);
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        // Movement in the vertical direction
        bool hitGround = false;
        RaycastHit hit;
        if(vertMoveSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertMoveSpeed = jumpSpeed;
            }
            else
            {
                vertMoveSpeed = minFallSpeed;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            vertMoveSpeed += gravity * 5 * Time.deltaTime;
            if (vertMoveSpeed < terminalVelocity)
            {
                vertMoveSpeed = terminalVelocity;
            }

            if(_contact != null)
            {
                _animator.SetBool("Jumping", true);
            }

            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * groundMoveSpeed;
                }
                else
                {
                    movement += _contact.normal * groundMoveSpeed;
                }
            }
        }
        movement.y = vertMoveSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
}
