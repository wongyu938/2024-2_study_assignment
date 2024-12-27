using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();

    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        // moveInfo의 distance만큼 direction을 이동시키며 이동이 가능한지를 체크
        // 보드에 있는지, 다른 piece에 의해 막히는지 등을 체크
        // 폰에 대한 예외 처리를 적용
        // --- TODO ---
        int dirX = moveInfo.dirX;
        int dirY = moveInfo.dirY;
        int distance = moveInfo.distance;

        int startX = piece.MyPos.Item1;
        int startY = piece.MyPos.Item2;

        for (int step = 1; step <= distance; step++)
        {
            int x = startX + dirX * step;
            int y = startY + dirY * step;

            if (!Utils.IsInBoard((x, y))) return false; // 보드 범위를 벗어나면 이동 불가

            var blockingPiece = gameManager.Pieces[x, y];

            // 폰 이동 시 특수 규칙 처리
            if (piece is Pawn pawn)
            {
                if (dirX != 0) // 대각선 이동 (공격)
                {
                    if (blockingPiece == null || blockingPiece.PlayerDirection == piece.PlayerDirection)
                        return false; // 적 말이 없거나 아군 말이 있으면 대각선 이동 불가
                }
                else // 직진 이동
                {
                    if (blockingPiece != null) return false; // 직진 경로에 다른 말이 있으면 이동 불가
                }
            }
            else
            {
                if (blockingPiece != null)
                {
                    if (blockingPiece.PlayerDirection == piece.PlayerDirection)
                        return false; // 아군 말이 있으면 이동 불가
                    if (step == distance) return true; // 적 말이 있으면 마지막 위치에서만 이동 가능
                    return false; // 중간에 적이 있으면 이동 불가
                }
            }
        }

        return true; // 모든 조건 통과 시 이동 가능
        // ------
    }

    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false;

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }
        
        return false;
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        }

        // 왕이 지금 체크 상태인지를 리턴
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
            {
                for (int y = 0; y < Utils.FieldHeight; y++)
                {
                    var enemyPiece = gameManager.Pieces[x, y];
                    if (enemyPiece != null && enemyPiece.PlayerDirection != playerDirection)
                    {
                        if (IsValidMoveWithoutCheck(enemyPiece, (kingPos.Item1, kingPos.Item2)))
                            return true; // 왕이 공격받으면 체크 상태
                    }
                }
            }

        return false; // 체크 아님
        // ------
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // --- TODO ---
        foreach (var moveInfo in piece.GetMoves())
        {
            int dirX = moveInfo.dirX;
            int dirY = moveInfo.dirY;
            int distance = moveInfo.distance;

            for (int step = 1; step <= distance; step++)
            {
                int x = piece.MyPos.Item1 + dirX * step;
                int y = piece.MyPos.Item2 + dirY * step;

                if (!Utils.IsInBoard((x, y))) break;

                var targetPiece = gameManager.Pieces[x, y];
                if (targetPiece != null && targetPiece.PlayerDirection == piece.PlayerDirection)
                    break; // 아군 말이 있는 경우 이동 중단

                if (IsValidMove(piece, (x, y)))
                {
                    // 월드 좌표로 변환하여 효과의 위치를 설정
                    Vector3 worldPosition = Utils.ToRealPos((x, y));

                    // 효과를 생성하여 표시
                    var effectObj = Instantiate(effectPrefab, effectParent);
                    effectObj.transform.position = worldPosition;  // 수정된 부분
                    currentEffects.Add(effectObj);
                }

                if (targetPiece != null) break; // 적 말이 있으면 그 위치에서 멈춤
            }
        }
        // ------
    }

    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}