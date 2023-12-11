using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisualizer : MonoBehaviour
{
    #region Declaration & Definition
    public GameObject uiPanel;
    #endregion

    #region Unity Default Method
    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            uiPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            uiPanel.SetActive(false);
    }
    #endregion

}