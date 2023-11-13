using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoveIsOpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;

    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;

    public void OpenDoor()
    {
        rightDoor.transform.DOLocalRotate(new Vector3(0, -100, 0), 3.0f);
        leftDoor.transform.DOLocalRotate(new Vector3(0, 100, 0), 3.0f);

        openSound.Play();
    }

    public void CloseDoor()
    {
        rightDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 3.1f);
        leftDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), 3.1f);

        closeSound.Play();
    }

}