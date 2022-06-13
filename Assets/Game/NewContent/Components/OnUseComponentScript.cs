using System;
using UnityEngine;
public struct OnUseEventData
{
    public MouseButton mouseButton;
}
public class OnUseComponentScript : BaseUnitComponentScript
{
    Action<OnUseEventData> onUseBindings;

    [SerializeField] bool debug_simualtePlayerRMC;
    public override void GameUpdate()
    {
        if (debug_simualtePlayerRMC)
        {
            PlayerUsed(new OnUseEventData { mouseButton = MouseButton.Right });
            debug_simualtePlayerRMC = false;
        }
    }
    public void BindOnUse(Action<OnUseEventData> a)
    {
        onUseBindings += a;
    }

    public void PlayerUsed(OnUseEventData args)
    {
        onUseBindings.Invoke(args);
    }
}
