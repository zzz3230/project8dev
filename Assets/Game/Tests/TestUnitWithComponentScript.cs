public class TestUnitWithComponentScript : RuntimeUnitScript
{
    public override void GameStart()
    {
        var ui = GetComponent<UiComponentScript>();
        var widgetScr = ui.GetWidget().CastTo<TestWidgetScript>();

        var itemPrt = GetComponent<ItemStorageComponentScript>();
        itemPrt.count = 3;

        for (int i = 0; i < 3; i++)
        {
            Utils.SpawnSlotWidget(widgetScr.originalSlot, widgetScr.panel, itemPrt.GetPointer(i));
        }

        GetComponent<OnUseComponentScript>().BindOnUse((d) =>
        {
            ui.SwitchShowing();
        });
    }
}
