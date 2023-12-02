using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class MainMenus : MonoBehaviour
{
    public float MainMenuScale = 1.5f; //���θ޴� �������� ũ�Ⱑ �ʹ� �۾Ƽ� ������ϴ�


    [SerializeField]
    private TextMeshProUGUI mainMenuText; //�߾ӿ� ��Ÿ�� ���θ޴� �����ؽ�Ʈ

    [SerializeField]
    private string myName;  //�ڽŸ޴��� �̸�. ex)ĳ����, ����, ��ų����  �����Ϳ��� �� �޴��鸶�� ��������


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DirectController")) //���θ޴� Ŀ����
        {
            transform.DOScale(2f, 0.5f)
           .SetEase(Ease.OutBack);

            mainMenuText.text = myName;   //���θ޴� �ؽ�Ʈ ����
        }
    }

    private void OnTriggerExit(Collider other) //���θ޴� �۾�����
    {
        if (other.CompareTag("DirectController"))
        {
            transform.DOScale(MainMenuScale, 0.5f)
           .SetEase(Ease.OutBack);

            mainMenuText.text = "";     //���θ޴� �ؽ�Ʈ ����
        }
    }
}
