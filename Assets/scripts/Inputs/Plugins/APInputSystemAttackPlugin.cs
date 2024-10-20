using UnityEngine;
using UnityEngine.InputSystem;

public class APInputSystemAttackPlugin : APInputButtonPlugin
{
    private PlayerInputControl playerInputControl;

    // 将初始化逻辑移到 Awake 中
    private void Awake()
    {
        playerInputControl = new PlayerInputControl();
        playerInputControl.Enable();
    }
    // 添加点击
        // 添加点击按钮的功能
    public override bool GetButton()
    {
        return playerInputControl.Player.Fire.triggered;
    }

    public override bool GetButtonDown()
    {
        return playerInputControl.Player.Fire.WasPressedThisFrame();
    }

    public override bool GetButtonUp()
    {
        return playerInputControl.Player.Fire.WasReleasedThisFrame();
    }
}
