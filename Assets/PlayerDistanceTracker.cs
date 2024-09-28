using UnityEngine;
using TMPro;  // 引入 TextMeshPro 命名空间

public class PlayerDistanceTracker : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;  // 添加一个公共变量引用 TextMeshPro UI 组件
    private float initialXPosition;
    private float maxReachedXPosition;
    public float distanceTraveled;

    void Start()
    {
        initialXPosition = player.position.x;
        maxReachedXPosition = initialXPosition;
        if (distanceText == null)
        {
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        float currentPlayerXPosition = player.position.x;
        
        if (currentPlayerXPosition > maxReachedXPosition)
        {
            distanceTraveled += currentPlayerXPosition - maxReachedXPosition;
            maxReachedXPosition = currentPlayerXPosition;
        }
        
        // 更新 UI 文本显示距离
        distanceText.text = $"{distanceTraveled:N2} 米";  // N2 格式化为显示两位小数
        // PlayerPrefs.SetFloat("MaxDistance", distanceTraveled);
        // PlayerPrefs.Save(); // 确保数据被保存到设备上
    }
}
