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
        if(BoardManagement.currentActor == 1)
        {
            text.text = "��ǰ��ң��������";
        }
        else
        {
            text.text = "��ǰ��ң��������";
        }
        
    }
}
