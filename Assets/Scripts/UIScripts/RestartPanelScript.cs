using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPanelScript : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        panel = gameObject;
        
    }

    public void YesButtFunc()
    {
        //�����������
        BoardManagement.currentActor = 2;
        SceneManager.LoadScene(0);
    }

    public void NoButtFunc()
    {
        panel.SetActive(false);
    }
}
