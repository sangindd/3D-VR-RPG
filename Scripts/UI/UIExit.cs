using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
public class UIExit : MonoBehaviour
{
    public UnityEvent OnEnter;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DirectController"))
        {
            if (gameObject.activeSelf)
                OnEnter?.Invoke();
        }
    }
}