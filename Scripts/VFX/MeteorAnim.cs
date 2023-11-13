using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorAnim : MonoBehaviour
{
    public GameObject vfx_fireball2;
    public GameObject vfx_light;
    public GameObject vfx_Trail2;

    [Header("Meteor Collider")]
    [SerializeField]
    GameObject col;
    [SerializeField]
    private float colScale = 1.5f;
    [SerializeField]
    private float colAnimTime= .5f;

    [Header("Meteor Local Position")]
    [SerializeField]
    private Transform meteorTopPos;
    [SerializeField]
    private Transform meteorBottomPos;

    [Header("Meteor Animation Value")]
    [SerializeField]
    private Ease easyType = Ease.Linear;
    [SerializeField]
    private float animTime = 1.5f;

    private void OnEnable()
    {
        FireballActive(true);
        transform.position = meteorTopPos.position;
        col.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
        StartCoroutine(Animate());
    }

    private void FireballActive(bool isActive)
    {
        vfx_fireball2.SetActive(isActive);
        vfx_light.SetActive(isActive);
        vfx_Trail2.SetActive(isActive);
    }

    IEnumerator Animate()
    {
        transform.DOMove(meteorBottomPos.position, animTime).SetEase(easyType);
        yield return new WaitForSeconds(animTime);
        col.transform.DOScale(new Vector3(colScale, colScale, colScale), colAnimTime);
        yield return new WaitForSeconds(0.1f);
        FireballActive(false);
    }
}
