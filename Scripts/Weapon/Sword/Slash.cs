using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slash : MonoBehaviour
{
    public GameObject Slash1;
    public GameObject Slash2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Process()
    {
        Slash1.GetComponent<ParticleSystem>().Play();
        Slash2.GetComponent<ParticleSystem>().Play();
    }
}
