using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct SavingDataElement
{
    public ulong uid;
    public string key;
    public object values;
}

public class SavingDataComponentScript : BaseUnitComponentScript
{
    SaveAccessDataSet MakeSaveAccessDataSet(ulong uid)
    {
        return new SaveAccessDataSet { key = "sav_comp", uid = uid };
    }
    public List<SavingDataElement> data { get; private set; } = new List<SavingDataElement>();

    public T[] LoadAny<T>(ulong uid, string key) { return LoadObjects(uid, key).Cast<T>().ToArray(); }
    public ItemsManagerPointer[] LoadItemPtrs(ulong uid, string key) { return LoadAny<ItemsManagerPointer>(uid, key); }
    public object[] LoadObjects(ulong uid, string key) 
    {
        data = (List<SavingDataElement>)RuntimeSavingSystem.LoadObject(new SaveAccessDataSet { key = "sav_comp", uid = this.uid });
        //print($"Loaded: uid={uid}; key={key}; objs.c=");
        return (object[])data.Find(e => e.key == key).values; 
    }
    public void SaveItemPtrs(ulong uid, string key, ItemsManagerPointer[] ptrs) { SaveObjects(uid, key, ptrs); }
    public bool HasKey(ulong uid, string key)
    {
        if(RuntimeSavingSystem.HasKey(new SaveAccessDataSet { key = "sav_comp", uid = this.uid }))
        {
            var all = (List<SavingDataElement>)RuntimeSavingSystem.LoadObject(MakeSaveAccessDataSet(this.uid));
            if (all.Any((x) => x.key == key))
                return true;
        }
        return false;
    }
    public void SaveObjects(ulong uid, string key, object[] objs)
    {
        //print($"Saved: uid={uid}; key={key}; objs.c={objs.Length}");
        var el = new SavingDataElement { uid = uid, key = key, values = objs };
        bool replaced = false;
        for (int i = 0; i < data.Count; i++)
        {
            if(data[i].key == key && data[i].uid == uid)
            {
                data[i] = el;
                replaced = true;
                break;
            }
        }

        if(!replaced)
            data.Add(el);

        RuntimeSavingSystem.SaveObject(new SaveAccessDataSet { key = "sav_comp", uid = this.uid }, data);
    }
}
