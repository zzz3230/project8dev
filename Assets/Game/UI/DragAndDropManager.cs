public static class DragAndDropManager
{
    internal static SlotWidgetScript lastEntred;
    static SlotManager _draggingSlot;
    static MouseButton _movingMouseBtn;

    public static bool isDragging { get => _draggingSlot != null; }

    static public void Begin(SlotManager slot, MouseButton mBtn)
    {
        _draggingSlot = slot;
        _movingMouseBtn = mBtn;
    }

    /// <summary>
    /// return slot from Begin(slot)
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    static public (SlotManager slotManager, MouseButton mouseBtn) End()
    {
        var slot = _draggingSlot;
        _draggingSlot = null;
        return (slot, _movingMouseBtn);
    }
}

