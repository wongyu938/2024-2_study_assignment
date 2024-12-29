using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 프리팹들
    public GameObject TilePrefab;
    public GameObject[] PiecePrefabs;   // King, Queen, Bishop, Knight, Rook, Pawn 순
    public GameObject EffectPrefab;

    // 오브젝트의 parent들
    private Transform TileParent;
    private Transform PieceParent;
    private Transform EffectParent;
    
    private MovementManager movementManager;
    private UIManager uiManager;
    
    public int CurrentTurn = 1; // 현재 턴 1 - 백, 2 - 흑
    public Tile[,] Tiles = new Tile[Utils.FieldWidth, Utils.FieldHeight];   // Tile들
    public Piece[,] Pieces = new Piece[Utils.FieldWidth, Utils.FieldHeight];    // Piece들

    void Awake()
    {
        TileParent = GameObject.Find("TileParent").transform;
        PieceParent = GameObject.Find("PieceParent").transform;
        EffectParent = GameObject.Find("EffectParent").transform;
        
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        movementManager = gameObject.AddComponent<MovementManager>();
        movementManager.Initialize(this, EffectPrefab, EffectParent);
        
        InitializeBoard();
    }

    void InitializeBoard()
    {
        // 8x8로 타일들을 배치
        // TilePrefab을 TileParent의 자식으로 생성하고, 배치함
        // Tiles를 채움
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                GameObject tile = Instantiate(TilePrefab, new Vector3(x * Utils.TileSize, y * Utils.TileSize, 0), Quaternion.identity);
                tile.transform.SetParent(TileParent);
                Tiles[x, y] = tile.GetComponent<Tile>();
                Tiles[x, y].MyPos = (x, y);
                Tiles[x, y].Set((x, y));
            }
        }

        // 체스 말 배치
        // ------

        PlacePieces(1);
        PlacePieces(-1);
    }

    void PlacePieces(int direction)
    {
        // PlacePiece를 사용하여 Piece들을 적절한 모양으로 배치
        // --- TODO ---
        int pawnRow = direction == 1 ? 1 : 6;
        int mainRow = direction == 1 ? 0 : 7;

        // Pawn 배치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            PlacePiece(0, (x, pawnRow), direction); // 0: Pawn
        }

        // 주요 말 배치
        PlacePiece(1, (0, mainRow), direction); // 1: Rook
        PlacePiece(1, (7, mainRow), direction);
        PlacePiece(2, (1, mainRow), direction); // 2: Knight
        PlacePiece(2, (6, mainRow), direction);
        PlacePiece(3, (2, mainRow), direction); // 3: Bishop
        PlacePiece(3, (5, mainRow), direction);
        PlacePiece(4, (3, mainRow), direction); // 4: Queen
        PlacePiece(5, (4, mainRow), direction); // 5: King (한 번만 호출)
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // Piece를 배치 후, initialize
        // PiecePrefabs의 원소를 사용하여 배치, PieceParent의 자식으로 생성
        // Pieces를 채움
        // 배치한 Piece를 리턴
        // --- TODO ---
        Vector3 realPos = Utils.ToRealPos(pos);
        realPos -= new Vector3(
            Utils.FieldWidth * Utils.TileSize / 2f - Utils.TileSize / 2f,
            Utils.FieldHeight * Utils.TileSize / 2f - Utils.TileSize / 2f,
            0
        );

        GameObject pieceObj = Instantiate(PiecePrefabs[pieceType], PieceParent);
        pieceObj.transform.position = new Vector3(realPos.x, realPos.y, 1); // 타일 중심에 위치

        Piece piece = pieceObj.GetComponent<Piece>();
        piece.initialize(pos, direction);
        Pieces[pos.Item1, pos.Item2] = piece;

        return piece;
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        return movementManager.IsValidMove(piece, targetPos);
    }

    public void ShowPossibleMoves(Piece piece)
    {
        movementManager.ShowPossibleMoves(piece);
    }

    public void ClearEffects()
    {
        movementManager.ClearEffects();
    }


    public void Move(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMove(piece, targetPos)) return;
        
        // 해당 위치에 다른 Piece가 있다면 삭제
        // Piece를 이동시킴
        // --- TODO ---
        (int oldX, int oldY) = piece.MyPos;
        Pieces[oldX, oldY] = null;

        // 타겟 위치에 말이 있으면 제거
        Piece targetPiece = Pieces[targetPos.Item1, targetPos.Item2];
        if (targetPiece != null)
        {
            Destroy(targetPiece.gameObject);
        }

        // 말 이동
        piece.MoveTo(targetPos);
        Pieces[targetPos.Item1, targetPos.Item2] = piece;

        // 턴 변경
        ChangeTurn();
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        CurrentTurn *= -1; // 턴 전환
        uiManager.UpdateTurn(CurrentTurn); // UI 업데이트
        // ------
    }
}
