using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public int coin { get; set; }
    public UnityEvent OnGetCoin;
}
