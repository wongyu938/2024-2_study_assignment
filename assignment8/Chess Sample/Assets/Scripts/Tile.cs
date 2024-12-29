using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public (int, int) MyPos;    // Tile의 좌표
    Color tileColor = new Color(255 / 255f, 193 / 255f, 204 / 255f);    // 색깔
    SpriteRenderer MySpriteRenderer;

    private void Awake()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 타일을 처음에 배치하는 함수
    public void Set((int, int) targetPos)
    {
        // MyPos를 targetPos로 지정함
        // 위치를 targetPos 이동시키고, 배치에 따라 색깔을 지정
        // --- TODO ---
        MyPos = targetPos;
        transform.position = Utils.ToRealPos(targetPos);

        bool isDarkTile = (targetPos.Item1 + targetPos.Item2) % 2 == 1;
        MySpriteRenderer.color = isDarkTile ? new Color(0.6f, 0.4f, 0.4f) : new Color(0.9f, 0.8f, 0.8f);

        // ------
    }
}
