using System;
using System.Collections.Generic;
using HereBeElements.Templates;
using UnityEngine;
using UnityEngine.UI;

namespace HereBeElements.UI
{
    [Serializable]
    public class UITable<T, V> : UIElementGroup, IUpdatable<List<T>> where V : UITableLine<T>
    {
        public GameObject linePrefab;
        private Dictionary<string, T> _elements = new Dictionary<string, T>();
        private Transform _listTransform;
        
        public bool overrideColors = false;
        [ColorUsage(true, true)]
        public Color colorOverride;
        [ColorUsage(true, true)]
        public Color stepOverride;
        [ColorUsage(true, true)] 
        public Color highlightOverride;
        [ColorUsage(true, true)] 
        public Color selectOverride;
        
        protected override void Awake()
        {
            base.Awake();
            _listTransform = GetComponentInChildren<VerticalLayoutGroup>().transform;
        }

        public virtual void UpdateState(List<T> data)
        {
            if (_listTransform != null)
            {
                T el;
                for (int i = 0; i < data.Count; i++)
                {
                    el = data[i];
                    if (i >= _listTransform.childCount)
                        GetElement(el, i);
                    else
                    {
                        Transform obj = _listTransform.GetChild(i);
                        V comp = obj.GetComponent<V>();
                        comp.UpdateState(el);
                    }
                }

                if (_listTransform.childCount > data.Count)
                {
                    for(int i = data.Count; i< _listTransform.childCount; i++)
                    {
                        Destroy(_listTransform.GetChild(i).gameObject);
                    }
                }
            }
        }

        private GameObject GetElement(T data, int index)
        {
            GameObject element = Instantiate(linePrefab, _listTransform, true);
            element.transform.localScale = new Vector3(1,1,1);
            V line = element.GetComponent<V>();
            if (overrideColors)
            {
                ColorBlock cb = line.colors;
                Color normal = index % 2 != 0 ? stepOverride : colorOverride;
                line.colors = new ColorBlock()
                {
                    normalColor = normal,
                    highlightedColor = highlightOverride,
                    pressedColor = normal,
                    selectedColor = selectOverride,
                    disabledColor = cb.disabledColor,
                    colorMultiplier = cb.colorMultiplier,
                    fadeDuration = cb.fadeDuration
                };
                
            }

            line.UpdateState(data);
            return element;
        }
        
    }
}