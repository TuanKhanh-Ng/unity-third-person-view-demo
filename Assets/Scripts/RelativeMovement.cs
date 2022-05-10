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

    void Start()
    {
        _charController = GetComponent<CharacterController>();
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

        // Movement in the vertical direction
        if (_charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertMoveSpeed = jumpSpeed;
            }
            else
            {
                vertMoveSpeed = minFallSpeed;
            }
        }
        else
        {
            vertMoveSpeed += gravity * 5 * Time.deltaTime;
            if (vertMoveSpeed < terminalVelocity)
            {
                vertMoveSpeed = terminalVelocity;
            }
        }
        movement.y = vertMoveSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
    }
}
