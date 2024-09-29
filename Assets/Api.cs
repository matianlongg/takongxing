using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;

public class Api : MonoBehaviour
{
    private string encryptionKey = "urDw32mgb9DZ5Fv3V5AvhwcM"; // 16, 24, or 32 bytes long key for AES

    public void UploadScoreToCloud(string playerName, float score)
    {
        StartCoroutine(SubmitScoreCoroutine(playerName, score));
    }

    private IEnumerator SubmitScoreCoroutine(string playerName, float score)
    {
        // 创建 JSON 数据
        ScoreData data = new ScoreData(playerName, score);
        string jsonData = JsonUtility.ToJson(data);

        // 加密数据
        string encryptedData = EncryptData(jsonData);

        // 创建上传请求
        using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:5000/submit_score", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(encryptedData);
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
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }

    private string EncryptData(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV(); // 初始化向量

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // 将IV和密文组合成最终加密数据（转换为Base64）
                byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

                return Convert.ToBase64String(result);
            }
        }
    }
}
