using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    [SerializeField]
    public float Basic_Dmg = 1;
    float damage = 1;
    [SerializeField]
    float coefficient = 0.1f;

    [SerializeField]
    Player.Type type;
    Stat stat;
    Weapon weapon;

    public UnityEvent OnHit;
    public UnityEvent OnSound;
    public ScriptablePlayer scriptablePlayer;

    void Awake()
    {
        scriptablePlayer = Resources.Load<ScriptablePlayer>("Stat");
        stat = FindObjectOfType<Player>().GetComponent<Stat>();
        weapon = GetComponent<Weapon>();
        OnSound?.Invoke();
        SetDamage();
    }
    private void Update()
    {
        if (!CompareTag("Monster"))
        {
            if (weapon != null && weapon.Damage_Basic != null)
            {
                Basic_Dmg = int.Parse(weapon.Damage_Basic);
                SetDamage();
            }
            if (GetComponent<Staff>() != null || GetComponent<Sword>() != null)
            {
                damage = FindObjectOfType<Player>().dmg + Basic_Dmg;
            }
        }
        else if (CompareTag("Monster"))
        {
            damage = Basic_Dmg;
        }
        else
            SetDamage();
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.CompareTag("Environment"))
        //{
        //    Destroy(this.gameObject, 3f);
        //    return;
        //}
        if (GetComponent<Sword>() != null)
            if (!GetComponent<Sword>().isDelay)
            {
                var t = other.gameObject.GetComponent<Hittable>();

                if (t == null)
                    return;

                if (Random.Range(0f, 1f) <= FindObjectOfType<Player>().cri / 100)
                {
                    t.OnHit?.Invoke(damage * 2);
                    print(damage * 2);
                }
                else
                {
                    t.OnHit?.Invoke(damage);
                    print(damage);
                }
            }
        if (gameObject.name.Contains("Stomp"))
        {
            if (other.gameObject.GetComponent<XROrigin>())
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(other.transform.up * 500f);
                var t = other.gameObject.GetComponent<Hittable>();
                t.OnHit?.Invoke(damage);
                print(damage);
                Destroy(transform.GetChild(1).gameObject);
                Destroy(gameObject, 3f);
            }
        }
        if (gameObject.name.Contains("Rush"))
        {
            print("Rush");
            if (other.gameObject.GetComponent<XROrigin>())
            {
                var t = other.gameObject.GetComponent<Hittable>();
                var dir = other.gameObject.transform.position - other.contacts[0].point;
                dir.y = 0;
                other.gameObject.GetComponent<Rigidbody>().AddForce((-other.transform.forward + other.transform.up) * 500f);
                //Destroy(transform.GetChild(1).gameObject);
                t.OnHit?.Invoke(damage);
                print(damage);
                //Destroy(gameObject, 3f);
            }

        }
        OnHit?.Invoke();

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Environment"))
        //{
        //    Destroy(this.gameObject, 3f);
        //    return;
        //}
        if (this.GetComponent<Sword>() == null && this.GetComponent<Staff>() == null && !transform.CompareTag("Monster"))
        {
            var t = other.gameObject.GetComponent<Hittable>();

            if (t == null)
                return;

            if (other.CompareTag("Ground"))
            {
                OnHit?.Invoke();
                Destroy(this.gameObject, 3f);
            }

            if (Random.Range(0f, 1f) <= FindObjectOfType<Player>().cri / 100)
            {
                t.OnHit?.Invoke(damage * 2);
                print(damage * 2);
            }
            else
            {
                t.OnHit?.Invoke(damage);
                print(damage);
            }
            OnHit?.Invoke();
            Destroy(this.gameObject, 3f);
        }

        if (this.GetComponent<XROrigin>() != null)
        {
            var t = other.gameObject.GetComponent<Hittable>();

            if (t == null)
                return;

            t.OnHit?.Invoke(damage);
            print(damage);

            OnHit?.Invoke();
        }
        if (other.gameObject.GetComponent<XROrigin>() != null)
        {
            var t = other.gameObject.GetComponent<Hittable>();

            if (t == null)
                return;

            t.OnHit?.Invoke(damage);
            print(damage);
            print(other.name);
            OnHit?.Invoke();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player"))
        {
            var t = other.gameObject.GetComponent<Hittable>();

            if (t == null)
                return;

            if (Random.Range(0f, 1f) <= FindObjectOfType<Player>().cri / 100)
            {
                t.OnHit?.Invoke(damage * 2);
                print(damage * 2);
            }
            else
            {
                t.OnHit?.Invoke(damage);
                print(damage);
            }
            OnHit?.Invoke();
            Destroy(this.gameObject, 2f);
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetDamage()
    {
        if (type == FindObjectOfType<Player>().jobType)
            if (type == Player.Type.Sorcerer)
                damage = Basic_Dmg + (scriptablePlayer.Intelligence * coefficient);//데미지 계산
            else
                damage = Basic_Dmg + (scriptablePlayer.Strength * coefficient);
        else
            damage = Basic_Dmg;
    }
}
