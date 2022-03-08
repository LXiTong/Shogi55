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
        //�ж��Ƿ�����
        PieceInfo info = controller.lastMovePiece.GetComponent<PieceInfo>();
        controller.SentWarning(info);

        panel.SetActive(false);
    }

    public void NoButtFunc()
    {
        //�ж��Ƿ�����
        PieceInfo info = controller.lastMovePiece.GetComponent<PieceInfo>();
        controller.SentWarning(info);

        BoardManagement.ChangeActor();
        panel.SetActive(false);
    }
}
