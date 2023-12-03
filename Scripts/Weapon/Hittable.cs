using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public UnityEvent<float> OnHit;

    public void Hit(float value)
    {
        OnHit?.Invoke(value);
    }

    private void Start()
    {
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            FindObjectOfType<Player>().Coin += other.GetComponent<Coin>().coin;
            other.GetComponent<Coin>().OnGetCoin?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
