using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Checkmate
{
    /// <summary>
    /// ��в����/�񽫵����ӣ�ӵ����Ϊ����/������ң�
    /// </summary>
    public static Queue<PieceInfo> threat = new Queue<PieceInfo>();
    /// <summary>
    /// һ����Ӫ������
    /// </summary>
    public static Queue<PieceInfo> soldiers = new Queue<PieceInfo>();
    /// <summary>
    /// ����/��һ���ƶ��ܵ��������
    /// </summary>
    public static Queue<Vector2Int> kingOneMove = new Queue<Vector2Int>();

    /// <summary>
    /// ������зǿն���
    /// </summary>
    public static void ClearQueue()
    {
        if (threat.Count > 0)
        {
            threat.Clear();
        }
        if (soldiers.Count > 0)
        {
            soldiers.Clear();
        }
        if (kingOneMove.Count > 0)
        {
            kingOneMove.Clear();
        }
    }

    /// <summary>
    /// �ж������Ƿ��ڽ�����
    /// </summary>
    /// <param name="pieceInfo"></param>
    /// <param name="kingInfo"></param>
    /// <returns></returns>
    public static bool IsNextToKing(PieceInfo pieceInfo, PieceInfo kingInfo)
    {
        int disX = Mathf.Abs(pieceInfo.locationX - kingInfo.locationX);
        int disY = Mathf.Abs(pieceInfo.locationY - kingInfo.locationY);
        if (disX <= 1 && disY <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ������������/��һ���ƶ��ܵ��������
    /// </summary>
    /// <param name="kingInfo"></param>
    public static void KingMovableSqu(PieceInfo kingInfo)
    {
        Vector2Int kingLoca = new Vector2Int(kingInfo.lastLocaX, kingInfo.locationY);
        int kingOneMovLeft, kingOneMovRigh, kingOneMovUp, kingOneMovDown;

        #region ���Ʊ߽�
        if (kingLoca.x == 0)
        {
            kingOneMovRigh = kingLoca.x;
        }
        else
        {
            kingOneMovRigh = kingLoca.x - 1;
        }

        if (kingLoca.x == 4)
        {
            kingOneMovLeft = kingLoca.x;
        }
        else
        {
            kingOneMovLeft = kingLoca.x + 1;
        }

        if (kingLoca.y == 0)
        {
            kingOneMovUp = kingLoca.y;
        }
        else
        {
            kingOneMovUp = kingLoca.y - 1;
        }

        if (kingLoca.y == 4)
        {
            kingOneMovDown = kingLoca.y;
        }
        else
        {
            kingOneMovDown = kingLoca.y + 1;
        }
        #endregion

        for (int i = kingOneMovRigh; i <= kingOneMovLeft; i++)
        {
            for (int j = kingOneMovUp; j <= kingOneMovDown; j++)
            {
                Transform temp;
                if(BoardManagement.IsSquareEmpty(i,j, out temp))
                {
                    kingOneMove.Enqueue(new Vector2Int(i, j));
                }
                else
                {
                    if (temp.GetComponent<PieceInfo>().owner != kingInfo.owner)
                    {
                        kingOneMove.Enqueue(new Vector2Int(i, j));
                    }                    
                }
            }
        }
    }

    /// <summary>
    /// �ټ�һ����Ӫ�����������ȫ������
    /// </summary>
    /// <param name="kingInfo">����Ӫ����</param>
    public static void GatherSoldiers(PieceInfo kingInfo)
    {
        Transform[,] currComp = BoardManagement.currentComplexion;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(currComp[i,j] != null)
                {
                    PieceInfo pieceInfo = currComp[i, j].GetComponent<PieceInfo>();
                    if (pieceInfo.owner == kingInfo.owner && pieceInfo != kingInfo)
                    {
                        soldiers.Enqueue(pieceInfo);
                    }
                }               
            }
        }
    }

    /// <summary>
    /// �ж��Ƿ���������в������������������в����������
    /// </summary>
    /// <param name="kingInfo">���ܱ���в�ŵ�����</param>
    /// <returns></returns>
    public static bool IsKingInCheck(PieceInfo kingInfo)
    {
        bool isThereThreat = false;
        Transform[,] currComp = BoardManagement.currentComplexion;
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if(currComp[i,j] != null)
                {
                    PieceInfo pieceInfo = currComp[i, j].GetComponent<PieceInfo>();
                    if (pieceInfo.CanCapture(kingInfo))
                    {
                        isThereThreat = true;
                        //����������в����/�񽫵�����
                        threat.Enqueue(pieceInfo);
                    }
                }                           
            }
        }
        return isThereThreat;        
    }
    /// <summary>
    /// �ж��������ƶ���ĳλ�ú��Ƿ���������в������
    /// </summary>
    /// <param name="loca">����һ���ƶ��ܵ���λ��</param>
    /// <param name="kingInfo">���ܱ���в�ŵ�����</param>
    /// <returns></returns>
    public static bool IsKingInCheck(Vector2Int loca, PieceInfo kingInfo)
    {
        Transform[,] currComp = BoardManagement.currentComplexion;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(currComp[i,j] != null)
                {
                    PieceInfo pieceInfo = currComp[i, j].GetComponent<PieceInfo>();
                    if (pieceInfo.owner != kingInfo.owner && pieceInfo.IsMovable(loca.x, loca.y) 
                        && !pieceInfo.isInHand)
                    {
                        return true;

                    }
                }              
            }
        }
        return false;
    }

    //private static void TryCaptThreat(PieceInfo threat, PieceInfo kingInfo)
    //{

    //}

    /// <summary>
    /// �ж��Ƿ�ڵ
    /// </summary>
    /// <param name="kingInfo">���ܱ�ڵ������</param>
    /// <returns></returns>
    public static bool IsCheckmate(PieceInfo kingInfo)
    {
        
        //������в������������
        if (!IsKingInCheck(kingInfo))
        {
            ClearQueue();
            return false;
        }
        else
        {
            Debug.Log("1");
            KingMovableSqu(kingInfo);
            //���������ƶ�
            if (kingOneMove.Count > 0)
            {
                Debug.Log("2");
                while (kingOneMove.Count > 0)
                {
                    if (!IsKingInCheck(kingOneMove.Dequeue(), kingInfo))
                    {
                        Debug.Log("run");
                        ClearQueue();
                        return false;
                    }
                }
                //��ʱkingOneMoveΪ�գ�������Ӧ�ƶ�
                //����в�߳���һ��
                if (threat.Count > 1)
                {
                    Debug.Log("3");
                    ClearQueue();
                    //ڵ
                    return true;
                }
                //��в��ֻ��һ��
                else
                {
                    Debug.Log("4");
                    PieceInfo theThreat = threat.Dequeue();

                    #region ���Բ�����в��
                    //�ټ���������
                    GatherSoldiers(kingInfo);
                    while (soldiers.Count > 0)
                    {
                        if (soldiers.Dequeue().CanCapture(theThreat))
                        {
                            
                            ClearQueue();
                            //������������������ڲ�����в״̬
                            return false;
                        }
                    }
                    Vector2Int temp = new Vector2Int(theThreat.locationX, theThreat.locationY);

                    //�������ܲ�����в�ߣ�������һ�ֺ�����������в
                    if (kingInfo.CanCapture(theThreat) && !IsKingInCheck(temp, kingInfo))
                    {
                        ClearQueue();
                        return false;
                    }
                    #endregion

                    //��ʱ�޷�������в�ߣ����������޷�����
                    #region ���Ե�ס��в�ߵĽ���

                    //����в���ڽ����������޷���ס��в�ߵĽ���
                    if (IsNextToKing(theThreat, kingInfo))
                    {
                        ClearQueue();
                        return true;
                    }
                    else if(theThreat.pieceType == PieceType.Knight)
                    {
                        //�޷���ס����Ľ���
                        ClearQueue();
                        return true;
                    }
                    //��ʱ����ͨ���ƶ��Ѿ��������ӵ�ס��в��
                    else
                    {
                        Vector2Int[] tempPath = new Vector2Int[8];
                        theThreat.GetMovPath(kingInfo.locationX, kingInfo.locationY);
                        //�����ӵ�������·�����Ƶ���ʱ·��������
                        theThreat.movePath.CopyTo(tempPath, 0);

                        //�ټ�����������ļ�������
                        GatherSoldiers(kingInfo);

                        #region �����ƶ��Ѿ�                        
                        while (soldiers.Count > 0)
                        {
                            for(int i = 0; i < tempPath.Length; i++)
                            {
                                if(tempPath[i] != null)
                                {
                                    if(soldiers.Dequeue().IsMovable(tempPath[i].x, tempPath[i].y))
                                    {
                                        ClearQueue();
                                        return false;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ���Դ������
                        if (BoardManagement.captPieNum[kingInfo.owner - 1] == 0)
                        {
                            ClearQueue();
                            return true;
                        }
                        else if(BoardManagement.captPieNum[kingInfo.owner - 1] == 1)
                        {
                            //��ʱ��̨
                            Transform[] tempTran = new Transform[BoardManagement.pieceStand1.Length];
                            if (kingInfo.owner == 1)
                            {
                                tempTran = BoardManagement.pieceStand1;
                            }
                            else
                            {
                                tempTran = BoardManagement.pieceStand2;
                            }
                            for (int i = 0; i < tempTran.Length; i++)
                            {
                                if (tempTran[i] != null)
                                {
                                    PieceInfo tempInfo = tempTran[i].GetComponent<PieceInfo>();
                                    for (int j = 0; j < tempPath.Length; j++)
                                    {
                                        if (tempPath[j] != null)
                                        {
                                            if(tempInfo.CanDropped(tempPath[j].x, tempPath[j].y))
                                            {
                                                ClearQueue();
                                                return false;
                                            }
                                            
                                        }
                                    }
                                    
                                }
                            }

                        }
                        #endregion
                    }
                    #endregion
                }
            }
            //���������ƶ�
            //����������Χ8������ֻ����2����������������ѷ����ӣ����ӳ������̱߽�

            //���ʱ����в��һ���ǹ���
            else
            {
                //����ʱ�в�ֻһ����в��
                if(threat.Count > 1)
                {
                    ClearQueue();
                    return true;
                }
                //��ʱֻ��һ����в��
                else
                {
                    //�ټ�����������ļ�������
                    GatherSoldiers(kingInfo);

                    //��ʱ����������ļ�����������������
                    while (soldiers.Count > 0)
                    {
                        //��������һ���Ѿ��ܲ�����в��
                        if (soldiers.Dequeue().CanCapture(threat.Peek()))
                        {
                            ClearQueue();
                            return false;
                        }
                    }                    
                }
            }
            ClearQueue();
            return false;


            
        }
    }

    //public static bool DropPawnMate()
    //{

    //}
}
