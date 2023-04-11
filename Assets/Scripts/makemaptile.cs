﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makemaptile : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectPrefabs;

    [SerializeField]
    GameObject[] rockPrefabs;

    public Transform objRoot;


    int[,,] maps;
    int[,] easyMap;

    public int[,] currentMap;

    Difficulty nowDifficulty;

    void Start()
    {

        nowDifficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty");
        Debug.Log("Current Difficulty : " + nowDifficulty);

        currentMap = new int[21, 21];

        easyMap = new int[21, 21]
        {
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,1,0,0,0,0,0 },
            { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0 },
            { 0,2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
            { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0 },
            { 0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0 },
            { 0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,2,0 }
        };



        maps = new int[19, 10, 10] {
            {
                {0,0,0,0,1,0,0,0,0,0},
                { 0,0,0,0,0,0,0,3,0,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,1,0,0,0,0 },
                { 0,2,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,1,0,0 },
                { 0,0,2,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,1,0,0,0,1,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
            },
            {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,1,1,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,2,0},
                { 1,0,0,0,1,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,3,0,0,0,0,0,0},
                { 0,0,0,0,0,0,1,0,0,0},
                { 0,1,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,1,0,0}
            },
            {
                { 0,1,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,1,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,2,0,0,0,0,0,3,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,1,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,2,0,0}
            },
            {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,1,0,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,1,0,0,0},
                { 0,0,0,1,0,0,0,0,1,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,3,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,2,0,0,0},
                { 0,1,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0}
            },
            {
                { 0,0,0,0,0,0,2,0,0,0},
                { 0,0,0,0,1,0,0,0,0,0},
                { 0,2,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,2,0},
                { 1,0,0,0,3,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,1,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,1,0,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,1}
            },
            {
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,2,0,0,0,1,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,3,0,0,1},
                { 0,1,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,1,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,2,0,0}
            },
            {
                { 0,0,0,0,0,0,2,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,1,0,0,0,0,1,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,1,0,0,0,0,1,0},
                { 1,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,3,0,0,0,0},
                { 0,0,0,0,0,0,0,0,1,0},
                { 0,1,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,2,0}
            },
            {
                { 0,1,0,0,0,0,0,0,0,0},
                { 0,0,0,0,1,0,0,0,1,0},
                { 0,2,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,2,0},
                { 1,0,0,0,3,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,1,0,0,0,0,0,0},
                { 0,0,0,0,0,0,1,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,1,0,0,0,0,0,0,0}
            },
            {
                { 0,0,0,0,0,0,0,0,0,1 },
                { 0,1,0,0,0,1,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,2,0,0,0,0,0,0,0 },
                { 0,0,0,0,1,0,0,0,0,1 },
                { 0,3,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,1,0,0,0,0,0,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,2,0,0 }
             },
            {
                { 0,0,0,0,0,0,0,1,0,0 },
                { 0,0,0,2,0,0,0,0,0,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,1,0,0,0 },
                { 0,0,0,0,0,0,0,0,1,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,3,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,1 },
                { 0,1,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,2,0 }
            },
            {
                {0,0,0,0,0,0,2,0,3,0},
                {0,0,1,0,2,0,0,0,1,0},
                {0,0,0,0,3,0,1,0,1,0},
                {0,2,0,0,0,0,0,0,0,0},
                {0,2,0,3,0,1,0,0,0,0},
                {0,0,0,2,0,0,0,3,0,2},
                {2,0,0,0,0,1,1,0,1,0},
                {0,0,0,1,0,0,0,0,0,0},
                {0,0,1,0,0,3,0,0,0,0},
                {0,0,0,0,0,0,0,1,0,2}
            },
            {
                {2,0,0,0,3,0,3,0,0,0},
                {0,0,1,0,0,0,0,0,0,0},
                {0,2,1,0,0,2,0,2,1,0},
                {0,0,0,0,1,1,0,0,0,0},
                {3,0,2,0,0,0,0,3,0,3},
                {0,0,1,0,0,2,0,0,0,0},
                {0,0,0,0,3,0,0,1,2,0},
                {0,0,0,0,0,0,1,0,0,0},
                {0,1,0,0,1,0,0,0,2,0},
                {3,0,0,0,0,3,0,1,0,0}
            },
            {
                {0,2,0,0,1,0,2,0,0,0},
                {0,0,0,0,0,0,0,1,3,0},
                {0,1,3,0,3,0,0,0,0,1},
                {2,0,0,2,0,1,0,0,0,0},
                {0,1,0,0,0,0,0,1,3,0},
                {0,0,3,0,1,0,3,0,0,0},
                {2,0,0,0,1,0,0,0,2,0},
                {0,0,2,0,0,0,2,0,1,0},
                {0,3,1,0,3,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,1,0}
            },
            {
                {0,1,3,0,1,1,0,0,0,0},
                {0,0,0,0,2,0,0,3,0,2},
                {0,2,0,0,0,0,0,0,0,0},
                {0,0,1,0,0,0,2,0,1,0},
                {0,2,0,0,3,1,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,1,0,2,0,0,0},
                {0,0,1,2,0,0,0,0,3,0},
                {0,0,1,0,0,3,0,0,0,3},
                {1,0,0,0,0,0,0,1,0,0}
            },
            {
                {0, 0, 0, 2, 0, 0, 2, 0, 0, 1},
                {0, 1, 0, 2, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {2, 0, 0, 0, 0, 0, 2, 0, 2, 0},
                {0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
                {0, 0, 0, 2, 2, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 2, 2},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 2, 1, 2, 0, 0},
                {0, 0, 1, 1, 0, 0, 0, 0, 0, 0}
            },
            {
                {1, 0, 0, 1, 1, 1, 1, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 2, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 3, 0},
                {0, 0, 0, 0, 3, 0, 0, 0, 3, 0},
                {1, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
                {0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
                {1, 1, 0, 0, 1, 1, 0, 0, 1, 0}
            },
            {
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
                { 1, 3, 2, 3, 1, 2, 1, 3, 2, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 3, 2, 3, 1, 1, 3, 2, 2},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 2, 1, 1, 2, 3, 3, 2, 0, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 3, 3},
                { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 2, 3, 1, 2, 3, 1, 2, 1, 3, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
            },
            {
                {0,0,0,0,0,0,0,0,1,0 },
                {0,0,0,0,0,0,0,0,0,0 },
                {0,0,1,1,0,0,1,1,0,0 },
                {1,0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0,0 },
                { 0,1,0,0,1,0,0,1,0,0},
                { 0,1,0,0,1,0,0,1,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                {0,0,1,0,0,1,0,0,0,1 },
                {0,0,1,0,0,1,0,0,0,1 }
            },{
                { 3,3,0,0,0,0,3,0,0,0},
                { 0,0,0,0,3,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,3,3},
                { 0,1,0,0,0,3,0,0,0,3},
                { 0,1,0,0,0,0,3,0,0,1},
                { 0,0,0,0,1,0,0,0,0,0},
                { 3,0,0,0,0,3,0,0,0,0},
                { 0,0,0,3,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,3,0,0},
                { 0,3,0,0,3,0,0,1,1,0}
            }
        };


        GenerateMapArray();
        InitiateMap();
    }

    public int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }

    public void GenerateMapArray()
    {
        int[] mapindex;
        if (nowDifficulty == Difficulty.Normal)
        {
            mapindex = GetRandomInt(4, 0, 10);
        }else
        {
            mapindex = GetRandomInt(4, 0, 19);
        }

        for (int i = 0; i < 4; i++)
        {
            int x = 0;
            int y = 0;
            if (i == 1 || i == 3)
            {
                x = 11;
            }

            if (i == 2 || i == 3)
            {
                y = 11;
            }
            Debug.Log((mapindex[i]+1).ToString() + " 번째 맵 생성됨");
            CopyMap(mapindex[i], Random.Range(0, 4), x, y);

        }
    }

    void InitiateMap()
    {
        for(int i = 0; i < 21; i++)
        {
            for(int j = 0; j < 21; j++)
            {
                if (currentMap[i, j].Equals(0))
                    continue;

                if (currentMap[i, j].Equals(1))
                {
                    GameObject obj = Instantiate(rockPrefabs[Random.Range(0,3)], new Vector2(i - 5.5f, -j + 5.5f), Quaternion.identity);
                    obj.transform.parent = objRoot;
                }
                else
                {
                    GameObject obj = Instantiate(objectPrefabs[currentMap[i, j]], new Vector2(i - 5.5f, -j + 5.5f), Quaternion.identity);
                    obj.transform.parent = objRoot;
                }
             }
        }

    }

    void CopyMap(int index, int angle,int x, int y )
    {
        if (nowDifficulty == Difficulty.Easy)
        {
            currentMap = easyMap;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (angle >= 2)
                    {
                        currentMap[i + x, j + y] = maps[index, i, j];
                    }
                    else
                    {
                        currentMap[i + x, j + y] = maps[index, j, i];
                    }
                }
            }

            for (int i = 0; i < 21; i++)
            {
                currentMap[i, 10] = 0;
                currentMap[10, i] = 0;
            }
        }
    }
}
