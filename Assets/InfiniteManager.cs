using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteManager : MonoBehaviour
{
    public List<GameObject> levelPrefabs;  // 地形块的预制件列表
    public int numberOfSegments = 2;
    public float segmentLength = 59.54f;  // 根据地形块的实际宽度调整这个值
    public int maxSavedSegments = 10;  // 最大保存的地形块数量
    public float preGenerateDistance = 20.0f;  // 提前生成的距离

    private List<GameObject> segments = new List<GameObject>();  // 当前显示的地形块
    private Stack<GameObject> usedSegments = new Stack<GameObject>();  // 保存玩家经过的地形块
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // 设置初始位置为你场景中主地形块的位置
        Vector3 startPosition = new Vector3(-28.83515f, 12.95562f, 0f);  // Z 轴根据需求调整

        // 初始化地形块
        for (int i = 0; i < numberOfSegments; i++)
        {
            // 随机选择一个预制件
            GameObject segmentPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];
            GameObject segment = Instantiate(segmentPrefab, startPosition, Quaternion.identity);
            segments.Add(segment);
            startPosition += Vector3.right * segmentLength;
        }
    }

    void Update()
    {
        // 向前跑 - 检查玩家是否接近当前最右边地形块的末端
        if (player.position.x >= segments[segments.Count - 1].transform.position.x - preGenerateDistance)
        {
            // 保存当前最左边的地形块到usedSegments栈中
            if (usedSegments.Count < maxSavedSegments)
            {
                usedSegments.Push(segments[0]);
            }
            else
            {
                Destroy(segments[0]);  // 如果超过最大保存数量，则销毁最左边的地形块
            }

            segments.RemoveAt(0);

            // 随机选择一个新的预制件
            GameObject newSegmentPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];
            Vector3 newPos = segments[segments.Count - 1].transform.position + Vector3.right * segmentLength;

            // 实例化新的地形块并添加到列表
            GameObject newSegment = Instantiate(newSegmentPrefab, newPos, Quaternion.identity);
            segments.Add(newSegment);
        }

        // 向后跑 - 检查玩家是否接近当前最左边地形块的起始端
        if (player.position.x <= segments[0].transform.position.x + preGenerateDistance)
        {
            // 如果有之前的地形块记录，从usedSegments中取出上一个地形块
            if (usedSegments.Count > 0)
            {
                // 获取上一个地形块并重新添加到segments列表
                GameObject lastSegment = usedSegments.Pop();
                Vector3 newPos = segments[0].transform.position - Vector3.right * segmentLength;

                lastSegment.transform.position = newPos;
                segments.Insert(0, lastSegment);
            }
            else
            {
                // 如果栈中没有可用的地形块，则从对象池中取出或生成新的
                GameObject newSegmentPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];
                Vector3 newPos = segments[0].transform.position - Vector3.right * segmentLength;
                GameObject newSegment = Instantiate(newSegmentPrefab, newPos, Quaternion.identity);
                segments.Insert(0, newSegment);
            }
        }
    }
}
