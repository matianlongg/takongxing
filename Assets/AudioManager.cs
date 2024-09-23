using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // 单例模式
    private AudioSource audioSource;

    void Awake()
    {
        // 确保只存在一个 AudioManager 实例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 切换场景时不销毁
            audioSource = GetComponent<AudioSource>();
            
            // 根据保存的状态决定是否播放音乐
            if (PlayerPrefs.GetInt("MusicPlaying", 0) == 1)
            {
                PlayMusic();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 播放音乐
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            PlayerPrefs.SetInt("MusicPlaying", 1); // 保存音乐播放状态
        }
    }

    // 停止音乐
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            PlayerPrefs.SetInt("MusicPlaying", 0); // 保存音乐停止状态
        }
    }

    // 判断音乐是否正在播放
    public bool IsMusicPlaying()
    {
        return audioSource.isPlaying;
    }
}
