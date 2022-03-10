using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardManagement
{
    /// <summary>
    /// 棋盘尺寸
    /// </summary>
    public static int boardSize = 5;
    /// <summary>
    /// 方格尺寸
    /// </summary>
    public static float squareSize = 1;

    /// <summary>
    /// 当前棋谱
    /// </summary>
    public static Transform[,] currentComplexion = new Transform[5, 5];

    /// <summary>
    /// 当前可行动的玩家
    /// 先手玩家 = 2，后手玩家 = 1
    /// </summary>
    public static int currentActor = 2;
    /// <summary>
    /// 改变当前行动玩家
    /// </summary>
    public static void ChangeActor()
    {
        if (currentActor == 1)
        {
            currentActor = 2;
        }
        else if(currentActor == 2)
        {
            currentActor = 1;
        }
    }

    #region 驹台
    /// <summary>
    /// 玩家1的驹台
    /// </summary>
    public static Transform[] pieceStand1 = new Transform[19];
    /// <summary>
    /// 下一个放入玩家1驹台的棋子的位置编号
    /// </summary>
    public static int capturePieLoca1 = 0;    
    /// <summary>
    /// 玩家2的驹台
    /// </summary>
    public static Transform[] pieceStand2 = new Transform[19];
    /// <summary>
    /// 下一个放入玩家1驹台的棋子的位置编号
    /// </summary>
    public static int capturePieLoca2 = 0;
    /// <summary>
    /// 玩家手中棋子数量
    /// </summary>
    public static int[] captPieNum = new int[2] { 0, 0 };

    public static Vector3 pieceStandLoca1 = new Vector3(-2, -3, -0.55f);
    public static Vector3 pieceStandLoca2 = new Vector3(2, 3, -0.55f);

    /// <summary>
    /// 获取下一个放入驹台的棋子的位置编号
    /// </summary>
    /// <param name="pieceStand"></param>
    /// <param name="owner"></param>
    /// <param name="pieceOut"></param>
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
    /// 将棋子放入驹台(必须在被捕获的棋子改变拥有者后调用）
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="owner"></param>
    public static void InPieceStand(Transform piece)
    {
        piece.SetParent(null);

        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.isInHand = true;
        captPieNum[info.owner - 1]++;
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
    /// 棋子离开驹台
    /// </summary>
    /// <param name="piece"></param>
    public static void OutPieceStand(Transform piece)
    {
        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.isInHand = false;
        captPieNum[info.owner - 1]--;
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
    /// 判断某方格上是否有棋子
    /// </summary>
    /// <param name="locationX">此方格的横坐标</param>
    /// <param name="locationY">此方格的纵坐标</param>
    /// <param name="piece">此方格上的棋子</param>
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
    /// <summary>
    /// 判断某方格上是否有棋子
    /// </summary>
    /// <param name="locationX">此方格的横坐标</param>
    /// <param name="locationY">此方格的纵坐标</param>
    /// <returns></returns>
    public static bool IsSquareEmpty(int locationX, int locationY)
    {
        if (currentComplexion[locationX, locationY] == null)
        {            
            return true;
        }
        else
        {            
            return false;
        }
    }


}
