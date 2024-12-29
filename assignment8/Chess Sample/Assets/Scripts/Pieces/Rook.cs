using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rook.cs
public class Rook : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        List<MoveInfo> moves = new List<MoveInfo>();

        for (int i = 1; i < Utils.FieldWidth; i++)
        {
            moves.Add(new MoveInfo(i, 0, i));   // 오른쪽
            moves.Add(new MoveInfo(-i, 0, i));  // 왼쪽
            moves.Add(new MoveInfo(0, i, i));   // 위
            moves.Add(new MoveInfo(0, -i, i));  // 아래
        }

        return moves.ToArray();
        // ------
    }
}
