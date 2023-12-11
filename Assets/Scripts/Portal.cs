using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public UnityEvent OnPortal;

    void Start()
    {

    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!SceneManager.GetSceneByName("03_Loading").isLoaded)
            {
                OnPortal?.Invoke();
            }
        }
    }

    public void OnLoadScene(string scene)
    {
        StartCoroutine(LoadMyAsyncScene1(scene));
    }

    IEnumerator LoadMyAsyncScene1(string scene)
    {
        if (!SceneManager.GetSceneByName("03_Loading").isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);


            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            //if (asyncLoad.isDone)
            //{
            //    if (SceneManager.GetSceneByName("01_CharacterSelection").isLoaded)
            //    {
            //        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("01_CharacterSelection");
            //        yield return asyncUnload;
            //    }
            //}
        }
    }
}
