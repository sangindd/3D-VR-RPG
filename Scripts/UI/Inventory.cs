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

    private void Update() //�׽�Ʈ
    {
        GoldUpdate();
    }

    public void GoldUpdate() //�κ�â�� ��� ������Ʈ
    {
        goldText.text = player.Coin.ToString();
    }

}
