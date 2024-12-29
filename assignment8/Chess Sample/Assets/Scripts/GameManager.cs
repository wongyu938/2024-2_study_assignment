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
                GameObject tileObj = Instantiate(TilePrefab, TileParent);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Set((x, y));
                Tiles[x, y] = tile;
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
        int backRow = direction == 1 ? 0 : 7;

        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            PlacePiece(5, (x, pawnRow), direction);
        }

        int[] pieceOrder = { 4, 3, 2, 1, 0, 2, 3, 4 };
        for (int x = 0; x < pieceOrder.Length; x++)
        {
            PlacePiece(pieceOrder[x], (x, backRow), direction);
        }
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // Piece를 배치 후, initialize
        // PiecePrefabs의 원소를 사용하여 배치, PieceParent의 자식으로 생성
        // Pieces를 채움
        // 배치한 Piece를 리턴
        // --- TODO ---
        GameObject pieceObj = Instantiate(PiecePrefabs[pieceType], PieceParent);
        Piece piece = pieceObj.GetComponent<Piece>();
        piece.initialize(pos, direction);
        Pieces[pos.Item1, pos.Item2] = piece;
        SpriteRenderer pieceSpriteRenderer = pieceObj.GetComponent<SpriteRenderer>();
        pieceSpriteRenderer.sortingLayerName = "Pieces";  // "Pieces"라는 Sorting Layer로 설정
        pieceSpriteRenderer.sortingOrder = 1;  // Piece는 Tiles보다 높은 Order로 설정
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
        Piece targetPiece = Pieces[targetPos.Item1, targetPos.Item2];

        if (targetPiece != null)
        {
            // 상대방 말을 삭제
            Destroy(targetPiece.gameObject);
        }

        // Piece를 이동시킴
        Pieces[piece.MyPos.Item1, piece.MyPos.Item2] = null;  // 기존 위치의 Piece 제거
        piece.MyPos = targetPos;  // 새로운 위치로 이동
        Pieces[targetPos.Item1, targetPos.Item2] = piece;  // 새 위치에 Piece 추가

        ChangeTurn();  // 턴 변경
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        bool isCheck = movementManager.IsInCheck(CurrentTurn);

    // 턴을 변경
    CurrentTurn *= -1; // 턴 전환

    // UI 업데이트 (턴 정보)
    uiManager.UpdateTurn(CurrentTurn);

    // 체크 상태가 있다면 UI에 체크 메시지 출력
    if (isCheck)
    {
        uiManager.ShowCheck(); // 체크 상태 출력
    }

    // 체크메이트 상태 확인
    bool isCheckmate = movementManager.IsInCheck(CurrentTurn);

    // 체크메이트 상태가 있다면 UI에 체크메이트 메시지 출력
    if (isCheckmate)
    {
        uiManager.ShowCheckmate(CurrentTurn); // 체크메이트 상태 출력
    }
        // ------
    }
}
