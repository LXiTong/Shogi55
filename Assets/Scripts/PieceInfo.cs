using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ϣ
/// </summary>
public class PieceInfo : Promotion
{
    
    public PieceType pieceType;

    public Material normMaterial;
    public Material promotionMaterial;

    //public bool isSelected = false;

    /// <summary>
    /// �����ӵĳ�����
    /// ������� = 1��������� = 2
    /// </summary>
    public int owner;
    /// <summary>
    /// �ı�����ӵ�ӵ����
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
    
    public override bool IsMovable(int inputX, int inputY)
    {
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
                    break;
                }
            case PieceType.PromotedRook:
                {
                    if((inputX != locationX && (inputY < locationY - 1 || inputY > locationY + 1))
                        || (inputY != locationY && (inputX < locationX - 1 || inputX > locationX + 1)))
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

    

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="piece">�����������</param>
    public void Capture(Transform piece)
    {
        PieceInfo info = piece.GetComponent<PieceInfo>();
        info.lastLocaX = info.locationX;
        info.lastLocaY = info.locationY;
        info.ChangeOwner();
        
        if (info.isPromoted)
        {
            info.pieceType--;
            info.lastStepIsProm = true;
            info.isPromoted = false;            
            GetComponent<MeshRenderer>().material = normMaterial;
        }

        BoardManagement.InPieceStand(piece);
    }
    /// <summary>
    /// ������������
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

    public override void Promote()
    {
        pieceType++;
        isPromoted = true;
        lastStepIsProm = true;
        GetComponent<MeshRenderer>().material = promotionMaterial;        
    }
    public override void UndoPromote()
    {
        pieceType--;
        isPromoted = false;
        GetComponent<MeshRenderer>().material = normMaterial;
    }

    private void Start()
    {
        normMaterial = GetComponent<MeshRenderer>().material;
    }
}
