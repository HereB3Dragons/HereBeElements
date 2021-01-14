using HereBeElements.Templates;

namespace HereBeElements.UI
{
    public abstract class UITableLine<T>: UIElementGroup, IUpdatable<T>
    {
        private UIDisplay[] _cells;
        private T _data;

        public T Data
        {
            get => _data;
            protected set => _data = value;
        }

        protected UIDisplay[] Cells => _cells;
        protected override void Awake()
        {
            base.Awake();
            _cells = GetComponentsInChildren<UIDisplay>();
        }

        public abstract void UpdateState(T td);

    }
}