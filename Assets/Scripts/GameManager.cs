using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Obstacle
{
    None = 0, Rock = 1, Crab = 2, Mush = 3, Sea = 99
}

public enum Difficulty
{
    Easy = 2, Normal = 10, Hard = 20
}

public class GameManager : MonoBehaviour
{
    //노드 전체를 제어하기 위한 연결리스트
    List<Monkey> monkeyList = new List<Monkey>();

    public GameObject monkeyPrefab;            //tail에 추가하기 위한 prefab 설정
    public Monkey monkeyHead;                  //머리 저장
    public Monkey monkeyLast;                  //마지막 원소 따로 저장
    public Vector2 tailVec;                    //이동 전 마지막 원숭이 위치 저장

    bool canSwitchDir;                         //한 움직임 당 한 번만 방향을 바꿀 수 있도록 설정
    public int addCount;                       //한 움직임 당 하나씩 감소하면서 원숭이 추가

    public GameObject[] bananaPrefabs;
    public GameObject batPrefab;

    List<Bat> batList;

    public int Point;
    public Text PointText;
    public Text PointShadowText;

    public Obstacle infrontObstacle;               //바로 앞에 장애물이 있는 경우, true
    public bool infrontMonkey;                 //바로 앞에 원숭이들이 있는 경우, 또는, 바다가 앞에 있는 경우, true. 단, 마지막 원숭이는 제외
    bool isGameOver;                           //게임오버 시, true
    bool isConfuse;                            //박쥐 효과 적용 여부

    public float frameTime = 0.3f;             //30초마다 0.05씩 감소
    Coroutine MoveCoroutine;                   //원숭이 이동 코루틴
    Coroutine CreateCoroutine;                 //바나나 생성 코루틴

    makemaptile MapStatus;
    Touch tempTouch;
    public PauseManager PauseM;

    Difficulty nowDifficulty;

    int TileCount;                             //16타일 마다 원숭이 한 마리 씩 감소
    int StartCount;
    //instance와 Awake는 GameObject가 1개만 존재하도록 설정
    public static GameManager instance = null;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);

        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        batList = new List<Bat>();
        monkeyList.Add(monkeyHead);
        monkeyLast = monkeyList[monkeyList.Count - 1];
        canSwitchDir = true;
        MapStatus = GameObject.FindWithTag("MapGen").GetComponent<makemaptile>();

        addCount = 2;
        MoveCoroutine = StartCoroutine(Move());
        CreateCoroutine = StartCoroutine(CreateBanana());
        StartCoroutine(SpeedUp());

        nowDifficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty");

        if (nowDifficulty == Difficulty.Hard)
            frameTime = 0.15f;

        Point = 0;
        Time.timeScale = 1;
        AudioManager.instance.PlaySound("BGM");
        AudioManager.instance.PlaySound("Seagull");
    }

    //30초 마다 속도 증가
    private IEnumerator SpeedUp()
    {
        yield return new WaitForSeconds(30);
        Debug.Log("Speed Up");
        frameTime = Mathf.Max(0.05f, frameTime - 0.05f);
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if(infrontMonkey)           //꼬리에 닿았을 때
            {
                GameOver();
            }
            else if (infrontObstacle != Obstacle.None)   //장애물에 닿았을 때
            {
                LoseMonkey((int)infrontObstacle);
                canSwitchDir = true;
            }
            else                        //그 외에는 이동
            {
                if(isConfuse)
                {
                    //박쥐 먹었을 때, easy면 1.5 * 원숭이 수 * 난이도 계수
                    //그 외 난이도는 2 * 원숭이 수 * 난이도 계수
                    //난이도 계수는 각각 10, 50, 100
                    if(nowDifficulty==Difficulty.Easy)
                    {
                        Point += (int)(1.5f * monkeyList.Count * (int)nowDifficulty);
                    }
                    else
                    {
                        Point += 2 * monkeyList.Count * (int)nowDifficulty;
                    }

                    PointShadowText.text = PointText.text = Point.ToString();
                }
                else
                {
                    Point += monkeyList.Count * (int)nowDifficulty;
                    PointShadowText.text = PointText.text = Point.ToString();
                }
                tailVec.Set(monkeyLast.transform.position.x, monkeyLast.transform.position.y);
                
                if (addCount > 0)            //바나나를 먹었을 때
                {
                    foreach (Monkey tmp in monkeyList)
                        tmp.Move();

                    if(StartCount == 0)
                    {
                        StartCount = 1;
                        monkeyHead.transform.position = new Vector3(tailVec.x, tailVec.y + 1);
                    }

                    Add(tailVec);
                }
                else
                {
                    foreach (Monkey tmp in monkeyList)
                        tmp.Move();
                }


                if(TileCount < 16)
                {
                    TileCount++;
                }
                else
                {
                    Debug.Log("Starve Damage");
                    LoseMonkey(1);
                    TileCount = 0;
                }

                monkeyHead.DetectWall();

                canSwitchDir = true;

                for (int i = monkeyList.Count - 1; i > 0; i--)
                {
                    monkeyList[i].SetDir(monkeyList[i - 1]);
                }
            }

            yield return new WaitForSeconds(frameTime);
        }
    }

    private void Add(Vector2 Pos)
    {
        addCount--;
        monkeyHead.PlayAddAnimation();
        GameObject addMonkey = Instantiate(monkeyPrefab, Pos, Quaternion.identity);

        monkeyList.Add(addMonkey.GetComponent<Monkey>());
        monkeyList[monkeyList.Count - 1].SetDir(monkeyLast);
        monkeyLast = monkeyList[monkeyList.Count - 1];
    }

    //바나나의 점수 계산 함수
    public void PointPlus(int amount)
    {
        switch(nowDifficulty)
        {
            case Difficulty.Easy:
                Point += 5 * amount;
                break;
            case Difficulty.Normal:
                Point += 10 * amount;
                break;
            case Difficulty.Hard:
                Point += 20 * amount;
                break;
        }

        PointShadowText.text = PointText.text = Point.ToString();
    }

    /*
    public void Reverse()
    {
        for (int i = 0; i < monkeyList.Count - 1; i++)
            monkeyList[i].ReverseDir(monkeyList[i + 1]);

        monkeyList[monkeyList.Count - 1].ReverseDir(monkeyList[monkeyList.Count - 1]);

        monkeyList.Reverse();
        monkeyHead = monkeyList[0];
        monkeyLast = monkeyList[monkeyList.Count - 1];
    }
    */

    public void LoseMonkey(int howmany)
    {
        Debug.Log("Lose" + howmany);
        monkeyHead.PlayHitAnimation();
        AudioManager.instance.PlaySound("OnDamage");
        for (int i = 0; i < howmany; i++)
        {
            if (addCount > 0)
                addCount--;
            else
            {
                if (monkeyLast != monkeyHead)
                {
                    monkeyList.RemoveAt(monkeyList.Count - 1);
                    Destroy(monkeyLast.gameObject);
                    monkeyLast = monkeyList[monkeyList.Count - 1];
                }
                else
                {
                    GameOver();
                    break;
                }
            }
        }
    }

    public void GameOver()
    {
        if (isConfuse)
            AudioManager.instance.StopSound("BGM8bit");
        else
        {
            AudioManager.instance.StopSound("BGM");
            AudioManager.instance.StopSound("Seagull");
        }

        monkeyHead.PlayHitAnimation();
        isGameOver = true;
        AudioManager.instance.PlaySound("Fail");
        Debug.Log("game over");
        StopCoroutine(MoveCoroutine);
        StopCoroutine(CreateCoroutine);

        StartCoroutine(GameOverAfter());
    }

    IEnumerator GameOverAfter()
    {
        yield return new WaitForSeconds(1f);

        PauseM.GameOver();
        if (PlayerPrefs.GetInt("HighScore") < Point)
        {
            PlayerPrefs.SetInt("HighScore", Point);
            PauseM.HighScore();
        }
    }

    void Update()
    {

        if (canSwitchDir && !isGameOver)
        {
            if (Application.platform == RuntimePlatform.Android)
            {


                if (Input.touchCount == 1)
                {

                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        return;
                    }

                    tempTouch = Input.GetTouch(0);

                    if (tempTouch.phase == TouchPhase.Began)
                    {
                        if (tempTouch.position.x > Screen.width / 2)
                        {
                            monkeyHead.SetDir((MoveDir)((int)MoveDir.Right + (isConfuse ? 2 : 0)));
                            canSwitchDir = false;
                        }
                        else
                        {
                            monkeyHead.SetDir((MoveDir)((int)MoveDir.Left + (isConfuse ? -2 : 0)));
                            canSwitchDir = false;
                        }
                    }
                }
            }
            else
            {

            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                monkeyHead.SetDir((MoveDir)((int)MoveDir.Left + (isConfuse ? -2 : 0)));
                canSwitchDir = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                monkeyHead.SetDir((MoveDir)((int)MoveDir.Right + (isConfuse ? 2 : 0)));
                canSwitchDir = false;
            }
        }
    }

    private IEnumerator CreateBanana()
    {
        while (true)
        {
            yield return new WaitForSeconds(10 * frameTime);
            int xPos = Random.Range(0, 21);
            int yPos = Random.Range(0, 21);
            
            while(MapStatus.currentMap[xPos, yPos] != 0)
            {
                xPos = Random.Range(0, 21);
                yPos = Random.Range(0, 21);
            }

            int rand = Random.Range(0, 100);

            if(rand < 30)
                Instantiate(bananaPrefabs[0], new Vector3(xPos - 5.5f, 5.5f - yPos, 0), Quaternion.identity);
            else if (rand < 70)
                Instantiate(bananaPrefabs[1], new Vector3(xPos - 5.5f, 5.5f - yPos, 0), Quaternion.identity);
            else
                Instantiate(bananaPrefabs[2], new Vector3(xPos - 5.5f, 5.5f - yPos, 0), Quaternion.identity);
        }

    }

    public void CreateBat()
    {
        for (int i = 0; i < 2; i++)
        {
            int xPos = Random.Range(0, 21);
            int yPos = Random.Range(0, 21);

            while (MapStatus.currentMap[xPos, yPos] != 0)
            {
                xPos = Random.Range(0, 21);
                yPos = Random.Range(0, 21);
            }

            GameObject tmp = Instantiate(batPrefab, new Vector3(xPos - 5.5f, 5.5f - yPos, 0), Quaternion.identity);

            batList.Add(tmp.GetComponent<Bat>());                
        }
    }

    public void ConfuseDirection()
    {
        if (!isConfuse)
        {
            AudioManager.instance.PlaySound("BGM8bit");
            AudioManager.instance.StopSound("BGM");
            AudioManager.instance.StopSound("Seagull");
        }
        else
        {
            AudioManager.instance.PlaySound("Seagull");
            AudioManager.instance.PlaySound("BGM");
            AudioManager.instance.StopSound("BGM8bit");
        }

        isConfuse = !isConfuse;
        for (int i = batList.Count - 1; i >= 0; i--)
            Destroy(batList[i].gameObject);

        batList.Clear();
    }
}
