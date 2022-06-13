using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotWidgetScript : WidgetBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotManager manager;
    public int index = -1;

    [SerializeField] Slider _durabilitySlider;
    [SerializeField] Image _durabilityImage;
    float _durability;
    public float durablilty
    {
        get => _durability;
        set
        {
            _durability = value;
            _durabilitySlider.value = value;
            // H 112 -> 359
            // S 74
            // V 97
            // A 100
            //Log.Ms(value);

            _durabilityImage.enabled = value > 0f;

            float hMin = 0f;           //112f / 359f;
            float hMax = 112f / 359f;  //1f;
            float hDiff = hMax - hMin;
            float h = hMin + hDiff * value;
            float s = 74f / 100f;
            float v = 97f / 100f;
            //Log.Ms(h);
            //float a = 1f;
            _durabilityImage.color = Color.HSVToRGB(h, s, v);
        }
    }


    [SerializeField] Image _spriteRenderer;
    public Sprite sprite
    {
        get { return _spriteRenderer.sprite; }
        set { _spriteRenderer.sprite = value; }
    }

    [SerializeField] TextMeshProUGUI _countText;
    public int count
    {
        get { return _count; }
        set
        {
            _count = value;
            _spriteRenderer.gameObject.SetActive(_count != 0);
            _countText.text = _count == 1 || _count == 0 ? string.Empty : _count.ToString();
        }
    }

    int _count = 0;

    [SerializeField] Image panelImage;
    [SerializeField] Color baseColor;
    [SerializeField] Color selectedColor;

    public bool selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            panelImage.color = _selected ? selectedColor : baseColor;
        }
    }

    public Texture2D image { set { sprite = Sprite.Create(value, new Rect(0, 0, value.width, value.height), new Vector2(0.5f, 0.5f)); } }

    bool _selected = false;

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    eventData.button.log();
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        manager.BeginDrag((MouseButton)(int)eventData.button);
        //print("biegin " + index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        DragAndDropManager.lastEntred.manager.EndDrag();
        //print("end " + index);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if(DragAndDropManager.isDragging)
        //print("Enter " + index);

        panelImage.color = selectedColor;
        //eventData.pointerPress = gameObject; 
        DragAndDropManager.lastEntred = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //print("Exit " + index); 
        panelImage.color = baseColor;
    }
}
