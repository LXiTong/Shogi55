using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子信息
/// </summary>
public class PieceInfo : Promotion
{
    public GameController controller;

    public PieceType pieceType;

    /// <summary>
    /// 棋子的移动路径（不包括起点、终点；棋子不包括桂马）
    /// </summary>
    public Queue<Vector2Int> movePath = new Queue<Vector2Int>();

    public Material normMaterial;
    public Material promotionMaterial;

    //public bool isSelected = false;

    /// <summary>
    /// 此棋子的持有者
    /// 先手玩家 = 1，后手玩家 = 2
    /// </summary>
    public int owner;
    /// <summary>
    /// 改变此棋子的拥有者
    /// </summary>
    public void ChangeOwner()
    {
        if(owner == 1)
        {
            owner = 2;
        }
        else
        {
            owner = 1;
        }
    }

    /// <summary>
    /// 获得棋子的移动路径（不包括起点、终点；棋子不包括桂马）
    /// </summary>
    /// <param name="inputX">目标位置的横坐标</param>
    /// <param name="inputY">目标位置的纵坐标</param>
    public void GetMovPath(int inputX, int inputY)
    {
        movePath.Clear();
        //若此棋子是飞车或龙王
        if (pieceType == PieceType.Rook || pieceType == PieceType.PromotedRook)
        {
            //若是横向长距离移动
            if (Mathf.Abs(inputX - locationX) >= 1)
            {
                //向右移动
                if (inputX < locationX)
                {
                    for (int i = locationX - 1; i > inputX; i--)
                    {
                        movePath.Enqueue(new Vector2Int(i, locationY));
                        
                    }
                }
                //向左移动
                else
                {
                    for (int i = locationX + 1; i < inputX; i++)
                    {
                        movePath.Enqueue(new Vector2Int(i, locationY));
                        
                    }
                }
            }
            //纵向长距离移动
            else
            {
                //向上移动
                if (inputY < locationY)
                {
                    for (int j = locationY - 1; j > inputY; j--)
                    {
                        movePath.Enqueue(new Vector2Int(locationX, j));
                        
                    }
                }
                //向下移动
                else
                {
                    for (int j = locationY + 1; j < inputY; j++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX, j));
                        
                    }
                }
            }
        }
        //若此棋子是角行或龙马
        else if (pieceType == PieceType.Bishop || pieceType == PieceType.PromotedBishop)
        {
            //角行移动时纵坐标到目标的距离等于横坐标到目标的距离
            int dis = Mathf.Abs(inputX - locationX);

            //向右长距离移动
            if (inputX < locationX - 1)
            {
                //向上移动
                if (inputY < locationY - 1)
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX - i, locationY - i));
                        
                    }
                }
                //向下移动
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX - i, locationY + i));
                        
                    }
                }
            }
            //向左长距离移动
            else
            {
                //向上移动
                if (inputY < locationY - 1)
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX + i, locationY - i));
                        
                    }
                }
                //向下移动
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX + i, locationY + i));
                        
                    }
                }
            }
        }
        //若此棋子是香车
        else if (pieceType == PieceType.Lance)
        {
            if (inputY < locationY - 1)
            {
                for (int j = locationY - 1; j > inputY; j--)
                {
                    movePath.Enqueue(new Vector2Int(locationX, j));
                    
                }
            }
        }
    }

    /// <summary>
    /// 判断棋子移动时是否会被挡住，不能在判断棋子移动是否违规前执行 
    /// </summary>
    /// <param name="inputX">目标的横坐标</param>
    /// <param name="inputY">目标的纵坐标</param>
    /// <returns></returns>
    public bool IsBlocked(int inputX, int inputY)
    {
        
        //若此棋子是飞车或龙王
        if (pieceType == PieceType.Rook || pieceType == PieceType.PromotedRook)
        {
            //若是横向长距离移动
            if (Mathf.Abs(inputX - locationX) >= 1)
            {
                //向右移动
                if (inputX < locationX)
                {
                    for (int i = locationX - 1; i > inputX; i--)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(i, locationY))
                        {
                            return true;
                        }
                    }
                }
                //向左移动
                else
                {
                    for (int i = locationX + 1; i < inputX; i++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(i, locationY))
                        {
                            return true;
                        }
                    }
                }
            }
            //纵向长距离移动
            else
            {
                //向上移动
                if (inputY < locationY)
                {
                    for (int j = locationY - 1; j > inputY; j--)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX, j))
                        {
                            return true;
                        }
                    }                    
                }
                //向下移动
                else
                {
                    for (int j = locationY + 1; j < inputY; j++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX, j))
                        {
                            return true;
                        }
                    }
                }                
            }           
        }
        //若此棋子是角行或龙马
        else if(pieceType == PieceType.Bishop || pieceType == PieceType.PromotedBishop)
        {
            //角行移动时纵坐标到目标的距离等于横坐标到目标的距离
            int dis = Mathf.Abs(inputX - locationX);

            //向右长距离移动
            if (inputX < locationX - 1)
            {
                //向上移动
                if(inputY < locationY - 1)
                {
                    for(int i = 1; i < dis; i++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX - i,locationY - i))
                        {
                            return true;
                        }
                    }
                }
                //向下移动
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX - i, locationY + i))
                        {
                            return true;
                        }
                    }
                }
            }
            //向左长距离移动
            else
            {
                //向上移动
                if(inputY < locationY - 1)
                {
                    for (int i = 1; i < dis; i++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX + i, locationY - i))
                        {
                            return true;
                        }
                    }
                }
                //向下移动
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        
                        if (!BoardManagement.IsSquareEmpty(locationX + i, locationY + i))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        //若此棋子是香车
        else if (pieceType == PieceType.Lance)
        {
            if(inputY < locationY - 1)
            {
                for (int j = locationY - 1; j > inputY; j--)
                {
                    
                    if (!BoardManagement.IsSquareEmpty(locationX, j))
                    {
                        return true;
                    }
                }
            }            
        }

        //上述棋子在其他情况不会被挡住，其他棋子移动时不会被挡住
        return false;
    }

    /// <summary>
    /// 判断金将（或与金将移动方式相同）能否移动到某一方格
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    /// <returns></returns>
    public bool IsGoldMovable(int inputX, int inputY)
    {
        if(owner == 1)
        {
            if ((inputY == locationY + 1 && (inputX == locationX + 1 || inputX == locationX - 1))
                            || inputX > locationX + 1 || inputX < locationX - 1
                                || inputY > locationY + 1 || inputY < locationY - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if ((inputY == locationY - 1 && (inputX == locationX + 1 || inputX == locationX - 1))
                            || inputX > locationX + 1 || inputX < locationX - 1
                                || inputY > locationY + 1 || inputY < locationY - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
    
    //判断棋子能否移动到某一方格
    public override bool IsMovable(int inputX, int inputY)
    {
        if(inputX == locationX && inputY == locationY)
        {
            return false;
        }
        switch (pieceType)
        {
            case PieceType.King:
                {
                    if(inputX > locationX + 1 || inputX < locationX - 1
                        || inputY > locationY + 1 || inputY < locationY - 1)
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.Rook:
                {
                    if(inputX != locationX && inputY != locationY)
                    {
                        return false;
                    }
                    if (IsBlocked(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.PromotedRook:
                {
                    if((inputX != locationX && (inputY < locationY - 1 || inputY > locationY + 1))
                        || (inputY != locationY && (inputX < locationX - 1 || inputX > locationX + 1)))
                    {
                        return false;
                    }
                    if (IsBlocked(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.Bishop:
                {
                    if((inputY != inputX + locationY - locationX)&&(inputY != -inputX + locationY + locationX))
                    {
                        return false;
                    }
                    if (IsBlocked(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.PromotedBishop:
                {
                    if(!(inputX == locationX && (inputY == locationY - 1 || inputY == locationY + 1))
                        && !(inputY == locationY && (inputX == locationX - 1 || inputX == locationX + 1))
                          && (inputY != inputX + locationY - locationX) && (inputY != -inputX + locationY + locationX))
                    {
                        return false;
                    }
                    if (IsBlocked(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.Gold:
                {
                    if (!IsGoldMovable(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.PromotedSilver:
                {
                    if (!IsGoldMovable(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
            case PieceType.PromotedPawn:
                {
                    if (!IsGoldMovable(inputX, inputY))
                    {
                        return false;
                    }
                    break;
                }
        }
        if(owner == 1)
        {
            switch (pieceType)
            {                
                case PieceType.Silver:
                    {
                        if (!(inputY == locationY - 1 && (inputX < locationX + 2 && inputX > locationX - 2))
                            && !(inputY == locationY + 1 && (inputX == locationX - 1 || inputX == locationX + 1)))
                        {
                            return false;
                        }
                        break;
                    }                
                case PieceType.Pawn:
                    {
                        if (inputX != locationX || inputY != (locationY - 1))
                        {
                            return false;
                        }
                        break;
                    }                
            }
        }
        else
        {
            switch (pieceType)
            {
                
                case PieceType.Silver:
                    {
                        if (!(inputY == locationY + 1 && (inputX < locationX + 2 && inputX > locationX - 2))
                            && !(inputY == locationY - 1 && (inputX == locationX - 1 || inputX == locationX + 1)))
                        {
                            return false;
                        }
                        break;
                    }                
                case PieceType.Pawn:
                    {
                        if (inputX != locationX || inputY != (locationY + 1))
                        {
                            return false;
                        }
                        break;
                    }                
            }
        }
        return true;
        
    }

    //判断是否可以打出某棋子到某方格   
    public override bool CanDropped(int locationX, int locationY)
    {
        if (IsPieceWithNoMoves(locationY, this))
        {
            return false;
        }
        else if (pieceType == PieceType.Pawn)
        {
            //“二步”规则
            if (TwoPawns(locationX))
            {
                return false;
            }
            //模拟打步后的局面
            BoardManagement.currentComplexion[locationX, locationY] = GetComponent<Transform>();
            Debug.Log(BoardManagement.currentComplexion[locationX, locationY].name);
            PieceInfo kinInf = controller.kingInfo[2 - owner];
            //“打步诘”规则
            if (Checkmate.IsCheckmate(kinInf))
            {
                Debug.Log("false");
                //恢复原局面
                BoardManagement.currentComplexion[locationX, locationY] = null;
                return false;
            }
            //恢复原局面
            BoardManagement.currentComplexion[locationX, locationY] = null;
            Debug.Log("true");
        }
        return true;
    }

    /// <summary>
    /// 判断某棋子是否能在下一回合捕获对方的棋子
    /// </summary>
    /// <param name="pieceInfo">此棋子的信息</param>
    /// <param name="kingInfo">可能被捕获的棋子的信息</param>
    /// <returns></returns>
    public bool CanCapture(PieceInfo preyPieceInfo)
    {
        if (owner != preyPieceInfo.owner)
        {
            if (IsMovable(preyPieceInfo.locationX, preyPieceInfo.locationY))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 捕获棋子
    /// </summary>
    /// <param name="piece">被捕获的棋子</param>
    public void Capture(Transform piece)
    {
        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.lastLocaX = info.locationX;
        info.lastLocaY = info.locationY;
        info.ChangeOwner();
        
        if (info.isPromoted)
        {
            #region 改变被捕获棋子的信息
            info.pieceType--;
            info.lastStepIsProm = true;
            info.isPromoted = false;
            info.GetComponent<MeshRenderer>().material = info.normMaterial;
            #endregion
        }
        if(info.pieceType != PieceType.King)
        {
            BoardManagement.InPieceStand(piece);
        }

        
    }
    /// <summary>
    /// 撤销捕获棋子
    /// </summary>
    public void UndoCapture()
    {
        BoardManagement.OutPieceStand(GetComponent<Transform>());

        UndoMove();
        ChangeOwner();

        if (lastStepIsProm)
        {
            lastStepIsProm = false;
            Promote();
        }   
    }

    //棋子升变
    public override void Promote()
    {
        pieceType++;
        isPromoted = true;
        lastStepIsProm = true;
        GetComponent<MeshRenderer>().material = promotionMaterial;        
    }
    //撤销棋子升变
    public override void UndoPromote()
    {
        pieceType--;
        isPromoted = false;
        GetComponent<MeshRenderer>().material = normMaterial;
    }

    private void Start()
    {
        //保存棋子未升变时的材质
        normMaterial = GetComponent<MeshRenderer>().material;

        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }
}
