using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHome : MonoBehaviour
{
    [SerializeField] public GameObject monsterHome;

    private Transform monsterTransform;


    void Start()
    {

        // Instantiate MonsterHome
        Instantiate(monsterHome, monsterTransform);


    }

    void Update()
    {
        
    }
}
