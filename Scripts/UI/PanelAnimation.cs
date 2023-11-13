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


    public void ScaleUp() //UI크기 커지는 애니메이션
    {
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }

    public void ScaleDown() //메뉴꺼질때
    {
        transform.DOScale(Vector3.zero, 0.3f)
           .SetEase(Ease.Linear).OnComplete(() =>
           {
               OnEnd?.Invoke();
           });
    }
}
