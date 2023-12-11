using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_TimeLine : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("02_CharacterSelection");
    }
}