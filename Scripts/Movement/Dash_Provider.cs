using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash_Provider : MonoBehaviour
{
    #region Declaration & Definition

    [Header("Mesh Renderer")]
    [SerializeField] private SkinnedMeshRenderer bodyMesh;

    [Header("Input")]
    // Set Dash Input Source
    [SerializeField] private InputActionProperty dashInputSource;
    [SerializeField] private Camera mainCamera;

    [Header("Dash Variable")]
    // Input Direction
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float dashDuration;

    [Header("CoolDown")]
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    [Header("Settings")]
    [SerializeField] private bool disableGravity = false;
    [SerializeField] private bool resetVelocity = false;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundLayer = 9;

    // Get Component
    private CapsuleCollider bodyCollider;
    private Rigidbody rigidBody;
    private Hittable hittable;

    // Set Parameter
    private bool isGrounded;
    private bool isDashing;

    private Vector3 delayedForceToApply;

    #endregion

    #region Unity Default Method
    void Start()
    {
        // Get Necessary Component
        bodyCollider = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
        hittable = GetComponent<Hittable>();
    }

    void Update()
    {
        bool dashInput = dashInputSource.action.WasPerformedThisFrame();

        if (dashInput)
            Dash();

        if (dashCoolDownTimer > 0)
            dashCoolDownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
    }
    #endregion

    #region Method
    private void Dash()
    {
        if (dashCoolDownTimer > 0) 
            return;

        else 
            dashCoolDownTimer = dashCoolDown;

        // isDashing = true;

        Vector3 forceToApply = mainCamera.transform.forward * dashForce + mainCamera.transform.up * dashUpwardForce;

        if (disableGravity)
            rigidBody.useGravity = false;

        delayedForceToApply = forceToApply;

        hittable.enabled = false;

        Invoke(nameof(delayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);

    }

    private void delayedDashForce()
    {
        if (resetVelocity)
            rigidBody.velocity = Vector3.zero;

        rigidBody.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        // isDashing = false;
        hittable.enabled = true;

        if (disableGravity)
            rigidBody.useGravity = true;
    }

    private bool CheckIfGrounded()
    {
        // Start Point of Ray = Center of Player's BodyCollider
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.02f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }

    #endregion
}