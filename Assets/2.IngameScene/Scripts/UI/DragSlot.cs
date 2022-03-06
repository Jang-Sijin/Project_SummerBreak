using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KON
{
    public class DragSlot : MonoBehaviour
    {
        static public DragSlot instance;

        public Slot dragSlot;

        // 아이템 이미지
        [SerializeField] private Image imageItem;

        private void Start()
        {
            instance = this;
        }

        public void DragSetImage(Image itemImage)
        {
            imageItem.sprite = itemImage.sprite;
            SetAlphaColor(1);
        }

        public void SetAlphaColor(float alpha)
        {
            Color color = imageItem.color;
            color.a = alpha;
            imageItem.color = color;
        }
    }
}