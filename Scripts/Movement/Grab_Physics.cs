using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grab_Physics : MonoBehaviour
{
    #region Declaration & Definition
    public float radius = 0.1f;

    public InputActionProperty grabInputSource;
    public LayerMask grabLayer;

    private FixedJoint fixedJoint;

    private bool isGrabbing = false;
    #endregion

    #region Unity Default Method
    private void FixedUpdate()
    {
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;

        if (isGrabButtonPressed && !isGrabbing)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, grabLayer, QueryTriggerInteraction.Ignore);

            // ��ó�� Collider�� ������ ���
            if (nearbyColliders.Length > 0 )
            {
                Rigidbody nearbyRigidbody = nearbyColliders[0].attachedRigidbody;

                fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.autoConfigureConnectedAnchor = false;

                // Grab ��ó�� RigidBody�� ������ ���
                if (nearbyRigidbody)
                {
                    fixedJoint.connectedBody = nearbyRigidbody;
                    fixedJoint.connectedAnchor = nearbyRigidbody.transform.InverseTransformPoint(transform.position);
                }

                // Grab ��ó�� RigidBody�� �������� ���� ���
                else
                {
                    fixedJoint.connectedAnchor = transform.position;
                }

                isGrabbing = true;
            }
        }

        // ��� ���� �� ��ư�� ���� ���
        else if (!isGrabButtonPressed && isGrabbing)
        {
            isGrabbing = false;

            if (fixedJoint)
            {
                Destroy(fixedJoint);
            }
        }
    }
    #endregion
}
