using UnityEngine;
using UnityEngine.EventSystems;
using Terresquall;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject joystick;  // 将 Joystick 子元素拖拽到该变量中
    private VirtualJoystick virtualJoystick;

    void Start()
    {
        if (joystick != null)
        {
            joystick.SetActive(false);
            virtualJoystick = joystick.GetComponent<VirtualJoystick>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (joystick != null)
        {
            joystick.SetActive(true);
            joystick.transform.position = eventData.position;  // 设置摇杆位置到触摸点
            virtualJoystick.OnPointerDown(eventData);  // 传递事件到 VirtualJoystick
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystick != null)
        {
            virtualJoystick.OnDrag(eventData);  // 传递拖动事件到 VirtualJoystick
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (joystick != null)
        {
            joystick.SetActive(false);
            virtualJoystick.OnPointerUp(eventData);  // 结束摇杆控制
        }
    }
}
