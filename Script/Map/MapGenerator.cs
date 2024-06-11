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

    public bool useRandomSeed;  // �������� �õ带 �������� ����

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
                // �� �����ڸ��� ������ 1�� �����ֵ��� ��.
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
                        // �õ� ������ �ý��� ������ ����� ������ �Ȱ��� �õ尪�̶�� ������ �Ȱ��� ������ ���� ( ��¥���� )
                        System.Random seedMap = new System.Random(seed.GetHashCode());

                        seed = Time.time.ToString(); // ������ �õ尪 ����.
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

                // �ֺ� ���� 4���� ������ 1�� ä������ 4���� ������ 0���� �������. 4��� ���� �״�� ���´�.
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

    // ������ �� (1) �� �󸶳� �ִ��� �����ϴ� �Լ�
    private int GetSurroundingWallCount(int width, int height)
    {
        int wallCount = 0;
            
        for ( int i = width - 1; i <= (width + 1); i++)
        {
            for ( int j = height - 1; j <= (height + 1); j++)
            {
                // ����üũ�� �� �� ���� ũ�⸦ ����ٸ� �˻縦 ���� �ʰ� ������ ����Ѵ�.
                if ( i < 0 || mapSizeWidth <= i || j < 0 || mapSizeHeight <= j)
                {
                    wallCount++;
                }
                else if (i != width || j != height)
                {
                    // ������ 1�� �ִٸ� wallCount�� �߰�
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
