using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryWidgetScript : WidgetBehaviour
{
    [SerializeField] GameObject _slotsPanel;
    public SlotWidgetScript slotWidgetOriginal;
    public int slotsCount = 30;

    void Start()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            //Instantiate(slotWidgetOriginal).transform.SetParent(_slotsPanel.transform);
            Utils.SpawnSlotWidget(slotWidgetOriginal, _slotsPanel, i);
        }
    }

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
}
 