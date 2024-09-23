using TMPro;  // TextMeshPro 的引用
using UnityEngine;
using UnityEngine.UI;

public class MusicToggleButton : MonoBehaviour
{
    private Button button;
    private TMP_Text buttonText;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TMP_Text>();

        // 根据音乐状态初始化按钮文字
        if (PlayerPrefs.GetInt("MusicPlaying", 0) == 1)
        {
            buttonText.text = "音乐：开";
        }
        else
        {
            buttonText.text = "音乐：关";
        }

        button.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        // 切换音乐播放状态
        if (AudioManager.instance.IsMusicPlaying())
        {
            AudioManager.instance.StopMusic();
            buttonText.text = "音乐：关";
        }
        else
        {
            AudioManager.instance.PlayMusic();
            buttonText.text = "音乐：开";
        }
    }
}
