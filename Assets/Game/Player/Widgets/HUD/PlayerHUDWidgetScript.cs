using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHUDWidgetScript : WidgetBehaviour
{
    List<SlotWidgetScript> _slots = new List<SlotWidgetScript> { };

    public SlotWidgetScript slotWidgetOriginal;
    [SerializeField] GameObject _slotsPanel;


    public int slotsCount = 10;
    private void Start()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            var slot = Utils.SpawnWidget(slotWidgetOriginal);
            slot.transform.SetParent(_slotsPanel.transform);
            slot.transform.localScale = Vector3.one;
            _slots.Add(slot);
        }
    }

    public void SetÑurrentSlot(int index)
    {
        _slots.ForEach((x) => x.selected = false);
        //print($"i={index}; max={_slots.Count}");
        _slots[index].selected = true;
        UIDocument x;
    }

    public bool visibleState { get { return gameObject.activeSelf; } }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SwitchVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        Utils.log($"set inv visible :  {gameObject.activeSelf}");
    }

    public void UpdateSlots()
    {

    }
}
