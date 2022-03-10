using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ϣ
/// </summary>
public class PieceInfo : Promotion
{
    public GameController controller;

    public PieceType pieceType;

    /// <summary>
    /// ���ӵ��ƶ�·������������㡢�յ㣻���Ӳ���������
    /// </summary>
    public Queue<Vector2Int> movePath = new Queue<Vector2Int>();

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

    /// <summary>
    /// ������ӵ��ƶ�·������������㡢�յ㣻���Ӳ���������
    /// </summary>
    /// <param name="inputX">Ŀ��λ�õĺ�����</param>
    /// <param name="inputY">Ŀ��λ�õ�������</param>
    public void GetMovPath(int inputX, int inputY)
    {
        movePath.Clear();
        //���������Ƿɳ�������
        if (pieceType == PieceType.Rook || pieceType == PieceType.PromotedRook)
        {
            //���Ǻ��򳤾����ƶ�
            if (Mathf.Abs(inputX - locationX) >= 1)
            {
                //�����ƶ�
                if (inputX < locationX)
                {
                    for (int i = locationX - 1; i > inputX; i--)
                    {
                        movePath.Enqueue(new Vector2Int(i, locationY));
                        
                    }
                }
                //�����ƶ�
                else
                {
                    for (int i = locationX + 1; i < inputX; i++)
                    {
                        movePath.Enqueue(new Vector2Int(i, locationY));
                        
                    }
                }
            }
            //���򳤾����ƶ�
            else
            {
                //�����ƶ�
                if (inputY < locationY)
                {
                    for (int j = locationY - 1; j > inputY; j--)
                    {
                        movePath.Enqueue(new Vector2Int(locationX, j));
                        
                    }
                }
                //�����ƶ�
                else
                {
                    for (int j = locationY + 1; j < inputY; j++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX, j));
                        
                    }
                }
            }
        }
        //���������ǽ��л�����
        else if (pieceType == PieceType.Bishop || pieceType == PieceType.PromotedBishop)
        {
            //�����ƶ�ʱ�����굽Ŀ��ľ�����ں����굽Ŀ��ľ���
            int dis = Mathf.Abs(inputX - locationX);

            //���ҳ������ƶ�
            if (inputX < locationX - 1)
            {
                //�����ƶ�
                if (inputY < locationY - 1)
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX - i, locationY - i));
                        
                    }
                }
                //�����ƶ�
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX - i, locationY + i));
                        
                    }
                }
            }
            //���󳤾����ƶ�
            else
            {
                //�����ƶ�
                if (inputY < locationY - 1)
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX + i, locationY - i));
                        
                    }
                }
                //�����ƶ�
                else
                {
                    for (int i = 1; i < dis; i++)
                    {
                        movePath.Enqueue(new Vector2Int(locationX + i, locationY + i));
                        
                    }
                }
            }
        }
        //�����������㳵
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
    /// �ж������ƶ�ʱ�Ƿ�ᱻ��ס���������ж������ƶ��Ƿ�Υ��ǰִ�� 
    /// </summary>
    /// <param name="inputX">Ŀ��ĺ�����</param>
    /// <param name="inputY">Ŀ���������</param>
    /// <returns></returns>
    public bool IsBlocked(int inputX, int inputY)
    {
        
        //���������Ƿɳ�������
        if (pieceType == PieceType.Rook || pieceType == PieceType.PromotedRook)
        {
            //���Ǻ��򳤾����ƶ�
            if (Mathf.Abs(inputX - locationX) >= 1)
            {
                //�����ƶ�
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
                //�����ƶ�
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
            //���򳤾����ƶ�
            else
            {
                //�����ƶ�
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
                //�����ƶ�
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
        //���������ǽ��л�����
        else if(pieceType == PieceType.Bishop || pieceType == PieceType.PromotedBishop)
        {
            //�����ƶ�ʱ�����굽Ŀ��ľ�����ں����굽Ŀ��ľ���
            int dis = Mathf.Abs(inputX - locationX);

            //���ҳ������ƶ�
            if (inputX < locationX - 1)
            {
                //�����ƶ�
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
                //�����ƶ�
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
            //���󳤾����ƶ�
            else
            {
                //�����ƶ�
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
                //�����ƶ�
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
        //�����������㳵
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

        //��������������������ᱻ��ס�����������ƶ�ʱ���ᱻ��ס
        return false;
    }

    /// <summary>
    /// �жϽ𽫣�������ƶ���ʽ��ͬ���ܷ��ƶ���ĳһ����
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
    
    //�ж������ܷ��ƶ���ĳһ����
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

    //�ж��Ƿ���Դ��ĳ���ӵ�ĳ����   
    public override bool CanDropped(int locationX, int locationY)
    {
        if (IsPieceWithNoMoves(locationY, this))
        {
            return false;
        }
        else if (pieceType == PieceType.Pawn)
        {
            //������������
            if (TwoPawns(locationX))
            {
                return false;
            }
            //ģ��򲽺�ľ���
            BoardManagement.currentComplexion[locationX, locationY] = GetComponent<Transform>();
            Debug.Log(BoardManagement.currentComplexion[locationX, locationY].name);
            PieceInfo kinInf = controller.kingInfo[2 - owner];
            //����ڵ������
            if (Checkmate.IsCheckmate(kinInf))
            {
                Debug.Log("false");
                //�ָ�ԭ����
                BoardManagement.currentComplexion[locationX, locationY] = null;
                return false;
            }
            //�ָ�ԭ����
            BoardManagement.currentComplexion[locationX, locationY] = null;
            Debug.Log("true");
        }
        return true;
    }

    /// <summary>
    /// �ж�ĳ�����Ƿ�������һ�غϲ���Է�������
    /// </summary>
    /// <param name="pieceInfo">�����ӵ���Ϣ</param>
    /// <param name="kingInfo">���ܱ���������ӵ���Ϣ</param>
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
            #region �ı䱻�������ӵ���Ϣ
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

    //��������
    public override void Promote()
    {
        pieceType++;
        isPromoted = true;
        lastStepIsProm = true;
        GetComponent<MeshRenderer>().material = promotionMaterial;        
    }
    //������������
    public override void UndoPromote()
    {
        pieceType--;
        isPromoted = false;
        GetComponent<MeshRenderer>().material = normMaterial;
    }

    private void Start()
    {
        //��������δ����ʱ�Ĳ���
        normMaterial = GetComponent<MeshRenderer>().material;

        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }
}
