using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StopVFX : MonoBehaviour
{
    //public VisualEffect vfx;
    public List<VisualEffect> vfxs;

    [SerializeField]
    private bool Isplayvfx = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (Isplayvfx)
        {
            //vfx.Play();
            VFXPlay();
        }
        else if (!Isplayvfx)
        {
            //vfx.Stop();
            VFXStop();
        }
    }

    public void VFXPlay()
    {
        for (int i = 0; i < vfxs.Count; i++)
        {
            vfxs[i].Play();
            Debug.Log($"{vfxs[i]} is Play");
        }
        Isplayvfx = false;
}

    public void VFXStop()
    {
        for (int i = 0; i < vfxs.Count; i++)
        {
            vfxs[i].Stop();
        }
    }
}
