using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
/*
public class PlayerInventoryUIManager : StVisualElement
{
    public int slotsCount = 27;
    List<SlotUIManager> slots = new List<SlotUIManager>();

    bool created = false;

    public PlayerInventoryUIManager()
    {
        //Log.Ms("slots spawned");
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (created)
                return;
            
            var ch = this.Children().ToList();

            this.Q("inv-slots-box")?.Clear();
            slots.Clear();

            for (int i = 0; i < slotsCount; i++)
            {
                slots.Add(new SlotUIManager());
                this.Q("inv-slots-box")?.Add(slots.Last());
            }

            Loaded();
            //slots[2].Highlight(true);
            created = true;
        });
    }

    public Action<object> loaded
    {
        get { return _loaded; }
        set { if (tocall && !called) { value(this); called = true; } _loaded = value; }
    }
    Action<object> _loaded;

    bool tocall = false;
    bool called = false;
    void Loaded()
    {
        if (called)
            return;

        if (loaded == null)
        {
            tocall = true;
        }
        else
        {
            loaded(this);
            called = true;
        } 
            
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<PlayerInventoryUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    internal SlotUIManager GetSlot(int i)
    {
        return slots[i];
    }
    #endregion
}
*/