using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class Rush : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Rush");
        if (collision.transform.GetComponent<XROrigin>()!=null){
            collision.gameObject.GetComponent<Rigidbody>().AddForce((-collision.transform.forward + collision.transform.up) * 300f);
        }
    }
}
