using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        List<MoveInfo> moves = new List<MoveInfo>();

        for (int i = 1; i < Utils.FieldWidth; i++)
        {
            moves.Add(new MoveInfo(i, 0, i));    // 오른쪽
            moves.Add(new MoveInfo(-i, 0, i));   // 왼쪽
            moves.Add(new MoveInfo(0, i, i));    // 위
            moves.Add(new MoveInfo(0, -i, i));   // 아래
            moves.Add(new MoveInfo(i, i, i));    // 오른쪽 위 대각선
            moves.Add(new MoveInfo(-i, i, i));   // 왼쪽 위 대각선
            moves.Add(new MoveInfo(i, -i, i));   // 오른쪽 아래 대각선
            moves.Add(new MoveInfo(-i, -i, i));  // 왼쪽 아래 대각선
        }

        return moves.ToArray();
        // ------
    }
}