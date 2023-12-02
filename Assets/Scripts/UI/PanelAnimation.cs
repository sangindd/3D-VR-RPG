using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PanelAnimation : MonoBehaviour
{
    public UnityEvent OnEnd;

    private void Awake()
    {
        transform.DOScale(Vector3.zero, 0f);
    }

    private void OnEnable()
    {
        transform.DOScale(Vector3.zero, 0f);
        ScaleUp();
    }

    public void End()
    {
        ScaleDown();
    }


    public void ScaleUp() //UIũ�� Ŀ���� �ִϸ��̼�
    {
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }

    public void ScaleDown() //�޴�������
    {
        transform.DOScale(Vector3.zero, 0.3f)
           .SetEase(Ease.Linear).OnComplete(() =>
           {
               OnEnd?.Invoke();
           });
    }
}
