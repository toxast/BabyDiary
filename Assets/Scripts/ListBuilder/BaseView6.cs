using System;
using System.Collections.Generic;
using UnityEngine;

namespace ui6 {
    public abstract class BaseView : MonoBehaviour, IViewPrefab {

        protected abstract bool ChangeParentOnRecycle { get; }

        public GameObject GAME_OBJ { get { return gameObject; } }
        public virtual void BeforeRelease() { }

        public void CallRecycle() {
            GAME_OBJ.Recycle(ChangeParentOnRecycle);
        }

        public abstract void SetPosition(float x, float y);
    }

    [System.Serializable]
    public abstract class BaseView5 : BaseView
    {
        [SerializeField] protected RectTransform thisContainer;

        protected override bool ChangeParentOnRecycle { get { return false; } }

        public override void SetPosition(float x, float y) {
            thisContainer.anchoredPosition = new Vector2(x, -y);
        }
    }


    public interface IViewPrefab {
        GameObject GAME_OBJ { get; }
        void SetPosition(float x, float y);
        void BeforeRelease();
        void CallRecycle();
    }

	public interface IHasBoolState {
		bool IsOn{get;}
	}

	public interface ITogglable : IHasBoolState{ 
        void SetState(bool on);
    }

    public interface IHasMultiSelectedState
    {
        bool IsMultiSelected { get; }
    }


    public interface IClickable {
        event Action OnClick;
    }

    public interface IDoubleClickable {
        event Action OnDoubleClick;
    }

    public interface IFilterable {
        bool isFiltered { get; set; }
    }

	public interface IHasHeight {
		float height { get;}
	}

    public interface IUniqueData {
        string Id { get; }
    }

    public interface IStringFilterable : IFilterable {
        string filterString { get; }
    }

    public interface INameable {
        void SetName(string name);
    }

    public interface IInteractable{
        bool Interactable { get; set;}
    }
}
