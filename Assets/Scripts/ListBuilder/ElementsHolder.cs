using System.Collections.Generic;


namespace ui6 {

    public interface IElementsHolderGeneric<T>
    {
        int Count { get; }
        T Get(int i);
    }

    public interface IDrawableElementsHolder : IElementsHolderGeneric<IDrawable> { }

	

    public class DrawableElementsHolder<M> : IDrawableElementsHolder
        where M : IDrawable
    {
        List<M> elements;

        public DrawableElementsHolder(List<M> elements) {
            this.elements = elements;
        }

        public int Count { get { return elements.Count; } }

        public IDrawable Get(int i) {
            return elements[i];
        }
    }

   
}