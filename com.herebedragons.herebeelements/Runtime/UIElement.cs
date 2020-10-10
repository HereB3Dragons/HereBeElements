using System;
using com.herebedragons.herebeelements.Runtime.Templates;
using HereBeElements.Components;
using Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
[RequireComponent(typeof(ShaderControl))]
[RequireComponent(typeof(UIImage))]
public class UIElement : UISelectable, IPointerClickHandler, IElement
{
    protected ShaderControl _sc;
    protected Graphic _graphic;
    // private CanvasGroup _canvasGroup;
    private bool _isHighlight = false;
    private bool _isVisible = true;


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
        return IsInteractable();
    }

    public void Enable(bool onOff = true)
    {
        this.interactable = onOff;
    }

    public void Disable()
    {
        Enable(false);
    }

    public bool IsHighlight()
    {
        return base.IsHighlighted() || _isHighlight;
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
 
    protected override void Awake()
    {
        base.Awake();
        _sc = GetComponent<ShaderControl>();
        _graphic = GetComponent<Graphic>();
        ApplyShaderConfig();
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
        Graphic g = GetGraphic();
        if (g != null)
            _sc.ApplyConfig(g);
    }

    public Graphic GetGraphic()
    {
        return _graphic;
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
        
    public void OnPointerClick(PointerEventData eventData)
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
}
