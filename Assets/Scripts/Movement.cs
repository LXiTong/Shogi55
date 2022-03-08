using UnityEngine;

/// <summary>
/// �����ƶ�
/// </summary>
public abstract class Movement : MonoBehaviour
{
    /// <summary>
    /// ֮ǰ��λ��
    /// </summary>
    public int lastLocaX, lastLocaY;

    /// <summary>
    /// ���ڵ�λ��
    /// </summary>
    public int locationX, locationY;

    /// <summary>
    /// �ж�ĳ�����Ƿ���ƶ���ĳһ����
    /// </summary>
    /// <param name="inputX">�˷���ĺ�����</param>
    /// <param name="inputY">�˷����������</param>
    /// <returns></returns>
    public abstract bool IsMovable(int inputX, int inputY);

    /// <summary>
    /// �ж�ĳ�����Ƿ񲻿��ƶ�
    /// </summary>
    /// <param name="locationY">���������ڷ����������</param>
    /// <param name="piece">������</param>
    /// <returns></returns>
    public bool IsPieceWithNoMoves(int locationY, PieceInfo piece)
    {
        //�����ӳ�����Ϊ�������
        if(piece.owner == 1)
        {
            switch (piece.pieceType)
            {
                #region ������㳵
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
                #region ������㳵
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
    /// �ƶ�������ĳһ����
    /// </summary>
    /// <param name="inputX">�˷���ĺ�����</param>
    /// <param name="inputY">�˷����������</param>
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
    /// ���������ƶ�
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


