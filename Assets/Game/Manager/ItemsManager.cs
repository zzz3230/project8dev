using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class ItemsManagerPointerUtils
{
    public static void Swap(this ItemsManagerPointer a, ItemsManagerPointer b)
    {
        (b.sector, a.sector) = (a.sector, b.sector);
        (b.offset, a.offset) = (a.offset, b.offset); 
        (b.count, a.count) = (a.count, b.count);
    }
    public static void SetFrom(this ItemsManagerPointer a, ItemsManagerPointer b)
    {
        a.sector = b.sector;
        a.offset = b.offset;
        a.count = b.count;
        a[0].empty = false;
    }
    public static void CopyTo(this ItemsManagerPointer a, ItemsManagerPointer b)
    {
        b[0].info = a[0].info;
        b[0].count = a[0].count;
        b[0].empty = false;

        if(a[0].metadata != null)
            b[0].metadata = Utils.DeepCopy(a[0].metadata);
    }
    public static void MoveTo(this ItemsManagerPointer a, ItemsManagerPointer b, int count)
    {
        if(count == 0)
        {
            Log.Error("moving count == 0", "itemsmanagerpointerutils-move");
            return;
        }

        int bCount = b[0] != null ? b[0].count : 0;
        var rest = a[0].count - count;

        if(rest < 0)
        {
            Log.Error($"rest in slot a less than zero", "itemsmanagerpointerutils-move");
            return;
        }
        
        if(rest == 0)
        {
            Swap(b, a); 

            b[0].count = count + bCount;

            a.Remove(0);
            a[0].empty = true;
        }
        else
        {
            CopyTo(a, b);
            a[0].count = rest;
            b[0].count = count + bCount;
            b[0].empty = false;
        }
    }
}

public class ItemsManagerPointer
{
    public int offset;
    public int count;
    public int sector;

    public void Remove(int index)
    {
        this[index] = ItemInstance.Empty;
    }

    public ItemInstance this[int i]
    {
        get { return ItemsManager.GetByGlobalIndex(sector, offset + i); }
        set { ItemsManager.SetByGlobalIndex(sector, offset + i, value); }
    }

    public override string ToString()
    {
        return $"item ptr [s:{sector} o:{offset} c:{count}] gl:{sector * 1024 + offset}";
    }
}

public static class ItemsManager
{
    public static Dictionary<int, ItemInstance[]> _loadedSectors = new Dictionary<int, ItemInstance[]>();

    public static ItemsManagerPointer Allocate(int count)
    {
        ItemsManagerPointer finalPointer = null;
        int iterCount = 0;

        while(finalPointer == null)
        {
            iterCount += 1;
            for (int i = 0; i < _loadedSectors.Keys.Count; i++)
            {
                var pointer = AllocateBySectorIndex(i, count);
                if (pointer != null)
                    return pointer;
            }
            LoadSector(_loadedSectors.Count);

            if (iterCount > 10)
                Log.Warning($"sector loading iterator > 10 ({iterCount})", "itemsmanager-allocate");
        }
        return finalPointer;
    }
    static bool IsSectorLoad(int index) => _loadedSectors.ContainsKey(index);

    static void LoadSector(int index)
    {
        if(_loadedSectors.ContainsKey(index))
        {
            Log.Error($"sector with index {index} already loaded", "itemsmanager-loadsector");
            return;
        }
        ItemInstance[] loaded;
        loaded = GameSavingManager.Instance.save.LoadItemSector(index);

        _loadedSectors.Add(index, loaded);
    }

    static void SaveAllSectors()
    {
        foreach (var item in _loadedSectors)
        {
            GameSavingManager.Instance.save.SaveItemSector(item.Key, item.Value);
        }
    }

    static ItemsManagerPointer AllocateBySectorIndex(int index, int count)
    {
        var sector = _loadedSectors[_loadedSectors.Keys.ToArray()[index]];

        var maxFree = 0;
        for (int j = 0; j < sector.Length; j++)
        {
            if (sector[j] == null)
                maxFree++;
            else
                maxFree = 0;

            if (maxFree == count)
            {
                for (int i = 0; i < count; i++)
                {
                    sector[j - count + 1 + i] = ItemInstance.Empty;
                }

                return new ItemsManagerPointer
                {
                    sector = index,
                    count = count,
                    offset = j - count + 1
                };
            }
        }
        return null;
    }

    public static void Free(ItemsManagerPointer pointer)
    {
        for (int i = 0; i < pointer.count; i++)
        {
            pointer[i] = null;
        }
        pointer.count = -1;
        pointer.sector = -1;
        pointer.offset = -1;
    }

    public static ItemInstance GetByGlobalIndex(int sector, int index)
    {
        if (!IsSectorLoad(sector))
            LoadSector(sector);


        //Log.Ms(sector + " " + index);

        return _loadedSectors[sector][index];
    }
    public static void SetByGlobalIndex(int sector, int index, ItemInstance item)
    {
        if (!IsSectorLoad(sector))
            LoadSector(sector);


        _loadedSectors[sector][index] = item;
    }
}

