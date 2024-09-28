using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankItemManager : MonoBehaviour
{
    public GameObject rankItemPrefab; // 这是 RankItem 的预制件
    public Transform gridTransform;   // 这是 Grid 的 Transform
    public RectTransform contentRectTransform; // 这是 Content 的 RectTransform
    public float itemHeight = 100f;

    // 方法来动态添加一个新的 RankItem
    public void AddRankItem(int rank, string name, float score)
    {
        // 生成一个新的 RankItem 实例
        GameObject newRankItem = Instantiate(rankItemPrefab, gridTransform);

        // 查找 RankItem 内的 Rank, Name, Score 子对象
        Text rankText = newRankItem.transform.Find("Rank_RankItem").GetComponent<Text>();
        Text nameText = newRankItem.transform.Find("Name_RankItem").GetComponent<Text>();
        Text scoreText = newRankItem.transform.Find("Score_RankItem").GetComponent<Text>();

        // 修改文本内容
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();

        UpdateContentHeight();
    }

    private void UpdateContentHeight()
    {
        // 获取 Grid Layout Group 的 Cell Size
        GridLayoutGroup gridLayoutGroup = gridTransform.GetComponent<GridLayoutGroup>();

        // 获取当前子对象数量
        int itemCount = gridTransform.childCount;

        // 计算新的 Content 高度
        float newHeight = itemCount * gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y * (itemCount - 1);

        // 更新 Content 的 RectTransform 高度
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, newHeight);
    }

    // 调整 Content 高度的方法
    // private void UpdateContentHeight()
    // {
    //     // 计算 Grid 下 RankItem 的数量
    //     int itemCount = gridTransform.childCount;

    //     // 计算新的 Content 高度
    //     float newHeight = itemCount * itemHeight;

    //     // 设置新的 Content 高度
    //     contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, newHeight);
    // }



    void Start()
    {
        StartCoroutine(GetScoresCoroutine());
        // 读取最大距离
        // float maxDistance = PlayerPrefs.GetFloat("MaxDistance", 0f);

        // 添加到排行榜
        // AddRankItem(1, "Player2", (int)maxDistance);


        // // 假设我们要初始化一些数据
        // AddRankItem(1, "Player1", 100);
        // AddRankItem(2, "Player2", 95);
        // AddRankItem(3, "Player3", 90);

        // for (int i = 1; i <= 20; i++)
        // {
        //     // 生成随机的分数，假设在 50 到 100 之间
        //     int randomScore = Random.Range(500, 1000);

        //     // 生成名字，假设玩家名字格式是 "Player" + i
        //     string playerName = "Player" + i;

        //     // 添加排行榜条目
        //     AddRankItem(i, playerName, randomScore);
        // }

    }

    private IEnumerator GetScoresCoroutine()
    {
        Debug.Log("get_scores");
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/get_scores"))
        {
            // 发送请求并等待响应
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error getting scores: " + www.error);
            }
            else
            {
                Debug.Log("Scores retrieved successfully!");

                // 解析返回的 JSON 数据
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                var wrappedJson = WrapJsonArray(jsonResponse);


                // 如果需要，可以将 JSON 反序列化为对象
                // ScoreData[] scores = JsonUtility.FromJson<ScoreDataList>(jsonResponse).scores;
                var scores = JsonUtility.FromJson<ScoreDataList>(wrappedJson);
                for (int i = 0; i < scores.scores.Length; i++)
                {
                    var score = scores.scores[i];
                    Debug.Log($"Player: {score.player_name}, Score: {score.score}, Created At: {score.created_at}, Updated At: {score.updated_at}");
                    // 调用 AddRankItem，使用动态排名
                    AddRankItem(i + 1, score.player_name, score.score);
                }
                // foreach (var score in scores)
                // {
                //     Debug.Log($"Player: {score.player_name}, Score: {score.score}, Created At: {score.created_at}, Updated At: {score.updated_at}");
                // }
            }
        }
    }

    // 包装 JSON 数组为对象
    private string WrapJsonArray(string jsonArray)
    {
        return "{\"scores\":" + jsonArray + "}";
    }
}
