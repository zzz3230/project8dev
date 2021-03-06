using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDWidgetScript : WidgetBehaviour
{
    List<SlotWidgetScript> _slots = new List<SlotWidgetScript> { };

    public SlotWidgetScript slotWidgetOriginal;
    [SerializeField] GameObject _slotsPanel;


    public int slotsCount = 9;
    private void Awake()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            var slot = Utils.SpawnWidget(slotWidgetOriginal);
            slot.transform.SetParent(_slotsPanel.transform);
            slot.transform.localScale = Vector3.one;
            _slots.Add(slot);
        }
    }

    public void Set?urrentSlot(int index)
    {
        _slots.ForEach((x) => x.selected = false);
        //print($"i={index}; max={_slots.Count}");
        _slots[index].selected = true;
        //UIDocument x;
    }

    public bool visibleState { get { return gameObject.activeSelf; } }
    /*
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    */
    public void SwitchVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        Utils.log($"set inv visible :  {gameObject.activeSelf}");
    }

    public void UpdateSlots()
    {

    }

    internal SlotWidgetScript GetSlot(int i)
    {
        return _slots[i];
    }
}
