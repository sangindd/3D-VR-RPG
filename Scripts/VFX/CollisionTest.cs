using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ParticleSystem Collision, Trigger »ç¿ë
//https://blog.naver.com/chvj7567/222680916995

public class CollisionTest : MonoBehaviour
{
    ParticleSystem particle;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Effect Trigger");
        particle.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        foreach (var item in inside)
        {
            Debug.Log("Effect Trigger2");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Effect Collision : {other.name}"); 
    }
}
