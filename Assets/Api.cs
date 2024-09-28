using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Api : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UploadScoreToCloud(string playerName, float score)
    {
        // 这里是上传得分的逻辑，可能涉及网络请求
        // 例如使用UnityWebRequest发送HTTP请求到你的云服务
        StartCoroutine(SubmitScoreCoroutine(playerName, score));
    }

    private IEnumerator SubmitScoreCoroutine(string playerName, float score)
    {
        // 创建 JSON 数据
        ScoreData data = new ScoreData(playerName, score);
        string jsonData = JsonUtility.ToJson(data);


        using (UnityWebRequest www = new UnityWebRequest("http://localhost:5000/submit_score", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // 发送请求并等待响应
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error submitting score: " + www.error);
            }
            else
            {
                Debug.Log("Score submitted successfully!");
                Debug.Log("Response: " + www.downloadHandler.text);  // 输出接口返回值
            }
        }
    }
}
