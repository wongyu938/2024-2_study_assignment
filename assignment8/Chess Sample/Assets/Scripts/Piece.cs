using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public (int, int) MyPos;
    public int PlayerDirection = 1;
    
    public Sprite WhiteSprite;
    public Sprite BlackSprite;
    
    protected GameManager MyGameManager;
    protected SpriteRenderer MySpriteRenderer;

    void Awake()
    {
        MyGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void initialize((int, int) targetPos, int direction)
    {
        PlayerDirection = direction;
        initSprite(PlayerDirection);
        MoveTo(targetPos);
    }

    void initSprite(int direction)
    {
        // direction에 따라 sprite를 설정하고 회전함
        // --- TODO ---
         if (direction == 1)
        {
            MySpriteRenderer.sprite = WhiteSprite;
        }
        else
        {
            MySpriteRenderer.sprite = BlackSprite;
            transform.rotation = Quaternion.Euler(0, 0, 180); // 아래쪽 방향으로 회전
        }
        // ------
    }

    public void MoveTo((int, int) targetPos)
    {
        // 말을 이동시킴
        // --- TODO ---
        MyPos = targetPos;
        // 보드 좌표를 월드 좌표로 변환 
        Vector3 worldPosition = Utils.ToRealPos(targetPos); // Utils.ToRealPos() 사용
        transform.position = worldPosition;
        // ------
    }

    public abstract MoveInfo[] GetMoves();
}