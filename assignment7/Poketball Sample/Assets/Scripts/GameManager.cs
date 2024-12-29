using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
{
    PlayerBall = GameObject.Find("PlayerBall");
    MyUIManager = FindObjectOfType<UIManager>();
    CamObj = GameObject.Find("Main Camera");        
}

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ShootBallTo(hit.point);
            }
        }
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 

        int ballIndex = 1; // 공 번호
        for (int row = 4; row >= 0; row--) // 삼각형 구조의 행 (위에서 아래로 배치)
        {
            for (int col = 0; col <= 4 - row; col++) // 각 행의 공 (행 번호에 맞게 공 개수 조정)
            {
                // 공 위치 계산
                // xOffset: 각 행의 공을 중앙 정렬하도록 계산
                float xOffset = (4 - row) * BallRadius + RowSpacing * (4 - row); // 행 중심 정렬
                Vector3 position = StartPosition + new Vector3(
                    (col * (BallRadius * 2 + RowSpacing)) - xOffset, // x축 간격
                    0,
                    row * (BallRadius * Mathf.Sqrt(3) + RowSpacing)); // z축 간격 (행이 내려갈수록 z값 증가)

                // 공 생성
                GameObject ball = Instantiate(BallPrefab, position, StartRotation);
                ball.name = ballIndex.ToString();

                // 공 머티리얼 적용
                ball.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/ball_{ballIndex}");
                ballIndex++;
            }
        }



        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
            Vector3 desiredPosition = PlayerBall.transform.position+ new Vector3(0, 15, 0);

        // 카메라와 PlayerBall 간의 거리를 계산
        float distance = Vector3.Distance(CamObj.transform.position, PlayerBall.transform.position);
        
        // 거리가 일정 이상이면, 카메라가 부드럽게 PlayerBall을 따라가도록 설정
        if (distance > 3f)
        {
            CamObj.transform.position = Vector3.Lerp(CamObj.transform.position, desiredPosition, Time.deltaTime * CamSpeed);
        }
        else
        {
            // 거리가 충분히 가까워지면, 카메라는 바로 PlayerBall 위치에 맞춰짐
            CamObj.transform.position = desiredPosition;
        }
            // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (targetPos - PlayerBall.transform.position).normalized;
                float power = CalcPower(targetPos - PlayerBall.transform.position);
                rb.AddForce(new Vector3(direction.x, 0, direction.z) * power, ForceMode.Impulse); // y축 제외
            }
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText($"{ballName} falls", 1.0f);
        // -------------------- 
    }
}
