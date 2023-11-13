using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    //public string loadingSceneName = "Loading";
    public string dungeonSceneName = "03_Dungeon";

    [SerializeField] Image progressBar;
    [SerializeField] Rigidbody rigid;

    public bool isLoading;

    private void Awake()
    {
        rigid = FindObjectOfType<Player>().GetComponentInChildren<Rigidbody>();
        rigid.isKinematic = true;
    }

    private void Start()
    {
        if (isLoading)
            return;

        StartCoroutine(LoadMyAsyncScene2());

    }

    IEnumerator LoadMyAsyncScene2()
    {
        isLoading = true;
        yield return null;

        // 던전 씬이 이미 로드되어 있는지 확인
        if (!SceneManager.GetSceneByName(dungeonSceneName).isLoaded)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(dungeonSceneName);
            op.allowSceneActivation = false;
            float timer = 0.0f;

            while (!op.isDone)
            {
                yield return null;
                timer += Time.deltaTime;
                if (op.progress < 0.9f)
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                    if (progressBar.fillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                    if (progressBar.fillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;
                        rigid.isKinematic = false;
                        isLoading = false;
                        yield break;
                    }
                }
            }

            //if (op.isDone)
            //{
            //    if (SceneManager.GetSceneByName("Loading").isLoaded)
            //    {
            //        Debug.Log("dd");
            //        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("Loading");
            //        rigid.isKinematic = false;
            //        yield return asyncUnload;
            //    }
            //}
        }
    }
}
