using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollisionAnim : MonoBehaviour
{
    public Animator anim;
    public VisualEffect vfx;
    [SerializeField]
    bool isvfxplay = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (vfx.isActiveAndEnabled)
        {
            isvfxplay = true;
        }

        if (isvfxplay)
        {
            anim.Play("vfxanim", -1, 0f);
        }
    }
}
        
