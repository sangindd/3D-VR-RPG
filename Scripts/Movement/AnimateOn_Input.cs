using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AnimationInput
{
    public string animationPropertyName;
    public InputActionProperty action;
}

public class AnimateOn_Input : MonoBehaviour
{
    #region Declaration & Definition
    public List<AnimationInput> animationInputs;
    public Animator animator;
    #endregion

    #region Unity Default Method
    void Update()
    {
        foreach (var item in animationInputs)
        {
            float actionValue = item.action.action.ReadValue<float>();
            animator.SetFloat(item.animationPropertyName, actionValue);
        }

        if (!UIManager.instance.PanelsActive()) //ui 켜진게 있다면
        {
            animator.SetFloat("Right Pinch", 1f);
            animator.SetFloat("Right Grab", 0f);
        }
    }
    #endregion
}