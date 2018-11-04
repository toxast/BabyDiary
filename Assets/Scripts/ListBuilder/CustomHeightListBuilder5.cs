using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ui6
{
    public class CustomHeightListBuilder5 : CustomHeightListBuilder
    {
        [SerializeField] public ScrollRect scrollView;

        protected override float GetContentHeight() {
            return scrollView.viewport.rect.height;
        }

        protected override float GetScrollPosition() {
            return scrollView.TopOffset();
        }

        protected override bool SetAreaHeight(float areaHeight) {
            return Helper.SetAreaHeight(areaHeight, scrollView, scrollView.content, scrollView.transform as RectTransform);
        }
    }
}