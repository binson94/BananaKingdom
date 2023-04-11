using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDir
{
    Up = 0, Right = 1, Down = 2, Left = 3
}

//각각의 원숭이 노드를 위한 클래스
public class Monkey : MonoBehaviour
{
    //각 노드 원숭이들이 현재 보고 있는 방향
    public MoveDir nowDirection = MoveDir.Down;
    MoveDir reverseDirection = MoveDir.Up;
    Animator anim;
    public LayerMask whatToHit;
    float moveSpeed;
    int randColor = 0;

    private void Awake()
    {
        randColor = Random.Range(0, 5);
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("Color", randColor);
    }

    void Start()
    {
        moveSpeed = transform.localScale.x;
    }

    //방향키 입력으로 방향 전환을 위한 함수
    public void SetDir(MoveDir inputDir)
    {
        //input은 왼쪽, 오른쪽으로만 주어짐

        if(inputDir == MoveDir.Right)
        {
            nowDirection = (MoveDir)(((int)nowDirection + 1) % 4);
        }
        else
        {
            nowDirection = (MoveDir)(((int)nowDirection + 3) % 4);
        }

        reverseDirection = (MoveDir)((int)(nowDirection + 2) % 4);

        PlayMoveAnimation();
        DetectWall();
    }

    public void SetDir(Monkey forwardMonkey)
    {
        nowDirection = forwardMonkey.nowDirection;
        reverseDirection = (MoveDir)((int)(nowDirection + 2) % 4);

        PlayMoveAnimation();
    }

    //현재 사용하지 않는 함수
    //꼬리가 머리로, 머리가 꼬리로 바뀜
    public void ReverseDir(Monkey backwardMonkey)
    {
        nowDirection = backwardMonkey.reverseDirection;
        reverseDirection = (MoveDir)((int)(nowDirection + 2) % 4);
    }
    
    //매 주기마다 이동을 위한 함수
    public void Move()
    {
        if (!anim)
            anim = GetComponent<Animator>();

        anim.SetBool("Damaged", false);
        anim.SetBool("Added", false);   //캐릭터 Move전 트리거 false로 리셋

        if (nowDirection == MoveDir.Up)
            transform.Translate(new Vector3(0, moveSpeed, 0));
        else if (nowDirection == MoveDir.Right)
            transform.Translate(new Vector3(moveSpeed, 0, 0));
        else if (nowDirection == MoveDir.Down)
            transform.Translate(new Vector3(0, -moveSpeed, 0));
        else
            transform.Translate(new Vector3(-moveSpeed, 0, 0));
    }

    //제일 앞에 있는 애만 실행하는 함수
    //벽이 있는 지 확인
    public void DetectWall()
    {
        Vector2 origin = new Vector2();
        origin.x = transform.position.x + (nowDirection == MoveDir.Right ? 0.25f : 0) + (nowDirection == MoveDir.Left ? -0.25f : 0);
        origin.y = transform.position.y + (nowDirection == MoveDir.Up ? 0.25f : 0) + (nowDirection == MoveDir.Down ? -0.25f : 0);

        Ray2D ray = new Ray2D(origin, origin - (Vector2)transform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.3f);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.25f, whatToHit);

        if (hit)
        {
            if (hit.collider.tag == "Obstacle")  //바다에 닿는 경우
            {
                GameManager.instance.infrontObstacle = Obstacle.Sea;
                //GameManager.instance.infrontMonkey = true;  //즉사판정
                Debug.Log(hit.collider.name + " 에 부딛혔습니다!");
            }
            else if (hit.collider.tag == "Obstacle_Crab")   //게에 닿는 경우
            {
                GameManager.instance.infrontObstacle = Obstacle.Crab;
                Debug.Log(hit.collider.name + " 에 부딛혔습니다!");
            }
            else if (hit.collider.tag == "Obstacle_Mush")   //버섯에 닿는 경우
            {
                GameManager.instance.infrontObstacle = Obstacle.Mush;
                Debug.Log(hit.collider.name + " 에 부딛혔습니다!");
            }
            else if (hit.collider.tag == "Obstacle_Rock")   //돌에 닿는 경우
            {
                GameManager.instance.infrontObstacle = Obstacle.Rock;
                Debug.Log(hit.collider.name + " 에 부딛혔습니다!");
            }
            else if (hit.collider.tag == "Monkey")   //원숭이에 닿는 경우
            {
                GameManager.instance.infrontObstacle = Obstacle.None;
                if (hit.collider.GetComponent<Monkey>() != GameManager.instance.monkeyLast)
                    GameManager.instance.infrontMonkey = true;
            }
        }
        else
        {
            GameManager.instance.infrontObstacle = Obstacle.None;
        }
    }
    
    public void PlayMoveAnimation()
    {
        if (!anim)
            anim = GetComponent<Animator>();

        anim.SetInteger("Direction",(int)nowDirection);
    }

    public void PlayHitAnimation()
    {
        anim.SetBool("Damaged", true);
    }

    public void PlayAddAnimation()
    {
        anim.SetBool("Added", true);
    }
}
/*
 1. 스네이크 게임 토대
 2. 맵 생성
 3. 중립 장애물
 4. 

1. 원숭이 방향 바꾸기
2. 원숭이들이 따라서 이동

3. 오브젝트 먹었을 때 화면 돌아가는 등 효과
4. 장애물 부딛혔을 때와 시간 지나면 애들 사라짐
4-1. 사라진 애들 장애물로 생성 //선택사항
5. 간단한 UI, 신 전환 - 하이스코어, 일시정지, 환경설정
6. 맵 랜덤 생성
     */
