using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// »ÚÆå°´Å¥
/// </summary>
public class UndoButtScript : MonoBehaviour
{
    public int owner = 0;
    Button button;
    GameController controller;

    void Start()
    {
        button = GetComponent<Button>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        if(controller.lastMovePiece != null)
        {
            if (BoardManagement.currentActor != owner)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
        else
        {
            button.interactable = false;
        }        
    }

    public void Undo()
    {
        controller.UndoMove();       
    }
}
