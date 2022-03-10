using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    
    void FixedUpdate()
    {
        if(BoardManagement.currentActor == 2)
        {
            text.text = "当前玩家：先手玩家";
        }
        else if(BoardManagement.currentActor == 1)
        {
            text.text = "当前玩家：后手玩家";
        }
        else
        {
            text.text = "不可行动";
        }
        
    }
}
