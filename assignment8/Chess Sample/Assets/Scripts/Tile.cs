using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public (int, int) MyPos;
    Color tileColor = new Color(255 / 255f, 193 / 255f, 204 / 255f);
    SpriteRenderer MySpriteRenderer;

    private void Awake()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Set((int, int) targetPos)
    {
        // targetPos로 이동시키고, 색깔을 지정
        // --- TODO ---
        MyPos = targetPos;

        // 월드 좌표로 변환
        float x = (targetPos.Item1 - Utils.FieldWidth / 2f) * Utils.TileSize;
        float y = (targetPos.Item2 - Utils.FieldHeight / 2f) * Utils.TileSize;
        transform.position = new Vector3(x, y, 0);

        // 색상을 설정
        MySpriteRenderer.color = (MyPos.Item1 + MyPos.Item2) % 2 == 0 ? Color.white : tileColor;
        // ------
    }
}
