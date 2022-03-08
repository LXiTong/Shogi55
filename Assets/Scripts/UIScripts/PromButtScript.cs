using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 升变按钮
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
        //判断是否王手
        PieceInfo info = controller.lastMovePiece.GetComponent<PieceInfo>();
        controller.SentWarning(info);

        panel.SetActive(false);
    }

    public void NoButtFunc()
    {
        //判断是否王手
        PieceInfo info = controller.lastMovePiece.GetComponent<PieceInfo>();
        controller.SentWarning(info);

        BoardManagement.ChangeActor();
        panel.SetActive(false);
    }
}
