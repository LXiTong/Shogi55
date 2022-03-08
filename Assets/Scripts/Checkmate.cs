using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Checkmate
{
    /// <summary>
    /// 威胁王将/玉将的棋子（拥有者为后手/先手玩家）
    /// </summary>
    public static Queue<PieceInfo> threat = new Queue<PieceInfo>();
    /// <summary>
    /// 一方阵营的棋子
    /// </summary>
    public static Queue<PieceInfo> soldiers = new Queue<PieceInfo>();
    /// <summary>
    /// 王将/玉将一次移动能到达的坐标
    /// </summary>
    public static Queue<Vector2Int> kingOneMove = new Queue<Vector2Int>();

    /// <summary>
    /// 清空所有非空队列
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
    /// 判断棋子是否邻接王将
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
    /// 保存所有王将/玉将一次移动能到达的坐标
    /// </summary>
    /// <param name="kingInfo"></param>
    public static void KingMovableSqu(PieceInfo kingInfo)
    {
        Vector2Int kingLoca = new Vector2Int(kingInfo.lastLocaX, kingInfo.locationY);
        int kingOneMovLeft, kingOneMovRigh, kingOneMovUp, kingOneMovDown;

        #region 限制边界
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
    /// 召集一方阵营除王将以外的全部棋子
    /// </summary>
    /// <param name="kingInfo">此阵营的王</param>
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
    /// 判断是否有棋子威胁着王将。储存所有威胁王将的棋子
    /// </summary>
    /// <param name="kingInfo">可能被威胁着的王将</param>
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
                        //储存所有威胁王将/玉将的棋子
                        threat.Enqueue(pieceInfo);
                    }
                }                           
            }
        }
        return isThereThreat;        
    }
    /// <summary>
    /// 判断若王将移动至某位置后，是否有棋子威胁着王将
    /// </summary>
    /// <param name="loca">王将一次移动能到的位置</param>
    /// <param name="kingInfo">可能被威胁着的王将</param>
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
    /// 判断是否诘
    /// </summary>
    /// <param name="kingInfo">可能被诘的王将</param>
    /// <returns></returns>
    public static bool IsCheckmate(PieceInfo kingInfo)
    {
        
        //若无威胁着王将的棋子
        if (!IsKingInCheck(kingInfo))
        {
            ClearQueue();
            return false;
        }
        else
        {
            Debug.Log("1");
            KingMovableSqu(kingInfo);
            //若王将能移动
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
                //此时kingOneMove为空，王将不应移动
                //若威胁者超过一个
                if (threat.Count > 1)
                {
                    Debug.Log("3");
                    ClearQueue();
                    //诘
                    return true;
                }
                //威胁者只有一个
                else
                {
                    Debug.Log("4");
                    PieceInfo theThreat = threat.Dequeue();

                    #region 尝试捕获威胁者
                    //召集己方棋子
                    GatherSoldiers(kingInfo);
                    while (soldiers.Count > 0)
                    {
                        if (soldiers.Dequeue().CanCapture(theThreat))
                        {
                            
                            ClearQueue();
                            //捕获此棋子则王将处于不受威胁状态
                            return false;
                        }
                    }
                    Vector2Int temp = new Vector2Int(theThreat.locationX, theThreat.locationY);

                    //若王将能捕获威胁者，并且这一手后王将不受威胁
                    if (kingInfo.CanCapture(theThreat) && !IsKingInCheck(temp, kingInfo))
                    {
                        ClearQueue();
                        return false;
                    }
                    #endregion

                    //此时无法捕获威胁者，并且王将无法逃跑
                    #region 尝试挡住威胁者的进攻

                    //若威胁者邻接王将，则无法挡住威胁者的进攻
                    if (IsNextToKing(theThreat, kingInfo))
                    {
                        ClearQueue();
                        return true;
                    }
                    else if(theThreat.pieceType == PieceType.Knight)
                    {
                        //无法挡住桂马的进攻
                        ClearQueue();
                        return true;
                    }
                    //此时可以通过移动友军或打出棋子挡住威胁者
                    else
                    {
                        Vector2Int[] tempPath = new Vector2Int[8];
                        theThreat.GetMovPath(kingInfo.locationX, kingInfo.locationY);
                        //把棋子到王将的路径复制到临时路径数组中
                        theThreat.movePath.CopyTo(tempPath, 0);

                        //召集除王将以外的己方棋子
                        GatherSoldiers(kingInfo);

                        #region 尝试移动友军                        
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

                        #region 尝试打出棋子
                        if (BoardManagement.captPieNum[kingInfo.owner - 1] == 0)
                        {
                            ClearQueue();
                            return true;
                        }
                        else if(BoardManagement.captPieNum[kingInfo.owner - 1] == 1)
                        {
                            //临时驹台
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
            //王将不能移动
            //并且王将周围8个格子只属于2种情况：格子上是友方棋子，格子超过棋盘边界

            //则此时的威胁者一定是桂马！
            else
            {
                //若此时有不只一个威胁者
                if(threat.Count > 1)
                {
                    ClearQueue();
                    return true;
                }
                //此时只有一个威胁者
                else
                {
                    //召集除王将以外的己方棋子
                    GatherSoldiers(kingInfo);

                    //此时除王将以外的己方棋子至少有三个
                    while (soldiers.Count > 0)
                    {
                        //若有至少一个友军能捕获威胁者
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
