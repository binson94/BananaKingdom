using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] mapPrefabs;

    [SerializeField]
    Transform gridParent;
    public int cellSize;
    public int unitInRow;

    private void Start()
    {
        MakeMap();
    }

    public void MakeMap()
    {
        int angle = Random.Range(0, 4);
        Quaternion qRotation = Quaternion.Euler(0f, 0f, 90*angle);

        for (int i = 0; i < unitInRow; i++)
        {
            for (int j = 0; j < unitInRow; j++)
            {
                GameObject map = Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], new Vector2(cellSize * i + i, -cellSize * j - j), qRotation);
                map.gameObject.transform.parent = gridParent;
            }
        }
    }

}
