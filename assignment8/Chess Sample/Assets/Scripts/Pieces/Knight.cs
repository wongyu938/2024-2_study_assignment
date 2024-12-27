using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Knight.cs
public class Knight : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(2, 1, 1),   // 오른쪽 위
            new MoveInfo(2, -1, 1),  // 오른쪽 아래
            new MoveInfo(-2, 1, 1),  // 왼쪽 위
            new MoveInfo(-2, -1, 1), // 왼쪽 아래
            new MoveInfo(1, 2, 1),   // 위 오른쪽
            new MoveInfo(1, -2, 1),  // 아래 오른쪽
            new MoveInfo(-1, 2, 1),  // 위 왼쪽
            new MoveInfo(-1, -2, 1)  // 아래 왼쪽
        };
        // ------
    }
}