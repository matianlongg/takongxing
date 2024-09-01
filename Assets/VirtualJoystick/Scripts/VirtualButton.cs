using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Terresquall {

    [System.Serializable]
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        public Image controlStick; // 按钮的图片引用

        [Header("Settings")]
        public bool onlyOnMobile = false;
        public Color pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f); // 按钮被按下时的颜色

        [Header("Events")]
        public Action OnButtonPressed; // 按钮按下时触发的事件
        public Action OnButtonReleased; // 按钮松开时触发的事件
        public Action OnButtonHeld; // 按钮按住时持续触发的事件

        // private bool isHeld = false; // 按钮是否被按住的状态

        public bool isHeld { get; private set; }

        internal Color originalColor; // 存储按钮的原始颜色
        Canvas canvas;

        void OnEnable() {
            if (!Application.isMobilePlatform && onlyOnMobile) {
                gameObject.SetActive(false);
                return;
            }

            canvas = GetComponentInParent<Canvas>();
            if (!canvas) {
                Debug.LogError(
                    string.Format("您的虚拟按钮 ({0}) 未附加到Canvas，因此无法工作。它已被禁用。", name),
                    gameObject
                );
                enabled = false;
            }

            originalColor = controlStick.color; // 存储按钮的原始颜色
        }

        // public bool IsHeld
        // {
        //     get { return isHeld; }
        // }

        public void OnPointerDown(PointerEventData data) {
            controlStick.color = pressedColor; // 改变颜色以指示按钮已被按下
            isHeld = true; // 设置为按住状态
            OnButtonPressed?.Invoke(); // 触发按下事件
        }

        public void OnPointerUp(PointerEventData data) {
            controlStick.color = originalColor; // 恢复原始颜色
            isHeld = false; // 取消按住状态
            OnButtonReleased?.Invoke(); // 触发松开事件
        }

        void Update() {
            if (isHeld) {
                OnButtonHeld?.Invoke(); // 持续触发按住事件
            }
        }

        void Reset() {
            for (int i = 0; i < transform.childCount; i++) {
                Image img = transform.GetChild(i).GetComponent<Image>();
                if (img) {
                    controlStick = img;
                    break;
                }
            }
        }
    }
}
