using System.Collections.Generic;
using System;

namespace ui6 {
    /// <summary>
    /// pass list, the referance will be saved. 
    /// Input changes will automaticly filter items on input change.
    /// Call RefilterAllItems/FilterItem if other updates are needed (exp: new item, item del, item change)
    /// </summary>
    public class FilterListByNames<T> where T : IStringFilterable {
        FilterImpl<IStringFilterable> filter;
        List<T> elements;
        public event Action OnFilterInputChanged;
        public FilterListByNames(List<T> elements, SimpleFilterViewImpl5 filterInput) {
            this.elements = elements;
            filter = new FilterImpl<IStringFilterable>();
            filter.OnFilterChanged += HandleOnFilterChanged;
            filter.ConnectRequest(new StringFilterRequest(filterInput));
        }

        protected virtual void HandleOnFilterChanged() {
            RefilterAllItems();
            if (OnFilterInputChanged != null) {
                OnFilterInputChanged();
            }
        }

        public void RefilterAllItems() {
            for (int i = 0; i < elements.Count; i++) {
                FilterItem(elements[i]);
            }
        }

        public void FilterItem(T item) {
            item.isFiltered = !filter.PassFilter(item);
        }

        class StringFilterRequest : SearchRequest5<IStringFilterable> {
            public override bool MatchFilter(IStringFilterable item) {
                if (StringRequest == string.Empty)
                    return true;

                return item.filterString.ToLower().Contains(StringRequest);
            }

            public StringFilterRequest(SimpleFilterViewImpl5 view, int orderId = 0)
            : base(view, orderId, true) { }
        }
    }

}