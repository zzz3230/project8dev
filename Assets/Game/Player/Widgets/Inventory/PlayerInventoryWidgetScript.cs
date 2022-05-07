using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryWidgetScript : WidgetBehaviour
{
    [SerializeField] GameObject _slotsPanel;
    public SlotWidgetScript slotWidgetOriginal;
    public List<SlotWidgetScript> slotsWidgets = new List<SlotWidgetScript>();
    NewBasePlayerScript playerScript;
    public int slotsCount = 27;

    public void Init(NewBasePlayerScript player)
    {
        playerScript = player;
    }

    void Awake()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            //Instantiate(slotWidgetOriginal).transform.SetParent(_slotsPanel.transform);
            slotsWidgets.Add(Utils.SpawnSlotWidget(slotWidgetOriginal, _slotsPanel, i));
        }
    }

    public SlotWidgetScript GetSlot(int i)
    {
        //print(i);
        return slotsWidgets[i];
    }

    public void SwitchVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        Utils.log($"set inv visible :  {gameObject.activeSelf}");
    }
}
 