using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stand : MonoBehaviour
{
    [SerializeField]
    GameObject stand;
    [SerializeField]
    List<Transform> stands;

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }
    public void Init()
    {
        Destroy(transform.GetChild(0).gameObject);
        var std = Instantiate(stand, transform);
        //std.transform.parent = this.transform.parent;
        var random = GameManager.Instance.weapon.OrderBy(g => Guid.NewGuid()).Take(4).ToList();
        for (int i = 0; i < random.Count; i++)
        {
            stands[i] = std.transform.GetChild(i).GetChild(2);
            var equip = Instantiate(random[i], stands[i]);
            equip.name = equip.name.Replace("(Clone)", "");
        }

        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
