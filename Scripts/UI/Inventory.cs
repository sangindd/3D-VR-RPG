using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] TextMeshProUGUI goldText;


    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update() //테스트
    {
        GoldUpdate();
    }

    public void GoldUpdate() //인벤창의 골드 업데이트
    {
        goldText.text = player.Coin.ToString();
    }

}
