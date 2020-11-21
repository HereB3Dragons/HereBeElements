using System;
using System.Collections;
using com.herebedragons.herebeelements.Runtime.Templates;
using HereBeElements.Components;
using Internal;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

[Serializable]
[RequireComponent(typeof(ShaderControl))]
public class Element: InGameSelectable, IElement
{
    private bool _isVisible = true;
    private bool _isHighlight = false;

    protected Renderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<Renderer>();
        ApplyShaderConfig();
    }

    public bool IsVisible()
    {
        return _isVisible;
    }

    public void Show(bool onOff = true)
    {
        _sc.Opacity = onOff ? 1 : 0;
        _isHighlight = onOff;
    }

    public void Hide()
    {
        Show(false);
    }

    public bool IsEnabled()
    {
        return true; //throw new NotImplementedException();
    }

    public void Enable(bool onOff = true)
    {
        //throw new NotImplementedException();
    }

    public void Disable()
    {
        //throw new NotImplementedException();
    }

    public bool IsHighlight()
    {
        //throw new NotImplementedException();
        return false;
    }

    public void Highlight(bool onOff = true)
    {
        if (onOff)
        {
            HighlightEventHandler highlight = HighlightEvent;
            if (highlight != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onHighlight", this);
                highlight();
                _isHighlight = true;
            }
        }
        else
        {
            DeHighlightEventHandler dehighlight = DeHighlightEvent;
            if (dehighlight != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onDeHighlight", this);
                dehighlight();
                _isHighlight = false;
            } 
        }
    }

    public void DeHighlight()
    {
        Highlight(false);
    }
    
#if UNITY_EDITOR
    protected override void OnValidate()
    { 
        base.OnValidate();
#else
        protected virtual void OnValidate()
        {
#endif
        ApplyShaderConfig(); 
    }

    public void ApplyShaderConfig()
    {
        Renderer r = GetGraphic();
        if (r != null)
            _sc.ApplyConfig(r);
    }

    public Renderer GetGraphic()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        return _renderer; 
    }

    public delegate void EnableEventHandler();
    public event EnableEventHandler EnableEvent;

    protected override void OnEnable()
    {
        base.OnEnable();
        EnableEventHandler enable = EnableEvent;
        if (enable != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onEnable", this);
            enable();
        }
    }
    
    public delegate void DisableEventHandler();
    public event DisableEventHandler DisableEvent;
    
    protected override void OnDisable()
    {
        base.OnDisable();
        DisableEventHandler disable = DisableEvent;
        if (disable != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onDisable", this);
            disable();
        }
    }

    public delegate void HighlightEventHandler();
    public event HighlightEventHandler HighlightEvent;
    
    public delegate void DeHighlightEventHandler();
    public event DeHighlightEventHandler DeHighlightEvent;
    
    public delegate void SelectEventHandler();
    public event SelectEventHandler SelectEvent;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SelectEventHandler select = SelectEvent;
        if (select != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onSelect", this);
            select();
        }
    }
    
    public delegate void DeselectEventHandler();
    public event DeselectEventHandler DeselectEvent;
    
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        DeselectEventHandler deselect = DeselectEvent;
        if (deselect != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onDeselect", this);
            deselect();
        }
    }

    public delegate void MouseEnterEventHandler();
    public event MouseEnterEventHandler MouseEnterEvent;
        
    public delegate void MouseLeaveEventHandler();
    public event MouseLeaveEventHandler MouseLeaveEvent;

    public delegate void ClickEventHandler();
    public event ClickEventHandler ClickEvent;
        
    protected virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        ClickEventHandler click = ClickEvent;
        if (click != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onClick", this);
            click();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        MouseEnterEventHandler mouseEnter = MouseEnterEvent;
        if (mouseEnter != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onMouseEnter", this);
            mouseEnter();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        MouseLeaveEventHandler mouseLeave = MouseLeaveEvent;
        if (mouseLeave != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onMouseLeave", this);
            mouseLeave();
        }
    }

    public IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
    {
        return Utils.LoadContent(assetRef, setter, percentageSetter);
    }
}