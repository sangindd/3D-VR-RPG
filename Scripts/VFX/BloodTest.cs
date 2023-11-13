using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTest : MonoBehaviour
{
    public GameObject blood;
    private Material material;
    //������ Range�� �޾����� Float�������� �ٲ�
    //1�� ���� ����, 2.5�� ����(���� �ݴ�) value Min:2.5, Max : 1 
    public float value = 3.0f;
    

    public float playerhp = 100;
    [SerializeField]
    private float playerMaxhp = 100;
    [SerializeField]
    private bool isBlood = false;

    void Start()
    {
        material = blood.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (isBlood)
        {
            OnBlood();
        }
    }

    private void OnBlood()
    {
        int i = (int)Math.Floor((playerhp / playerMaxhp)*10);
        blood.SetActive(true);
        switch (i)
        {
            case 10:
                value = 3.0f; //�Ⱥ���
                Debug.Log(value);
                break;
            case 9:
            case 8:
            case 7:
                value = 2.5f;
                Debug.Log(value);
                break;
            case 6:
            case 5:
            case 4:
                value = 2.0f;
                Debug.Log(value);
                break;
            case 3:
            case 2:
                value = 1.5f;
                Debug.Log(value);
                break;
            case 1:
            case 0:
                value = 1.0f;
                Debug.Log(value);
                break;
            default:
                Debug.Log("switch�� �۵�");
                break;
        }
        material.SetFloat("_BloodAlpha", value);
        isBlood = false;
    }
}
