using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }

    public UnityEvent OnGameStart;
    public UnityEvent OnReset;

    [SerializeField]
    public List<GameObject> weapon;
    [SerializeField]
    public List<GameObject> armor;

    [SerializeField]
    GameObject gameOver;

    [SerializeField]
    public float Level = 0;

    bool isOver=true;

    void Start()
    {
        StartCoroutine(GameStart());
        gameOver.SetActive(false);
        isOver = true;
        try
        {
            FindObjectOfType<XROrigin>().transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        catch { }
    }

    void Update()
    {
        //print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        if (gameOver.activeSelf)
        {
            if (isOver)
            {
                StartCoroutine(OnGameOver());
                isOver = false;
            }
        }
    }

    IEnumerator OnGameOver()
    {
        for (int i = 10; i > 0; i--)
        {
            if (i == 10)
                gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text.Replace("sec", i.ToString());
            else
                gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text.Replace((i + 1).ToString(), i.ToString());
            yield return new WaitForSeconds(1f);
        }
        gameOver.SetActive(false);
        gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameOver.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text.Replace("1", "sec");
        isOver = true;
        GetComponent<Portal>().OnLoadScene("04_Dungeon");
        OnReset?.Invoke();
        FindObjectOfType<XROrigin>().transform.position = GameObject.Find("BossSpawnPoint").transform.position;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(5f);
        OnGameStart?.Invoke();
    }
}
