using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ParticleSystem Collision, Trigger 사용
//https://blog.naver.com/chvj7567/222680916995

public class CollisionTest_Object : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField]
    private string vfx_name = "sword_Slash";

    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        particle = GameObject.Find(vfx_name).GetComponent<ParticleSystem>();
    }

    //발동안함
    private void OnParticleTrigger()
    {
        Debug.Log("Object Trigger");
        particle.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        foreach (var item in inside)
        {
            Debug.Log("Object Trigger2");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Object Collision : {other.name}");
    }
}
