using System;
using UnityEngine;
using System.Collections.Generic;
public class MapGenerator : MonoBehaviour
{
    public int mapSizeWidth;
    public int mapSizeHeight;

    int[,] map;

    [Range(0, 100)] public int randomFillPercent;

    public string seed;

    public bool useRandomSeed;  // 랜덤으로 시드를 생성할지 여부

    public int smoothMapCount = 0;

    public GameObject prefab;

    public Material testMaterial;
       

    private void Start()
    {
        GeneratorMap();
        
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            SmoothMap();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CreateWorld();
        }*/
    }

    private void GeneratorMap()
    {    
        map = new int[mapSizeWidth, mapSizeHeight];

        RandomFillMaping();

        for ( int i = 0; i < smoothMapCount; i++ )
        {
            SmoothMap();
        }

        CreateWorld();
    }

    private void RandomFillMaping()
    {    
        for ( int x = 0; x < mapSizeWidth; x++ )
        {
            for ( int y = 0; y < mapSizeHeight; y++ )
            {
                // 맵 가장자리는 무조건 1로 막혀있도록 함.
                if ( x == 0 || x == mapSizeWidth-1 || y == 0 || y == mapSizeHeight-1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    int seedValue;

                    if (useRandomSeed)
                    {
                        seedValue = UnityEngine.Random.Range(0, 101);
                    }
                    else
                    {
                        // 시드 값으로 시스템 랜덤을 만들기 때문에 똑같은 시드값이라면 언제나 똑같은 랜덤값 추출 ( 가짜랜덤 )
                        System.Random seedMap = new System.Random(seed.GetHashCode());

                        seed = Time.time.ToString(); // 임의의 시드값 생성.
                        seedValue = seedMap.Next(0, 100);
                    }
                    map[x, y] = (seedValue < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        if ( map != null)
        {
            for (int x = 0; x < mapSizeWidth; x++)
            {
                for (int y = 0; y < mapSizeHeight; y++)
                {
                    Gizmos.color = map[x,y] == 1 ? Color.black : Color.white;
                    Vector3 createPosition = new Vector3(-mapSizeWidth / 2 + x + 0.5f, 0, -mapSizeHeight / 2 + y + 0.5f);
                    Gizmos.DrawCube(createPosition, Vector3.one);
                }
            }
        }
    }*/

    private void SmoothMap()
    {
        for (int x = 0; x < mapSizeWidth; x++)
        {
            for (int y = 0; y < mapSizeHeight; y++)
            {
                int wallCount = GetSurroundingWallCount(x, y);

                // 주변 벽이 4보다 많으면 1로 채워지고 4보다 적으면 0으로 비워진다. 4라면 값은 그대로 남는다.
                if ( wallCount > 4)
                {
                    map[x, y] = 1;
                }
                else if (wallCount < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    // 주위에 벽 (1) 이 얼마나 있는지 감지하는 함수
    private int GetSurroundingWallCount(int width, int height)
    {
        int wallCount = 0;
            
        for ( int i = width - 1; i <= (width + 1); i++)
        {
            for ( int j = height - 1; j <= (height + 1); j++)
            {
                // 주위체크를 할 때 맵의 크기를 벗어난다면 검사를 하지 않고 벽으로 취급한다.
                if ( i < 0 || mapSizeWidth <= i || j < 0 || mapSizeHeight <= j)
                {
                    wallCount++;
                }
                else if (i != width || j != height)
                {
                    // 주위에 1이 있다면 wallCount에 추가
                    wallCount += map[i, j];
                }
            }
        }

        return wallCount;
    }

    private void CreateWorld()
    {
        List<CombineInstance> combine = new List<CombineInstance>();

        for (int x = 0; x < mapSizeWidth; x++)
        {
            for (int y = 0; y < mapSizeHeight; y++)
            {
                if (map[x, y] == 0)
                {
                    Vector3 createPosition = new Vector3((-mapSizeWidth / 2  + x + 0.5f) * 10, 0, (-mapSizeHeight / 2 + y + 0.5f) * 10);

                    //Vector3 createPosition = new Vector3(x * 10, 0, y * 10);
                    GameObject go = Instantiate(prefab, transform);

                    go.transform.position = createPosition;

                    go.transform.parent = ResourceManager.Instance.parentTransform; 
                }                
            }
        }
    }

}
