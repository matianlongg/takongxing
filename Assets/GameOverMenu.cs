using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public APSamplePlayer player;
    

    public void RestartGame()
    {
        player.StartCoroutine("RestartLevel");
    }

    public void QuitToMainMenu()
    {
        // 加载主菜单场景的逻辑
    }
}
