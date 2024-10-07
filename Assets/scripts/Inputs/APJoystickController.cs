using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class APJoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject joystick; // 摇杆的父对象
    private bool isDragging = false;
    private OnScreenStick onScreenStick;

    void Start()
    {
        joystick.SetActive(false); // 初始隐藏
        onScreenStick = joystick.GetComponent<OnScreenStick>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        joystick.SetActive(true);
        onScreenStick.OnPointerDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            onScreenStick.OnDrag(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        joystick.SetActive(false);
        onScreenStick.OnPointerUp(eventData);
    }
}