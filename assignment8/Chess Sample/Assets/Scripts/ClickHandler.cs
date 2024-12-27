using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameManager gameManager;
    private Piece selectedPiece = null;
    private Vector3 dragOffset;
    private Vector3 originalPosition;
    private bool isDragging = false;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private (int, int) GetBoardPosition(Vector3 worldPosition)
    {
        float x = worldPosition.x + (Utils.TileSize * Utils.FieldWidth) / 2f;
        float y = worldPosition.y + (Utils.TileSize * Utils.FieldHeight) / 2f;
        
        int boardX = Mathf.FloorToInt(x / Utils.TileSize);
        int boardY = Mathf.FloorToInt(y / Utils.TileSize);
        
        return (boardX, boardY);
    }

    void HandleMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var boardPos = GetBoardPosition(mousePosition);

        if (!Utils.IsInBoard(boardPos)) return;
        // 클릭된 piece을 검증하고, 가능한 이동 경로를 표시
        // --- TODO ---
        var piece = gameManager.Pieces[boardPos.Item1, boardPos.Item2];

        if (piece != null && piece.PlayerDirection == gameManager.CurrentTurn)
        {
            selectedPiece = piece;
            originalPosition = selectedPiece.transform.position;
            dragOffset = originalPosition - mousePosition;
            isDragging = true;

            // 이동 가능한 경로 표시
            gameManager.ShowPossibleMoves(selectedPiece);
        }
        // ------
    }

    void HandleDrag()
    {
        if (selectedPiece != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            selectedPiece.transform.position = mousePosition + dragOffset;
        }
    }

    void HandleMouseUp()
    {
        if (selectedPiece != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var boardPos = GetBoardPosition(mousePosition);

            // piece의 이동을 검증하고, 이동시킴
            // effect를 초기화
            // --- TODO ---
            if (Utils.IsInBoard(boardPos) && gameManager.IsValidMove(selectedPiece, boardPos))
            {
                // 유효한 이동이라면 말 이동
                gameManager.Move(selectedPiece, boardPos);
            }
            else
            {
                // 유효하지 않다면 원래 위치로 복귀
                selectedPiece.transform.position = originalPosition;
            }

            // 이동 효과 초기화
            gameManager.ClearEffects();
            selectedPiece = null;
            isDragging = false;
        }
            // ------
    }
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            HandleDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }
}