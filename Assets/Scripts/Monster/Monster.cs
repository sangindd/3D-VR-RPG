using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Monster : MonoBehaviour
{
    [SerializeField]
    float exp;
    [SerializeField]
    int gold;

    void Start()
    {

    }

    void Update()
    {

    }

    public void DropExp()
    {
        FindObjectOfType<Player>().GetEXP(exp);
    }

    public void DropGold()
    {
        var coin = Instantiate(Resources.Load("Prefabs/Coin") as GameObject, transform);
        coin.transform.SetParent(null);
        coin.transform.position = new Vector3(coin.transform.position.x, coin.transform.position.y + 1f, coin.transform.position.z);
        coin.GetComponent<Coin>().coin = gold;
    }

    public void DropItem()
    {
        var item = GameManager.Instance.armor.OrderBy(g => Guid.NewGuid()).Take(1).ToList()[0];
        item = Instantiate(item, transform);
        item.name = item.name.Replace("(Clone)", "");
        item.transform.SetParent(null);
        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + 1f, item.transform.position.z);
        item.GetComponent<XRGrabInteractable>().enabled = true;
    }

    public void Drop()
    {
        DropExp();
        DropGold();
        //if (UnityEngine.Random.Range(0f, 1f) <= 0.3f)
        //{
        //    DropItem();
        //}
        //else
        //{
        //    DropGold();
        //}
    }
}
