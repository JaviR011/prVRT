using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    public float speed = 3f;
    public float runMultiplier = 1.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 120f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isRunning = false;
    private bool isGrounded;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private float rotateInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        // Movimiento (Vector2)
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Correr
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;

        // Saltar
        inputActions.Player.Jump.performed += ctx => Jump();

        // Rotar (eje -1 / +1)
        inputActions.Player.Rotate.performed += ctx => rotateInput = ctx.ReadValue<float>();
        inputActions.Player.Rotate.canceled += ctx => rotateInput = 0f;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void Update()
    {
        // Suelo
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        // Movimiento (usa solo moveInput)
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        move = transform.TransformDirection(move);

        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Rotación (usa solo rotateInput)
        float rotation = Mathf.Clamp(rotateInput, -1f, 1f) * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
