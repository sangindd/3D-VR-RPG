using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    #region Declaration & Definition
    // Set Player's Transform
    public Transform playerHead;

    // Set Player's BodyCollider
    public CapsuleCollider bodyCollider;
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 1.8f;
    #endregion

    #region Unity Default Method
    private void Start()
    {
            
    }

    private void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);
    }
    #endregion
}
