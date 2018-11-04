using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ui6
{
    public interface IElementsHolderCustom : IElementsHolderGeneric<IDrawableCustomHeight> { }

    public class ElementsHolderCustom<M> : IElementsHolderCustom
        where M : IDrawableCustomHeight
    {
        List<M> elements;

        public ElementsHolderCustom(List<M> elements) {
            this.elements = elements;
        }

        public int Count { get { return elements.Count; } }

        public IDrawableCustomHeight Get(int i) {
            return elements[i];
        }
    }

    public abstract class CustomHeightListBuilder : MonoBehaviour
    {
        [NonSerialized] bool isEnabled = false;
        [NonSerialized] protected float updateInterval = 0.2f;
        [NonSerialized] bool listIsDirty = false;

        [SerializeField] protected int top;
        [SerializeField] protected int bottom;
        [SerializeField] protected int spacing;
        [SerializeField] protected int renderDistance;

        [SerializeField] Transform elementsContainer;

        protected IElementsHolderCustom holder;

        float scrollCurrentTime;
        float lastUpdatePosition = -1.0f;

        public bool IsActive {
            get { return isEnabled; }
            set {
                isEnabled = value;
                if ( isEnabled ) SetDirty();
            }
        }

        public void LinkList(IElementsHolderCustom holder) {
            this.holder = holder;
            SetDirty();
        }

        public void SetDirty() {
            listIsDirty = true;
        }

        public void Release() {
            for ( int i = 0; i < holder.Count; i++ ) {
                holder.Get(i).Release();
            }
        }

        public float GetHeightForXElementsSameHeight(int elements, float elemHeight) {
            return top + bottom + elements * elemHeight + (elements - 1) * spacing;
        }

        void Update() {
            if ( !isEnabled ) {
                return;
            }

            scrollCurrentTime -= Time.deltaTime;
            if ( listIsDirty || scrollCurrentTime <= 0) {
                scrollCurrentTime = updateInterval;
                float scrollPosition = GetScrollPosition();
                if ( !listIsDirty && Math.Abs(lastUpdatePosition - scrollPosition) < 10f ) {
                    return;
                }

                lastUpdatePosition = scrollPosition;
                RefreshElementsVisibility();
                listIsDirty = false;
            }
        }

        /// <summary>
        /// scroll top position in pixels. 
        /// </summary>
        protected abstract float GetScrollPosition();

        protected abstract float GetContentHeight();

        private void RefreshElementsVisibility() {
            float scrollPosition = GetScrollPosition();
            var minY = scrollPosition - renderDistance;
            var maxY = scrollPosition + GetContentHeight() + renderDistance;

            float y = bottom;
            bool first = true;
            for ( int i = 0; i < holder.Count; i++ ) {
                var element = holder.Get(i);
                if ( element.isFiltered ) {
                    Release(element);
                } else {
                    if ( !first ) {
                        y += spacing;
                    }

                    var elemBottom = y;
                    var elemTop = y + element.height;
                    if ( elemTop < minY || elemBottom > maxY ) {
                        Release(element);
                    } else {
                        UpdateElement(y, element);
                    }
                    y = elemTop;
                    first = false;
                }
            }
            float areaHeight = y + top;
            listIsDirty = SetAreaHeight(areaHeight);
        }

        static void Release(IDrawableCustomHeight element) {
            element.Release();
        }

        void UpdateElement(float y, IDrawableCustomHeight element) {
            if ( !element.IsViewExist ) {
                element.Create(elementsContainer);
            }
            element.SetPosition(0, y);
        }


        /// <summary>
        /// returns true if second refresh needed due to area height change or some other changes
        /// </summary>
        protected abstract bool SetAreaHeight(float areaHeight);
    }
}
