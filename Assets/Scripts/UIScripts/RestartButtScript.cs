using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtScript : MonoBehaviour
{
    GameObject panel;
    void Start()
    {
        panel = GameObject.Find("RestartPanel");
        panel.SetActive(false);
    }

    public void ButtFunc()
    {
        panel.SetActive(true);
    }
    
}
