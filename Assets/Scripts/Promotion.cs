using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public abstract class Promotion : Drops
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public bool isPromoted = false;
    /// <summary>
    /// ֮ǰ������״̬
    /// </summary>
    public bool lastStepIsProm = false;

    /// <summary>
    /// �ж������Ƿ��ڵ��󣨿���������
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
    /// �ж�ĳ�������ƶ����Ƿ������
    /// </summary>
    /// <param name="piece">������</param>
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
        //�뿪����ʱ������
        else if (IsInPromotionZone(piece.lastLocaY))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��������ʱ��Ϣ�仯
    /// </summary>
    public abstract void Promote();
    /// <summary>
    /// ����ʱ������������
    /// </summary>
    public abstract void UndoPromote();
}
