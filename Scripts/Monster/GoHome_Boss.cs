using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoHome_Boss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<Boss>().isGoHome = true;
        }
    }
}
