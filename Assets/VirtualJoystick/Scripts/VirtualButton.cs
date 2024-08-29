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
        public bool onlyOnMobile = true;
        public Color pressedColor = new Color(0.9f, 0.9f, 0.9f, 1f); // 按钮被按下时的颜色

        [Header("Events")]
        public Action OnButtonPressed; // 按钮按下时触发的事件
        public Action OnButtonReleased; // 按钮松开时触发的事件

        internal Color originalColor; // 存储按钮的原始颜色
        Canvas canvas;

        void OnEnable() {
            // 如果只在移动设备上运行且当前不是移动设备，则禁用按钮
            if (!Application.isMobilePlatform && onlyOnMobile) {
                gameObject.SetActive(false);
                return;
            }

            // 获取按钮所在的Canvas
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

        // 按下按钮时的处理
        public void OnPointerDown(PointerEventData data) {
            controlStick.color = pressedColor; // 改变颜色以指示按钮已被按下
            OnButtonPressed?.Invoke(); // 触发按下事件
        }

        // 松开按钮时的处理
        public void OnPointerUp(PointerEventData data) {
            controlStick.color = originalColor; // 恢复原始颜色
            OnButtonReleased?.Invoke(); // 触发松开事件
        }

        void Reset() {
            for (int i = 0; i < transform.childCount; i++) {
                // 找到合适的Image组件
                Image img = transform.GetChild(i).GetComponent<Image>();
                if (img) {
                    controlStick = img;
                    break;
                }
            }
        }
    }
}
