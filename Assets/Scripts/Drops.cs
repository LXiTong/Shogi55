using UnityEngine;

/// <summary>
/// ���Ӵ��
/// </summary>
public abstract class Drops : Movement
{
    /// <summary>
    /// �����Ƿ������У��ղ�������������У�
    /// </summary>
    public bool isInHand = false;

    /// <summary>
    /// �ж�ĳ�����Ƿ��в���
    /// </summary>
    /// <param name="locationX">���з���ĺ�����</param>
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
    /// �ж��Ƿ���Դ��ĳ���ӵ�ĳ����
    /// </summary>
    /// <param name="locationX">�˷���ĺ�����</param>
    /// <param name="locationY">�˷����������</param>
    /// <param name="piece">�����������</param>
    public bool CanDropped(int locationX, int locationY, PieceInfo piece)
    {        
        if (IsPieceWithNoMoves(locationY, piece))
        {
            return false;
        }
        else if (piece.pieceType == PieceType.Pawn)
        {
            //������������
            if (TwoPawns(locationX))
            {
                return false;
            }

            //����ڵ������
             

        }
        return true;
        
        
    }
}
