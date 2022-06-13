using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SavingDataComponentScript))]
public class ItemStorageComponentScript : BaseSaveableComponentScript
{
    public ItemsManagerPointer[] ptrs;
    [SerializeField] int _count;
    public int count { get => _count; set { _count = value; Resize(value); } }

    public override void GameAwake()
    {
        base.GameAwake();
    }

    public override void GameStart()
    {
        if (HasSaveWithKey("ptrs_"))
        {
            ptrs = LoadObjects<ItemsManagerPointer>("ptrs_");
            print($"Loaded ptr: {ptrs[0].offset}");
        }
        Resize(count);
    }

    public void Resize(int newSize)
    {
        var lstPtrs = ptrs.ToList();
        for (int i = 0; i < count; i++)
        {
            if (i >= lstPtrs.Count)
                lstPtrs.Add(ItemsManager.Allocate(1));
        }
        for (int i = 0; i < lstPtrs.Count; i++)
        {
            if (i >= count)
            {
                ItemsManager.Free(lstPtrs[i]);
                lstPtrs[i] = null;
            }
        }

        ptrs = lstPtrs.ToArray();

        System.Array.Resize(ref ptrs, newSize);
        for (int i = 0; i < count; i++)
        {
            GetPointer(i);
        }

        Resave();
    }
    void Resave()
    {
        SaveObjects($"ptrs_", ptrs);
    }

    public ItemsManagerPointer GetPointer(int index)
    {
        var ptr = ptrs[index];
        if (ptr == null)
        {
            ptr = ItemsManager.Allocate(1);
            ptrs[index] = ptr;
            Resave();
        }
        return ptr;
    }

}
