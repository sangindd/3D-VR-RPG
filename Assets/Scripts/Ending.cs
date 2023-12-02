using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public string SceneName = "05_Ending";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!SceneManager.GetSceneByName(SceneName).isLoaded)
            {
                OnLoadScene(SceneName);
            }
        }
    }

    public void OnLoadScene(string scene)
    {
        StartCoroutine(LoadMyAsyncScene3(scene));
    }

    IEnumerator LoadMyAsyncScene3(string scene)
    {
        if (!SceneManager.GetSceneByName(SceneName).isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
