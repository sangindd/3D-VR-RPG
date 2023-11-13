using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Potion : MonoBehaviour
{
    bool isGrab = false;

    [SerializeField]
    InputActionAsset action;

    void Start()
    {
        
    }

    void Update()
    {
        if(UIInteractor.instance.GrabInteractableObject == gameObject || UIInteractor.instance.LeftGrabInteractableObject == gameObject)
        {
            print(isGrab);
            isGrab = true;
            action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").performed += Activate;
            action.FindActionMap("XRI LefttHand Interaction").FindAction("Activate").performed += Activate;
        }
        else
        {
            try
            {
                action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").performed -= Activate;
                action.FindActionMap("XRI LefttHand Interaction").FindAction("Activate").performed -= Activate;
            }
            catch { }
        }
    }

    void Activate(InputAction.CallbackContext obj)
    {
        //print(obj.ReadValue<float>());
        FindObjectOfType<Player>().GetComponent<Hp>().Heal();
        action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").performed -= Activate;
        action.FindActionMap("XRI LefttHand Interaction").FindAction("Activate").performed -= Activate;
        Destroy(this.gameObject);
    }
}
