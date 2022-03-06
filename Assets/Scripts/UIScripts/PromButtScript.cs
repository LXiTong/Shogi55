using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���䰴ť
/// </summary>
public class PromButtScript : MonoBehaviour
{
    public GameController controller;
    public GameObject panel;

    private void Awake()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        panel = gameObject;
        
    }
    public void YesButtFunc()
    {
        controller.Promote();
        panel.SetActive(false);
    }

    public void NoButtFunc()
    {
        BoardManagement.ChangeActor();
        panel.SetActive(false);
    }
}
