using UnityEngine;

public class TestInteractiveHandItemScript : RuntimeHandItemScript
{
    [SerializeField] Light _light;

    public override void GameStart()
    {
        GetComponent<SavingDataComponentScript>().Init(this);

        var ui = GetComponent<UiComponentScript>();
        ui.Init();

        var widgetScr = ui.GetWidget().CastTo<TestWidgetScript>();

        //var itemPrt = GetComponent<ItemStorageComponentScript>();
        //itemPrt.count = 3;;
        var unitData = GetComponent<UnitDataComponentScript>();
        unitData.Init("~", this);
        
        //print($"my uid is {unitData.uid}"); 

        if (!unitData.HasKey("root"))
        {
            ItemsManagerPointer[] ptrs = new ItemsManagerPointer[3];
            for (int i = 0; i < ptrs.Length; i++)
            {
                ptrs[i] = ItemsManager.Allocate(1);
            }
            unitData.Set("root", ptrs);
        }

        for (int i = 0; i < 3; i++)
        {
            Utils.SpawnSlotWidget(widgetScr.originalSlot, widgetScr.panel, unitData.Get<ItemsManagerPointer[]>("root")[i]);
        }

        var onUseComp = GetComponent<OnUseComponentScript>();
        onUseComp.Init();
        
        onUseComp.BindOnUse((d) =>
        {
            if (d.mouseButton == MouseButton.Right)
                ui.SwitchShowing();
        });
    }
    public override void GameUpdate()
    {
        var md = info.CastMetadata<UnitMetadata>();
        if (md.durability <= 0)
        {
            _light.enabled = false;
            md.durability = 0f;
        }
        else
        {
            _light.enabled = true;
            md.durability -= Time.deltaTime * 1;
            DurabilityUpdated();
        }
    }
}
