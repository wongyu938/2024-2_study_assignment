using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// King.cs
public class King : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(0, 1, 1),   // 위
            new MoveInfo(1, 1, 1),   // 오른쪽 위
            new MoveInfo(1, 0, 1),   // 오른쪽
            new MoveInfo(1, -1, 1),  // 오른쪽 아래
            new MoveInfo(0, -1, 1),  // 아래
            new MoveInfo(-1, -1, 1), // 왼쪽 아래
            new MoveInfo(-1, 0, 1),  // 왼쪽
            new MoveInfo(-1, 1, 1)   // 왼쪽 위
        };
        // ------
    }
}
