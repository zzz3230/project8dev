using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryWidgetScript : WidgetBehaviour
{
    [SerializeField] GameObject _slotsPanel;
    [SerializeField] GameObject _childWidgetPanel;

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
            slotsWidgets.Add(Utils.SpawnSlotWidget(slotWidgetOriginal, _slotsPanel, null, i).widget);
        }
    }
    WidgetBehaviour childWidget;
    public void ShowWithChildWidget(WidgetBehaviour widget)
    {
        //throw new NotImplementedException();, false
        childWidget = widget;
        widget.transform.SetParent(_childWidgetPanel.transform);
        widget.Show();
        playerScript.ShowInventory();
    }
    public void HideWithChildWidget()
    {
        if (childWidget == null)
            Log.Error("widgeet is not showen");

        childWidget.transform.SetParent(null);
        childWidget.Hide();
        playerScript.HideInventory();
        childWidget = null;
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
