using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    #region Declaration & Definition
    public float speed = 3.0f;
    public float turnSpeed = 110.0f;
    public float inputTurnAxis;
    public float jumpVelocity = 7.0f;
    public float jumpHeight = 3.5f;

    // Check Ground Condition
    public bool isGrounded;
    public bool onlyMoveWhenGrounded = false;

    // Target을 움직일 Rigidbody(XR Origin을 받아옴)
    public Rigidbody rigidBody;
    // 방향을 잡아줄 Source(Main Camera의 방향을 받아옴)
    public Transform directionSource;
    // Turn 기능으로 방향을 바꿔줄 Source(Main Camera의 방향을 바꿈)
    public Transform turnSource;
    public CapsuleCollider bodyCollider;

    // Ground를 Check하기 위한 Layer
    public LayerMask groundLayer;
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;

    private Vector2 inputMoveAxis;
    #endregion

    #region Unity Default Method
    void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

        bool jumpInput = jumpInputSource.action.WasPressedThisFrame();

        if (jumpInput && isGrounded)
        {
            jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            rigidBody.velocity = Vector3.up * jumpVelocity;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();

        if (onlyMoveWhenGrounded || (onlyMoveWhenGrounded && isGrounded))
        {
            // Set Direction & Yaw
            Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            Vector3 targetMovePosition = rigidBody.position + direction * Time.fixedDeltaTime * speed;

            // Turn
            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;
            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rigidBody.MoveRotation(rigidBody.rotation * q);

            // Set new Move Position
            Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

            rigidBody.MovePosition(newPosition);
        }
    }
    #endregion

    #region Method
    public bool CheckIfGrounded()
    {
        // ray의 시작점을 Player의 BodyCollider 중앙으로 지정
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
    #endregion
}
