using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Rig : MonoBehaviour
{
    #region Declaration & Definition
    // Set Player's Transform
    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;

    // Set Player's Physics Transform (with ConfigurableJoint)
    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;

    // Set Player's BodyCollider
    public CapsuleCollider bodyCollider;
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 1.8f;

    #endregion

    #region Unity Default Method
    private void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);


        leftHandJoint.targetPosition = leftController.localPosition;
        leftHandJoint.targetRotation = leftController.localRotation;

        rightHandJoint.targetPosition = rightController.localPosition;
        rightHandJoint.targetRotation = rightController.localRotation;

        headJoint.targetPosition = playerHead.localPosition;
    }
    #endregion
}