using UnityEngine;
using UnityEngine.InputSystem;

public class APInputSystemPlugin : APInputJoystickPlugin
{
    private PlayerInputControl playerInputControl;

    // 将初始化逻辑移到 Awake 中
    private void Awake()
    {
        playerInputControl = new PlayerInputControl();
        playerInputControl.Enable();
    }

    public override float GetAxisX()
    {
        return playerInputControl.Player.Move.ReadValue<Vector2>().x;
    }

    public override float GetAxisY()
    {
        return playerInputControl.Player.Move.ReadValue<Vector2>().y;
    }
}
