using System;
using com.herebedragons.herebeelements.Runtime.Templates;
using HereBeElements.Components;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
[RequireComponent(typeof(ShaderControl))]
public class Element: MonoBehaviour, IElement
{
    private bool _isVisible = true;
    private bool _isHighlight = false;

    protected ShaderControl _sc;
    protected Renderer _renderer;

    private void Awake()
    {
        _sc = GetComponent<ShaderControl>();
        _renderer = GetComponent<Renderer>();
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
        throw new NotImplementedException();
    }

    public void Enable(bool onOff = true)
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }

    public bool IsHighlight()
    {
        throw new NotImplementedException();
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

    public void ApplyShaderConfig()
    {
        if (_renderer != null)
            _sc.ApplyConfig(_renderer);
    }
    
    public delegate void EnableEventHandler();
    public event EnableEventHandler EnableEvent;

    protected void OnEnable()
    {
        EnableEventHandler enable = EnableEvent;
        if (enable != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onEnable", this);
            enable();
        }
    }
    
    public delegate void DisableEventHandler();
    public event DisableEventHandler DisableEvent;
    
    protected  void OnDisable()
    {
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

    public void OnSelect(BaseEventData eventData)
    {
        SelectEventHandler select = SelectEvent;
        if (select != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onSelect", this);
            select();
        }
    }
    
    public delegate void DeselectEventHandler();
    public event DeselectEventHandler DeselectEvent;
    
    public void OnDeselect(BaseEventData eventData)
    {
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

    protected virtual void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnterEventHandler mouseEnter = MouseEnterEvent;
        if (mouseEnter != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onMouseEnter", this);
            mouseEnter();
        }
    }
        
    protected virtual void OnPointerExit(PointerEventData eventData)
    {
        MouseLeaveEventHandler mouseLeave = MouseLeaveEvent;
        if (mouseLeave != null)
        {
            UISystemProfilerApi.AddMarker("UIElement.onMouseLeave", this);
            mouseLeave();
        }
    }
}