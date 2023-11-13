using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimPlayTest : MonoBehaviour
{
    public GameObject vfx_fireball;
    public GameObject vfx_fireball_trail;
    public GameObject vfx_fireball_explosion;
    public VisualEffect fireball_explosion;

    [SerializeField]
    private bool IsExplosion = false;
    [SerializeField]
    private float delay = 1f;

    private void Update()
    {
        if (!IsExplosion)
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire1") && !IsExplosion) //È®ÀÎ¿ë (Fire1 : left ctrl)
        {
            Explosion_Fireball();
        }
    }

    public void Shoot()
    {
        vfx_fireball_trail.SetActive(true);
        vfx_fireball.GetComponent<Rigidbody>().AddForce(vfx_fireball.transform.up * 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsExplosion)
        {
            Debug.Log("OnCollisionEnter");
            Explosion_Fireball();
        }
    }

    void Explosion_Fireball()
    {
        Debug.Log("Explosion");
        vfx_fireball_trail.SetActive(false);
        vfx_fireball_explosion.SetActive(true);
        fireball_explosion.Play();
        StartCoroutine(ResetBool(IsExplosion, delay));
    }

    IEnumerator ResetBool(bool boolToReset, float delay)
    {
        yield return new WaitForSeconds(delay);
        vfx_fireball_explosion.SetActive(false);
        vfx_fireball.SetActive(false);
        IsExplosion = !IsExplosion;
    }
}
