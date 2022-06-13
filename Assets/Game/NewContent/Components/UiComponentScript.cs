using UnityEngine;


[RequireComponent(typeof(ReferenceComponentScript))]
public class UiComponentScript : BaseUnitComponentScript
{
    public enum KindOfShow
    {
        Independent, WithInventory
    }

    public WidgetBehaviour originalWidget;
    WidgetBehaviour widget;
    bool isWidgetShown = false;

    public KindOfShow kindOfShow;
    public bool unlockMouse;

    ReferenceComponentScript refComponent;
    public override void GameStart()
    {
        refComponent = GetComponent<ReferenceComponentScript>();
    }

    [SerializeField] bool _debug_ShowWidget;
    [SerializeField] bool _debug_HideWidget;
    public override void GameUpdate()
    {
        if (_debug_ShowWidget)
        {
            ShowWidget();
            _debug_ShowWidget = false;
        }
        if (_debug_HideWidget)
        {
            HideWidget();
            _debug_HideWidget = false;
        }
    }
    public WidgetBehaviour GetWidget()
    {
        if (widget == null)
        {
            widget = Utils.SpawnWidget(originalWidget);
            widget.Hide();
        }


        return widget;
    }
    public void SwitchShowing()
    {
        if (isWidgetShown)
            HideWidget();
        else
            ShowWidget();
    }

    public void ShowWidget()
    {
        if (widget == null)
            widget = Utils.SpawnWidget(originalWidget);

        if (kindOfShow == KindOfShow.WithInventory)
            refComponent.GetPlayerScript().playerInventoryWidgetScript.ShowWithChildWidget(widget);
        else
        {
            widget.Show();
            if (unlockMouse)
                refComponent.GetPlayerScript().UnlockMouse();
        }


        isWidgetShown = true;
    }
    public void HideWidget()
    {
        if (kindOfShow == KindOfShow.WithInventory)
            refComponent.GetPlayerScript().playerInventoryWidgetScript.HideWithChildWidget();
        else
        {
            widget.Hide();
            if (unlockMouse)
                refComponent.GetPlayerScript().LockMouse();
        }


        isWidgetShown = false;
    }

    private void OnDestroy()
    {
        if (widget)
        {
            //print("destroing widget");
            Destroy(widget.gameObject);
        }

    }
}
