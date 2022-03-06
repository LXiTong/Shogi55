using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int boardSize;

    public GameObject square, king1, king2, rook, bishop, gold, silver, pawn;
    public Material proRookMaterial, proBishopMaterial, proSilverMaterial, proPawnMaterial;

    public GameObject canvas, proInquirePanel;

    GameObject[] perfebs1 = new GameObject[5];
    GameObject[] perfebs2 = new GameObject[5];
    Material[] materials = new Material[5];

    /// <summary>
    /// ����ı߳�
    /// </summary>
    private float squareSize;
    /// <summary>
    /// ���ɷ����λ��
    /// </summary>
    private float squareLoca;

    /// <summary>
    /// ��ѡ�������
    /// </summary>
    public Transform selectedPiece;
    /// <summary>
    /// ��һ���ƶ�������
    /// </summary>
    public Transform lastMovePiece;
    /// <summary>
    /// ��һ���ƶ������������ķ���
    /// </summary>
    public Transform lastMovePiecePare;
    /// <summary>
    /// ��һ���ƶ��Ƿ��д������
    /// </summary>
    public bool didLastMoveIsDrop = false;
    /// <summary>
    /// ��һ���ƶ��Ƿ��в�������
    /// </summary>
    public bool isLastMoveCapture = false;
    /// <summary>
    /// ���һ�α����������
    /// </summary>
    public Transform lastCaptPiece;
    /// <summary>
    /// ���һ�α���������������ķ���
    /// </summary>
    public Transform lastCaptPiecePare;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="posi"></param>
    /// <param name="rota"></param>
    /// <param name="tempSqu"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="own"></param>
    void InstPiece(Vector3 posi, Vector3 rota, GameObject tempSqu, int i, int j, int own)
    {
        posi += new Vector3(0, 0, -0.55f);
        GameObject tempPiece;

        if (own == 1)
        {
            tempPiece = Instantiate(perfebs1[i], posi, Quaternion.Euler(rota));
            tempPiece.GetComponent<PieceInfo>().promotionMaterial = materials[4 - i];
        }
        else
        {
            tempPiece = Instantiate(perfebs2[i], posi, Quaternion.Euler(rota));
            tempPiece.GetComponent<PieceInfo>().promotionMaterial = materials[i];
        }       
        Transform tempTra = tempPiece.transform;
        BoardManagement.currentComplexion[i, j] = tempTra;
        tempTra.SetParent(tempSqu.transform);
        PieceInfo info = tempTra.GetComponent<PieceInfo>();
        info.locationX = i;
        info.locationY = j;
        info.owner = own;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="perfeb"></param>
    /// <param name="material"></param>
    /// <param name="posi"></param>
    /// <param name="rota"></param>
    /// <param name="tempSqu"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="own"></param>
    void InstPiece(GameObject perfeb, Material material, Vector3 posi, Vector3 rota, GameObject tempSqu, int i, int j, int own)
    {
        posi += new Vector3(0, 0, -0.55f);
        GameObject tempPiece = Instantiate(perfeb, posi, Quaternion.Euler(rota));
        Transform tempTra = tempPiece.transform;
        BoardManagement.currentComplexion[i, j] = tempTra;
        tempTra.SetParent(tempSqu.transform);
        PieceInfo info = tempTra.GetComponent<PieceInfo>();

        info.promotionMaterial = material;
        info.locationX = i;
        info.locationY = j;
        info.owner = own;
    }

    /// <summary>
    /// �ƶ�����
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    /// <param name="hitTra"></param>
    /// <param name="isCapture">�����ƶ��Ƿ��в�������</param>
    void Move(int inputX, int inputY, Transform hitTra, bool isCapture)
    {
        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();

        pieceInfo.Move(inputX, inputY);
        lastMovePiece = selectedPiece;
        lastMovePiecePare = selectedPiece.parent;
        if (isCapture)
        {
            selectedPiece.parent = hitTra.parent;
            lastCaptPiece = hitTra;
            lastCaptPiecePare = hitTra.parent;
        }
        else
        {
            selectedPiece.parent = hitTra;
            lastCaptPiece = null;
            lastCaptPiecePare = null;
            
        }
        selectedPiece.localPosition = new Vector3(0, 0, -0.55f);

        if (pieceInfo.lastStepIsProm)
        {
            pieceInfo.lastStepIsProm = false;
        }
        
        selectedPiece = null;
        if (pieceInfo.IsPromotable(pieceInfo) && !didLastMoveIsDrop)
        {
            if (pieceInfo.IsPieceWithNoMoves(pieceInfo.locationY, pieceInfo))
            {
                Promote();
            }
            else
            {
                //Instantiate(proInquirePanel, canvas.transform);
                proInquirePanel.SetActive(true);
            }           
        }
        else
        {
            BoardManagement.ChangeActor();
        }
    }
    /// <summary>
    /// ���������ƶ�
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    /// <param name="hitTra"></param>
    /// <param name="isCapture"></param>
    public void UndoMove()
    {
        PieceInfo pieceInfo = lastMovePiece.GetComponent<PieceInfo>();
        pieceInfo.UndoMove();

        
        if (didLastMoveIsDrop)
        {
            BoardManagement.InPieceStand(lastMovePiece);
        }
        else
        {
            lastMovePiece.SetParent(lastMovePiecePare);
            lastMovePiece.localPosition = new Vector3(0, 0, -0.55f);
            
            lastMovePiecePare = null;

            if (isLastMoveCapture)
            {
                lastCaptPiece.SetParent(lastCaptPiecePare);
                lastCaptPiece.localPosition = new Vector3(0, 0, -0.55f);
                lastCaptPiece.eulerAngles += new Vector3(0, 0, 180);
                lastCaptPiece.GetComponent<PieceInfo>().UndoCapture();

                lastCaptPiece = null;
                lastCaptPiecePare = null;
            }
            if (pieceInfo.lastStepIsProm)
            {
                pieceInfo.lastStepIsProm = false;
                pieceInfo.UndoPromote();
            }
        }
        lastMovePiece = null;
        BoardManagement.ChangeActor();
    }
    /// <summary>
    /// ����ʱ��Ϣ�仯
    /// </summary>
    public void Promote()
    {
        PieceInfo info = lastMovePiece.GetComponent<PieceInfo>();        
        info.Promote();
        BoardManagement.ChangeActor();
    }

    void Start()
    {
        boardSize = BoardManagement.boardSize;
        squareSize = BoardManagement.squareSize; 
        squareLoca = (boardSize - 1) * squareSize / 2;
        canvas = GameObject.Find("Canvas");
        proInquirePanel = GameObject.Find("PromotInquirePanel");
        proInquirePanel.SetActive(false);

        #region ��������������
        perfebs1 = new GameObject[5] { rook, bishop, silver, gold, king1 };
        perfebs2 = new GameObject[5] { king2, gold, silver, bishop, rook };
        materials = new Material[5] { null, null, proSilverMaterial, proBishopMaterial, proRookMaterial };
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {                
                square.GetComponent<SquareInfo>().locationX = i;
                square.GetComponent<SquareInfo>().locationY = j;
                Vector3 temp = new Vector3(squareLoca - (squareSize * i), squareLoca - (squareSize * j));
                GameObject tempSqu = Instantiate(square, temp, Quaternion.identity);
                if (j == 4)
                {
                    InstPiece(temp, new Vector3(0, 90, 0), tempSqu, i, j, 1);                    
                }
                else if(i == 4 && j == 3)
                {
                    InstPiece(pawn, proPawnMaterial, temp, new Vector3(0, 90, 0), tempSqu, i, j, 1);
                }
                else if(j == 0)
                {
                    InstPiece(temp, new Vector3(0, 90, 180), tempSqu, i, j, 2);
                }
                else if(i == 0 && j == 1)
                {
                    InstPiece(pawn, proPawnMaterial, temp, new Vector3(0, 90, 180), tempSqu, i, j, 2);
                }
            }
            
        }
        #endregion
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform hitTra = hit.transform;
                Debug.Log(hitTra.name);

                if (hitTra.tag == "Piece")
                {
                    if (hitTra.GetComponent<PieceInfo>().owner == BoardManagement.currentActor)
                    {
                        selectedPiece = hitTra;
                    }
                    //�Ƿ���ѡ������
                    else if (selectedPiece != null)
                    {
                        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();

                        //ѡ�������Ƿ��ھ�̨
                        if (!pieceInfo.isInHand)
                        {
                            int inputX = hitTra.GetComponent<PieceInfo>().locationX;
                            int inputY = hitTra.GetComponent<PieceInfo>().locationY;

                            //�Ƿ���ƶ����˷���
                            if (pieceInfo.IsMovable(inputX, inputY))
                            {
                                didLastMoveIsDrop = false;
                                isLastMoveCapture = true;                                
                                Move(inputX, inputY, hitTra, isLastMoveCapture);
                                //��������
                                pieceInfo.Capture(hitTra);
                            }
                        }
                    }                   
                }

                if (hitTra.tag == "Board")
                {
                    //�Ƿ���ѡ������
                    if (selectedPiece != null)
                    {
                        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();

                        int inputX = hitTra.GetComponent<SquareInfo>().locationX;
                        int inputY = hitTra.GetComponent<SquareInfo>().locationY;

                        //ѡ�������Ƿ��ھ�̨
                        if (pieceInfo.isInHand)
                        {
                            //�Ƿ�ɴ�����˷���
                            if (pieceInfo.CanDropped(inputX, inputY, pieceInfo))
                            {
                                BoardManagement.OutPieceStand(selectedPiece);
                                didLastMoveIsDrop = true;
                                isLastMoveCapture = false;
                                Move(inputX, inputY, hitTra, isLastMoveCapture);                              
                            }
                        }
                        else
                        {
                            //�Ƿ���ƶ����˷���
                            if (pieceInfo.IsMovable(inputX, inputY))
                            {
                                didLastMoveIsDrop = false;
                                isLastMoveCapture = false;
                                Move(inputX, inputY, hitTra, isLastMoveCapture);
                            }
                        }
                    }
                }                
                                
            }
        }        
    }

}
