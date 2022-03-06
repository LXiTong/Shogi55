using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardManagement
{
    public static int boardSize = 5;
    public static float squareSize = 1;

    /// <summary>
    /// ��ǰ����
    /// </summary>
    public static Transform[,] currentComplexion = new Transform[5, 5];

    /// <summary>
    /// ��ǰ���ж������
    /// ������� = 1��������� = 2
    /// </summary>
    public static int currentActor = 1;
    /// <summary>
    /// �ı䵱ǰ�ж����
    /// </summary>
    public static void ChangeActor()
    {
        if (currentActor == 1)
        {
            currentActor = 2;
        }
        else
        {
            currentActor = 1;
        }
    }

    #region ��̨
    /// <summary>
    /// ���1�ľ�̨
    /// </summary>
    public static Transform[] pieceStand1 = new Transform[19];
    /// <summary>
    /// ���1������������
    /// </summary>
    public static int capturePieLoca1 = 0;
    /// <summary>
    /// ���2�ľ�̨
    /// </summary>
    public static Transform[] pieceStand2 = new Transform[19];
    /// <summary>
    /// ���2������������
    /// </summary>
    public static int capturePieLoca2 = 0;

    public static Vector3 pieceStandLoca1 = new Vector3(-2, -3, -0.55f);
    public static Vector3 pieceStandLoca2 = new Vector3(2, 3, -0.55f);

    public static void GetPieStaLoca(Transform[] pieceStand, int owner, Transform pieceOut = null)
    {
        for(int i=0; i<pieceStand.Length; i++)
        {
            if(pieceStand[i] == pieceOut)
            {
                if (owner == 1)
                {
                    capturePieLoca1 = i;
                    pieceStandLoca1 = new Vector3(squareSize * i - 2, -3, -0.55f);
                }
                else
                {
                    capturePieLoca2 = i;
                    pieceStandLoca2 = new Vector3(squareSize * -i + 2, 3, -0.55f);
                }
                return;
            }
        }
    }

    /// <summary>
    /// �����ӷ����̨(�����ڱ���������Ӹı�ӵ���ߺ���ã�
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="owner"></param>
    public static void InPieceStand(Transform piece)
    {
        piece.SetParent(null);

        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.isInHand = true;        
        if (info.owner == 1)
        {
            GetPieStaLoca(pieceStand1, 1);
            pieceStand1[capturePieLoca1] = piece;
            piece.SetPositionAndRotation(pieceStandLoca1, Quaternion.Euler(0, 90, 0));
                       
            
        }
        else
        {
            GetPieStaLoca(pieceStand2, 2);
            pieceStand2[capturePieLoca2] = piece;            
            piece.SetPositionAndRotation(pieceStandLoca2, Quaternion.Euler(0, 90, 180));                        
        }
    }
    /// <summary>
    /// �����뿪��̨
    /// </summary>
    /// <param name="piece"></param>
    public static void OutPieceStand(Transform piece)
    {
        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.isInHand = false;
        if (info.owner == 1)
        {
            GetPieStaLoca(pieceStand1, 1, piece);
            pieceStand1[capturePieLoca1] = null;
            
        }
        else
        {
            GetPieStaLoca(pieceStand2, 2, piece);
            pieceStand2[capturePieLoca2] = null;
        }
    }
    #endregion

    
    /// <summary>
    /// �ж�ĳ�������Ƿ�������
    /// </summary>
    /// <param name="locationX">�˷���ĺ�����</param>
    /// <param name="locationY">�˷����������</param>
    /// <param name="piece">�˷����ϵ�����</param>
    /// <returns></returns>
    public static bool IsSquareEmpty(int locationX, int locationY, out Transform piece)
    {
        if(currentComplexion[locationX,locationY] == null)
        {
            piece = null;
            return true;
        }
        else
        {
            piece = currentComplexion[locationX, locationY];
            return false;
        }
    }


}