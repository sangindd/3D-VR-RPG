using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;
using UnityEngine.Video;

public class Staff : MonoBehaviour
{
    [SerializeField]
    InputActionReference right;
    [SerializeField]
    InputActionAsset action;

    [SerializeField]
    Transform bottomPos;
    [SerializeField]
    Transform topPos;
    [SerializeField]
    GameObject CoolDownText;
    [SerializeField]
    public Magic Skill;

    bool isActivate = false;
    bool isTrigger = false;
    bool isAvailable = true;

    GameObject skill;
    float cooldown;
    public Weapon weapon;

    public UnityEvent OnActivate;
    public UnityEvent OnChargingHaptic;
    public UnityEvent OnShootHaptic;

    void Start()
    {
        weapon = GetComponent<Weapon>();
        //DB.ReadWeaponData(gameObject.name, weapon);

        action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").performed += Activate;
        action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").canceled += CancleActivate;

    }

    void Update()
    {
        StartCoroutine(Charging());
    }

    void Activate(InputAction.CallbackContext obj)
    {
        //print(obj.ReadValue<float>());
        isActivate = true;
    }
    void CancleActivate(InputAction.CallbackContext obj)
    {
        isActivate = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isTrigger = true;
            //other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Ground") && isTrigger && isActivate)
        {
            GroundSkill();
            isActivate = false;
            isTrigger = false;
            //var skill = Instantiate(Skill, bottom.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            //skill.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 10f, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Trigger"))
        //{
        //    isTrigger = false;
        //    //other.gameObject.SetActive(false);
        //}
    }

    public void ChargingSkill()
    {
        if (Skill.charge != null)
        {
            if (skill == null)
            {
                skill = Instantiate(Skill.charge, topPos.transform);
                OnChargingHaptic?.Invoke();
            }
        }
    }

    public void GroundSkill()
    {
        if (Skill.ground != null && isAvailable)
        {
            //var skill = Instantiate(Skill.ground, bottomPos.transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            //var skill = Instantiate(Skill.ground, bottomPos.transform.position, Quaternion.identity);
            var v = new Vector3(bottomPos.position.x, FindObjectOfType<XROrigin>().gameObject.transform.position.y + 0.01f, bottomPos.position.z);
            var r = Quaternion.LookRotation(Camera.main.transform.forward);
            r.x = 0;
            r.z = 0;
            var skill = Instantiate(Skill.ground, v, r);
            isAvailable = false;
            cooldown = Skill.cooldown;
            //skill.transform.SetParent(null);
            OnActivate?.Invoke();
            StartCoroutine(CoolDown());
            Destroy(skill, Skill.delay_ground);
        }
        else //남은 쿨타임 팝업
        {
            var text = Instantiate(CoolDownText, FindObjectOfType<Player>().coolPos);
            text.GetComponent<TextMesh>().text = "재사용 가능까지 cool초 남았습니다!";
            text.GetComponent<TextMesh>().text = text.GetComponent<TextMesh>().text.Replace("cool", cooldown.ToString("F2"));
        }
        //if (skill == null)
        //    skill = Instantiate(Skill.magic, bottomPos.transform);
    }

    public void Shoot()
    {
        if (skill != null)
        {
            OnShootHaptic?.Invoke();
            skill.GetComponent<Rigidbody>().AddForce(skill.transform.up * 1000);
            skill.transform.SetParent(null);
            //Destroy(skill, 3f);
            skill = null;
        }
    }

    public void Chain()
    {
        if (skill != null)
        {
            skill.transform.SetParent(null);

            //가장 가까운 몬스터 3마리 탐색
            OnShootHaptic?.Invoke();
            GameObject[] mon = GameObject.FindGameObjectsWithTag("Monster");
            SortedDictionary<float, GameObject> dist = new SortedDictionary<float, GameObject>();
            foreach (var i in mon)
            {
                dist.Add(Vector3.Distance(transform.position, i.transform.position), i);
            }
            List<GameObject> monster = new List<GameObject>(dist.Values);
            List<float> monster_dist = new List<float>(dist.Keys);
            //스킬 구현
            List<GameObject> pos = new List<GameObject>();
            skill.GetChildGameObjects(pos);

            pos[0].transform.position = topPos.position;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (monster[i] != null && monster_dist[i] <= 8f)
                    {
                        pos[i + 1].transform.position = new Vector3(monster[i].transform.position.x, monster[i].transform.position.y + 1f, monster[i].transform.position.z);
                        //pos[i + 1].transform.position = Vector3.MoveTowards(pos[i + 1].transform.position,
                        //    new Vector3(monster[i].transform.position.x, topPos.position.y, monster[i].transform.position.z),
                        //    1);
                        monster[i].GetComponent<Hittable>().Hit(skill.GetComponent<Damage>().GetDamage());
                        pos[i + 1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                        skill.GetComponent<Damage>().OnHit?.Invoke();
                    }
                }
                catch
                {
                    pos[3].transform.position = pos[i + 1].transform.position;
                }
            }

            Destroy(skill, 3f);
            skill = null;
        }
    }

    IEnumerator Charging()
    {
        if (isTrigger && isActivate)
            if (transform.rotation.eulerAngles.x > 50f && transform.rotation.eulerAngles.x < 70f)
            {
                ChargingSkill();
                isActivate = false;
                isTrigger = false;
                //var skill = Instantiate(Skill, top.position+new Vector3(0, 0, 0.1f), top.rotation);
                //skill.GetComponent<Rigidbody>().AddRelativeForce(Camera.main.transform.forward*10f, ForceMode.Force);
            }
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator CoolDown()
    {
        while (true)
        {
            if (cooldown <= 0)
            {
                isAvailable = true;
                break;
            }
            yield return new WaitForSeconds(.2f);
            cooldown -= 0.2f;
        }
    }
}
