using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Buy : MonoBehaviour
{
    [SerializeField]
    InputActionAsset action;
    [SerializeField]
    TextMesh price;

    public UnityEvent OnBuy;

    bool isActivate = false;

    void Start()
    {
        action.FindActionMap("XRI RightHand Interaction").FindAction("Select").performed += Select;
        action.FindActionMap("XRI RightHand Interaction").FindAction("Select").canceled += CancleSelect;
        try
        {
            price.text = transform.GetChild(2).GetChild(0).GetComponent<Weapon>().Price.ToString();
        }
        catch
        {
            price.text = transform.GetChild(2).GetChild(0).GetComponent<Armor>().Price.ToString();
        }
    }

    void Update()
    {
        var a = UIInteractor.instance.GrabInteractableObject;
    }

    void Select(InputAction.CallbackContext obj)
    {
        //print(obj.ReadValue<float>());
        isActivate = true;
    }
    void CancleSelect(InputAction.CallbackContext obj)
    {
        isActivate = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isActivate)
        {
            if (FindObjectOfType<Player>().Coin >= int.Parse(price.text))
            {
                FindObjectOfType<Player>().Coin -= int.Parse(price.text);
                transform.GetChild(2).GetChild(0).GetComponent<XRGrabInteractable>().enabled = true;
                transform.GetChild(2).GetChild(0).SetParent(null);
                OnBuy?.Invoke();
            }

        }
    }
}
