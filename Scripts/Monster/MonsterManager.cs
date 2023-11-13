using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> Areas;
    [SerializeField]
    List<GameObject> Doors;
    [SerializeField]
    List<GameObject> Nevigator;
    [SerializeField]
    List<GameObject> Box;

    List<bool> isOpen = new List<bool> { false, false, false };

    [SerializeField]
    Hp Boss;

    private void Awake()
    {
        for (int i = 0; i < Areas[1].childCount - 1; i++)
        {
            Areas[1].GetChild(i).GetChild(0).GetComponent<Monster_StateMachine>().weapon.GetComponent<Damage>().Basic_Dmg = 10f;
        }
        for (int i = 0; i < Areas[2].childCount - 1; i++)
        {
            Areas[2].GetChild(i).GetChild(0).GetComponent<Monster_StateMachine>().weapon.GetComponent<Damage>().Basic_Dmg = 15f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < Areas.Count - 1; i++)
        {
            if (Areas[i].childCount <= 1 && !isOpen[i])
            {
                Doors[i].GetComponent<LoveIsOpenDoor>().OpenDoor();
                Nevigator[i + 1].SetActive(true);
                try
                {
                    Box[i].SetActive(true);
                }
                catch { }
                SoundManager.Instance.SetUpBGM("city_07");
                isOpen[i] = true;
            }
            if (isOpen[1])
            {
                Doors[2].GetComponent<LoveIsOpenDoor>().OpenDoor();
            }
        }
        foreach (var mon in FindObjectsByType<Monster_StateMachine>(FindObjectsSortMode.None))
        {
            if (mon.state != 0)
            {
                foreach (var door in Doors)
                {
                    //door.GetComponent<LoveIsOpenDoor>().CloseDoor();
                    if (SoundManager.Instance.transform.GetChild(0).GetComponent<AudioSource>().clip.name != "Dungeon")
                    {
                        SoundManager.Instance.SetUpBGM("Dungeon");
                    }
                }
                break;
            }
        }
        if (UIInteractor.instance.GrabInteractableObject != null)
        {
            Doors[3].GetComponent<LoveIsOpenDoor>().OpenDoor();
            foreach (var sound in Doors[3].GetComponents<AudioSource>())
            {
                sound.enabled = false;
            }
            Nevigator[0].SetActive(true);
        }
        try
        {
            Boss = FindObjectOfType<Boss>().GetComponent<Hp>();
            if (Boss.CurrentHp <= 0)
            {
                if (SoundManager.Instance.bgmPlayer.clip.name != "Fight")
                    SoundManager.Instance.SetUpBGM("Fight");
            }
        }
        catch
        {

        }

    }
}
