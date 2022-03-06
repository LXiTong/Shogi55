using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子升变
/// </summary>
public abstract class Promotion : Drops
{
    /// <summary>
    /// 升变状态
    /// </summary>
    public bool isPromoted = false;
    /// <summary>
    /// 之前的升变状态
    /// </summary>
    public bool lastStepIsProm = false;

    /// <summary>
    /// 判断棋子是否在敌阵（可升变区域）
    /// </summary>
    /// <param name="locationY"></param>
    /// <returns></returns>
    public bool IsInPromotionZone(int locationY)
    {
        if(BoardManagement.currentActor == 1)
        {
            if (locationY == 0)
            {
                return true;
            }
        }else if (locationY == 4)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 判断某棋子在移动后是否可升变
    /// </summary>
    /// <param name="piece">此棋子</param>
    /// <returns></returns>
    public bool IsPromotable(PieceInfo piece)
    {        
        if(piece.pieceType == PieceType.King || piece.pieceType == PieceType.Gold)
        {
            return false;
        }
        if (isPromoted)
        {
            return false;
        }

        if (IsInPromotionZone(piece.locationY))
        {
            return true;
        }
        //离开敌阵时可升变
        else if (IsInPromotionZone(piece.lastLocaY))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 棋子升变时信息变化
    /// </summary>
    public abstract void Promote();
    /// <summary>
    /// 悔棋时撤销棋子升变
    /// </summary>
    public abstract void UndoPromote();
}
