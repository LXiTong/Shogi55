using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int boardSize;

    public GameObject square, king1, king2, rook, bishop, gold, silver, pawn;
    public Material proRookMaterial, proBishopMaterial, proSilverMaterial, proPawnMaterial;

    public GameObject canvas, proInquirePanel, checkWarning, gameOverWarning;

    /// <summary>
    /// 是否出现过王手
    /// </summary>
    public bool isCheckBefore = false;

    GameObject[] perfebs1 = new GameObject[5];
    GameObject[] perfebs2 = new GameObject[5];
    Material[] materials = new Material[5];

    /// <summary>
    /// 方格的边长
    /// </summary>
    private float squareSize;
    /// <summary>
    /// 生成方格的位置
    /// </summary>
    private float squareLoca;

    /// <summary>
    /// 被选择的棋子
    /// </summary>
    public Transform selectedPiece;
    /// <summary>
    /// 上一次移动的棋子
    /// </summary>
    public Transform lastMovePiece;
    /// <summary>
    /// 上一次移动的棋子所属的方块
    /// </summary>
    public Transform lastMovePiecePare;
    /// <summary>
    /// 上一次移动是否有打出棋子
    /// </summary>
    public bool didLastMoveIsDrop = false;
    /// <summary>
    /// 上一次移动是否有捕获棋子
    /// </summary>
    public bool isLastMoveCapture = false;
    /// <summary>
    /// 最后一次被捕获的棋子
    /// </summary>
    public Transform lastCaptPiece;
    /// <summary>
    /// 最后一次被捕获的棋子所属的方块
    /// </summary>
    public Transform lastCaptPiecePare;

    /// <summary>
    /// 王将和玉将的信息
    /// </summary>
    public PieceInfo[] kingInfo = new PieceInfo[2];

    /// <summary>
    /// 生成棋子
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

        //保存王将和玉将的信息
        if(info.pieceType == PieceType.King)
        {
            kingInfo[info.owner - 1] = info;
        }
    }
    /// <summary>
    /// 生成棋子
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
    /// 移动棋子
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    /// <param name="hitTra"></param>
    /// <param name="isCapture">本次移动是否有捕获棋子</param>
    void Move(int inputX, int inputY, Transform hitTra, bool isCapture)
    {
        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();
        PieceInfo hitInfo = hitTra.GetComponent<PieceInfo>();

        pieceInfo.Move(inputX, inputY);
        //记录本次移动的棋子
        lastMovePiece = selectedPiece;
        lastMovePiecePare = selectedPiece.parent;
        if (isCapture)
        {            
            selectedPiece.parent = hitTra.parent;
            //记录被捕获的棋子
            lastCaptPiece = hitTra;
            lastCaptPiecePare = hitTra.parent;
        }
        else
        {
            selectedPiece.parent = hitTra;
            lastCaptPiece = null;
            lastCaptPiecePare = null;
            
        }
        //移动棋子
        selectedPiece.localPosition = new Vector3(0, 0, -0.55f);
        selectedPiece = null;

        if (pieceInfo.lastStepIsProm)
        {
            pieceInfo.lastStepIsProm = false;
        }
        
        //判断是否提示棋子晋升/直接晋升棋子
        if (pieceInfo.IsPromotable(pieceInfo) && !didLastMoveIsDrop && !IsGameOver(hitInfo))
        {
            if (pieceInfo.IsPieceWithNoMoves(pieceInfo.locationY, pieceInfo))
            {
                Promote();
                //发送 王手/诘 提示
                SentWarning(pieceInfo);
            }
            else
            {
                //Instantiate(proInquirePanel, canvas.transform);

                //玩家选择是否升变
                proInquirePanel.SetActive(true);
            }           
        }
        else
        {            
            //发送 王手/诘 提示
            SentWarning(pieceInfo);
            BoardManagement.ChangeActor();
        }

        
    }
    /// <summary>
    /// 撤销棋子移动
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
    /// 升变时信息变化
    /// </summary>
    public void Promote()
    {
        PieceInfo info = lastMovePiece.GetComponent<PieceInfo>();        
        info.Promote();
        BoardManagement.ChangeActor();
    }

    /// <summary>
    /// 发送 王手/诘 提示
    /// </summary>
    /// <param name="pieceInfo"></param>
    public void SentWarning(PieceInfo pieceInfo)
    {
        if (pieceInfo.CanCapture(kingInfo[2 - pieceInfo.owner]))
        {
            Debug.Log(kingInfo[2 - pieceInfo.owner].owner);
            if (!Checkmate.IsCheckmate(kingInfo[2 - pieceInfo.owner]))
            {
                checkWarning.transform.GetChild(0).GetComponent<Text>().text = "王手";
            }
            else
            {
                checkWarning.transform.GetChild(0).GetComponent<Text>().text = "诘";
            }

            //记录是否出现过王手
            isCheckBefore = true;

            checkWarning.SetActive(true);
            //一段时间后关闭 王手/诘 提示
            Invoke(nameof(CheckWarningTurnOff), 2);

            return;
        }
        if (isCheckBefore)
        {
            if (Checkmate.IsCheckmate(kingInfo[2 - pieceInfo.owner]))
            {
                checkWarning.transform.GetChild(0).GetComponent<Text>().text = "诘";
                checkWarning.SetActive(true);
                //一段时间后关闭 诘提示
                Invoke(nameof(CheckWarningTurnOff), 2);
            }            
        }                     
    }

    /// <summary>
    /// 关闭王手提示
    /// </summary>
    public void CheckWarningTurnOff()
    {
        checkWarning.SetActive(false);
    }
    /// <summary>
    /// 关闭游戏结束提示
    /// </summary>
    public void GameOverWarningTurnOff()
    {
        gameOverWarning.SetActive(false);
    }

    /// <summary>
    /// 判断被捕获的棋子是否为王将
    /// </summary>
    /// <param name="pieceInfo">被捕获的棋子</param>
    /// <returns></returns>
    public bool IsGameOver(PieceInfo pieceInfo)
    {
        if(pieceInfo != null)
        {
            if (pieceInfo.pieceType == PieceType.King)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }       
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="kingInfo">被捕获的王将的信息</param>
    public void GameOver(PieceInfo kingInfo)
    {
        Text text = gameOverWarning.transform.GetChild(0).GetComponent<Text>();
        if(kingInfo.owner == 2)
        {
            text.text = "后手玩家胜利!";
        }
        else
        {
            text.text = "先手玩家胜利!";
        }
        //王将 从棋盘上消失
        kingInfo.gameObject.SetActive(false);
        //禁止玩家操作棋子
        BoardManagement.currentActor = 0;
        //禁止悔棋
        lastMovePiece = null;

        gameOverWarning.SetActive(true);
        Invoke(nameof(GameOverWarningTurnOff), 2);
    }

    void Start()
    {
        boardSize = BoardManagement.boardSize;
        squareSize = BoardManagement.squareSize; 
        squareLoca = (boardSize - 1) * squareSize / 2;
        canvas = GameObject.Find("Canvas");
        proInquirePanel = GameObject.Find("PromotInquirePanel");        
        proInquirePanel.SetActive(false);
        checkWarning = GameObject.Find("CheckWarning");
        CheckWarningTurnOff();
        gameOverWarning = GameObject.Find("GameOverWarning");
        GameOverWarningTurnOff();

        #region 生成棋盘与棋子
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
                    //是否有选中棋子
                    else if (selectedPiece != null)
                    {
                        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();

                        //选中棋子是否在驹台
                        if (!pieceInfo.isInHand)
                        {
                            PieceInfo hitInfo = hitTra.GetComponent<PieceInfo>();
                            int inputX = hitInfo.locationX;
                            int inputY = hitInfo.locationY;

                            //是否可移动至此方格
                            if (pieceInfo.IsMovable(inputX, inputY))
                            {
                                didLastMoveIsDrop = false;
                                isLastMoveCapture = true;                                
                                Move(inputX, inputY, hitTra, isLastMoveCapture);
                                //当王将被捕获时游戏结束
                                if (IsGameOver(hitInfo))
                                {
                                    GameOver(hitInfo);
                                }
                                //捕获棋子
                                pieceInfo.Capture(hitTra);                               
                            }
                        }
                    }                   
                }

                if (hitTra.tag == "Board")
                {
                    //是否有选中棋子
                    if (selectedPiece != null)
                    {
                        PieceInfo pieceInfo = selectedPiece.GetComponent<PieceInfo>();

                        int inputX = hitTra.GetComponent<SquareInfo>().locationX;
                        int inputY = hitTra.GetComponent<SquareInfo>().locationY;

                        //选中棋子是否在驹台
                        if (pieceInfo.isInHand)
                        {
                            //是否可打出至此方格
                            if (pieceInfo.CanDropped(inputX, inputY))
                            {
                                BoardManagement.OutPieceStand(selectedPiece);
                                didLastMoveIsDrop = true;
                                isLastMoveCapture = false;
                                Move(inputX, inputY, hitTra, isLastMoveCapture);                              
                            }
                        }
                        else
                        {
                            //是否可移动至此方格
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
