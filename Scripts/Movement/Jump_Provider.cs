using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump_Provider : MonoBehaviour
{
    #region Declaration & Definition
    [Header("Mesh Renderer")]
    [SerializeField] public SkinnedMeshRenderer bodyMesh;

    [Header("Input")]
    [SerializeField] public InputActionProperty jumpInputSource;

    [Header("Jump Variable")]
    [SerializeField] public float jumpVelocity = 7.0f;
    [SerializeField] public float jumpHeight = 3.5f;

    [Header("Jump Layer")]
    [SerializeField] public LayerMask groundLayer = 9;

    // Get Component
    private Rigidbody rigidBody;
    private CapsuleCollider bodyCollider;

    // Check Ground Condition
    private bool isGrounded;
    public bool onlyMoveWhenGrounded = false;
    #endregion


    #region Unity Default Method
    private void Start()
    {
        bodyCollider = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool jumpInput = jumpInputSource.action.WasPressedThisFrame();

        if (jumpInput && isGrounded)
            Jump();

        if (jumpInput || !isGrounded)
            bodyMesh.enabled = false;

        else
            bodyMesh.enabled = true;
    }

    private void Jump()
    {
        jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
        Vector3 velocity = Vector3.up * jumpVelocity;

        rigidBody.AddForce(velocity, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
    }
    #endregion

    #region Method
    public bool CheckIfGrounded()
    {
        // ray의 시작점을 Player의 BodyCollider 중앙으로 지정
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.02f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
    #endregion
}
