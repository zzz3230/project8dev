using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotWidgetScript : WidgetBehaviour
{
    public int index = -1;

    [SerializeField] Image _spriteRenderer;
    public Sprite sprite { 
        get { return _spriteRenderer.sprite; } 
        set { _spriteRenderer.sprite = value; } 
    }
    [SerializeField] TextMeshProUGUI _countText; 
    public int count { 
        get { return _count; } 
        set { 
            _count = value;
            _spriteRenderer.gameObject.SetActive(_count != 0);
            _countText.text = _count == 1 ? string.Empty : _count.ToString(); 
        } 
    }
    int _count = 0;

    [SerializeField] Image panelImage;
    [SerializeField] Color baseColor;
    [SerializeField] Color selectedColor;

    public bool selected { 
        get { return _selected; } 
        set { 
            _selected = value;
            panelImage.color = _selected ? selectedColor : baseColor;
        } 
    }
    bool _selected = false;
}
