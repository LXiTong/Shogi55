using UnityEngine;

/// <summary>
/// 棋子移动
/// </summary>
public abstract class Movement : MonoBehaviour
{
    /// <summary>
    /// 之前的位置
    /// </summary>
    public int lastLocaX, lastLocaY;

    /// <summary>
    /// 现在的位置
    /// </summary>
    public int locationX, locationY;

    /// <summary>
    /// 判断某棋子是否可移动到某一方格
    /// </summary>
    /// <param name="inputX">此方格的横坐标</param>
    /// <param name="inputY">此方格的纵坐标</param>
    /// <returns></returns>
    public abstract bool IsMovable(int inputX, int inputY);

    /// <summary>
    /// 判断某棋子是否不可移动
    /// </summary>
    /// <param name="locationY">此棋子所在方格的纵坐标</param>
    /// <param name="piece">此棋子</param>
    /// <returns></returns>
    public bool IsPieceWithNoMoves(int locationY, PieceInfo piece)
    {
        //若棋子持有者为先手玩家
        if(piece.owner == 1)
        {
            switch (piece.pieceType)
            {
                #region 桂马和香车
                //case PieceType.Knight:
                //    {
                //        if (locationY <= 1)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //case PieceType.Lance:
                //    {
                //        if (locationY == 0)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                #endregion
                case PieceType.Pawn:
                    {
                        if (locationY == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        else
        {
            switch (piece.pieceType)
            {
                #region 桂马和香车
                //case PieceType.Knight:
                //    {
                //        if (locationY >= 7)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //case PieceType.Lance:
                //    {
                //        if (locationY == 8)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                #endregion
                case PieceType.Pawn:
                    {
                        if (locationY == 4)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                default:
                    {
                        return false;
                    }
            }
        }        
    }

    /// <summary>
    /// 移动棋子至某一方格
    /// </summary>
    /// <param name="inputX">此方格的横坐标</param>
    /// <param name="inputY">此方格的纵坐标</param>
    public void Move(int inputX, int inputY)
    {
        lastLocaX = locationX;
        lastLocaY = locationY;
        BoardManagement.currentComplexion[lastLocaX, lastLocaY] = null;
        locationX = inputX;
        locationY = inputY;
        BoardManagement.currentComplexion[locationX, locationY] = GetComponent<Transform>();               
    }
    /// <summary>
    /// 撤回棋子移动
    /// </summary>
    public void UndoMove()
    {
        BoardManagement.currentComplexion[locationX, locationY] = null;
        locationX = lastLocaX;
        locationY = lastLocaY;        
        BoardManagement.currentComplexion[locationX, locationY] = GetComponent<Transform>();

        lastLocaX = 0;
        lastLocaY = 0;
    }
}


