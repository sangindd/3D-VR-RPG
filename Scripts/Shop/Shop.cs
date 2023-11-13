using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    public UnityEvent OnOpenShop;
    public UnityEvent OnCloseShop;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnOpenShop?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            OnCloseShop?.Invoke();
    }
}
