using UnityEngine;

/// <summary>
/// 棋子打出
/// </summary>
public abstract class Drops : Movement
{
    /// <summary>
    /// 棋子是否在手中（刚捕获的棋子在手中）
    /// </summary>
    public bool isInHand = false;

    /// <summary>
    /// 判断某列上是否有步兵
    /// </summary>
    /// <param name="locationX">此列方格的横坐标</param>
    public bool TwoPawns(int locationX)
    {
        Transform temp;
        for(int i = 0; i < 5; i++)
        {
            if(!BoardManagement.IsSquareEmpty(locationX, i, out temp))
            {                
                if (temp.GetComponent<PieceInfo>().pieceType == PieceType.Pawn)
                {                    
                    if (temp.GetComponent<PieceInfo>().owner == BoardManagement.currentActor)
                    {                        
                        return true;
                    }
                }
            }             
        }       
        return false;
    }

    /// <summary>
    /// 判断是否可以打出某棋子到某方格
    /// </summary>
    /// <param name="locationX">此方格的横坐标</param>
    /// <param name="locationY">此方格的纵坐标</param>
    /// <param name="piece">被打出的棋子</param>
    public bool CanDropped(int locationX, int locationY, PieceInfo piece)
    {        
        if (IsPieceWithNoMoves(locationY, piece))
        {
            return false;
        }
        else if (piece.pieceType == PieceType.Pawn)
        {
            //“二步”规则
            if (TwoPawns(locationX))
            {
                return false;
            }

            //“打步诘”规则
             

        }
        return true;
        
        
    }
}
