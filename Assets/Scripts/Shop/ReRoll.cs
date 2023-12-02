using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReRoll : MonoBehaviour
{
    public UnityEvent OnReRoll;

    bool isReroll = false;

    void Start()
    {
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isReroll)
        {
            isReroll = true;
            OnReRoll?.Invoke();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isReroll = false;
        }
    }
}
