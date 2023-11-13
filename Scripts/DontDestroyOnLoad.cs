using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad Instance = null;

    private void Awake()
    {
        if (Instance)
        {
            var SpawnPoint = GameObject.Find("SpawnPoint");
            if (SpawnPoint != null)
            {
                Instance.transform.position = SpawnPoint.transform.position;
                Instance.transform.rotation = SpawnPoint.transform.rotation;
            }
            DestroyImmediate(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
