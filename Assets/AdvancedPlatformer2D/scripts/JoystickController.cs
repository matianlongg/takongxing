using Terresquall;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public GameObject joystick;  // 虚拟摇杆的GameObject
    private VirtualJoystick virtualJoystick;
    private RectTransform joystickRectTransform;

    void Start()
    {
        joystickRectTransform = joystick.GetComponent<RectTransform>();
        virtualJoystick = joystick.GetComponent<VirtualJoystick>();
        joystick.SetActive(false);  // 初始时隐藏虚拟摇杆
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 在触摸位置显示虚拟摇杆
                Vector2 touchPosition = touch.position;
                joystickRectTransform.position = touchPosition;
                joystick.SetActive(true);

                // 让虚拟摇杆捕捉输入事件
                virtualJoystick.OnPointerDown(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current) { position = touchPosition });
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // 当触摸结束时隐藏虚拟摇杆
                joystick.SetActive(false);
                virtualJoystick.OnPointerUp(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 在点击位置显示虚拟摇杆（用于鼠标点击测试）
                Vector2 clickPosition = Input.mousePosition;
                joystickRectTransform.position = clickPosition;
                joystick.SetActive(true);

                // 让虚拟摇杆捕捉输入事件
                virtualJoystick.OnPointerDown(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current) { position = clickPosition });
            }

            if (Input.GetMouseButtonUp(0))
            {
                // 当点击结束时隐藏虚拟摇杆
                joystick.SetActive(false);
                virtualJoystick.OnPointerUp(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
            }
        }
    }
}
