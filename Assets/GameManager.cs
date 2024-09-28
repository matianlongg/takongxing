using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float distanceRan;
    public string playerName;
    public TMP_InputField nameInputField;
    public Api api; // 引用API脚本

    public PlayerDistanceTracker playerDistanceTracker;

    public void RecordDistance()
    {
        distanceRan = playerDistanceTracker.distanceTraveled;
    }

    public void RecordPlayerName()
    {
        playerName = nameInputField.text;
    }

    public void SendScoreToWeb() {
        RecordDistance();
        RecordPlayerName();
        api.UploadScoreToCloud(playerName, distanceRan);
    }
}
