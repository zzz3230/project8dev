using System;
using System.IO;
using System.Linq;
using System.Text;

public class GameSavingManager
{
    public GameSavingManager(string saveName)
    {
        overworld = new WorldSavingManager();
        overworld.Init(saveName, 1);

        save = new WorldSavingManager();
        save.Init(saveName, 0);
    }
    public static GameSavingManager Instance { get { return Global.gameSavingManager; } }
    public WorldSavingManager overworld;
    public WorldSavingManager save;
}


public class WorldSavingManager
{
    struct Paths
    {
        public const string static_res = "static/";
        public const string worldx_regs = "world_{0}/regs/";
        public const string worldx_unit_components = "world_{0}/ucmp/";
        public const string worldx_items = "extra/items/";
        public static string Worldx_regs(int x) { return string.Format(worldx_regs, x.ToString()); }

        public static string Worldx_unit_components(int x) { return string.Format(worldx_unit_components, x.ToString()); }
        public static string Items() { return string.Format(worldx_items); }
        public const string player = "extra/";
        //public const string world0_regs = "world_0/regs/";

        public const string world_regxy_name = "reg_{0}_{1}";
        public static string World_regxy_name(int x, int y) { return string.Format(world_regxy_name, x, y); }

        public const string world_itemx_name = "item_{0}";
        public static string World_itemx_name(int x) { return string.Format(world_itemx_name, x); }

        public const string saves = "saves/{0}/";
        public static string Saves(string name) { return @"D:\unity_project\Project8\test_save\" + string.Format(saves, name); }
        public static string StaticRes(string name, string fileName) { return @"D:\unity_project\Project8\test_save\" + static_res + name + @"\" + fileName; }
    }
    string saveName;
    int worldIndex;

    public void Init(string saveName, int worldIndex)
    {
        this.saveName = saveName;
        this.worldIndex = worldIndex;
    }
    string MakeWorldUnitComponentsPath(int x)
    {
        return
            Paths.Worldx_unit_components(x);
    }
    string MakeWorldRegPath(int x, int y)
    {
        return
            Paths.Saves(saveName) +
            Paths.Worldx_regs(worldIndex) +
            Paths.World_regxy_name(x, y);
    }
    string MakeItemPath(int x)
    {
        return
            Paths.Saves(saveName) +
            Paths.Items() +
            Paths.World_itemx_name(x);
    }
    string MakeStaticResPath(string resName, string fileName)
    {
        return
            Paths.StaticRes(resName, fileName);
    }

    T BytesToObject<T>(byte[] data)
    {
        return (T)Convert.ChangeType(Newtonsoft.Json.JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), typeof(T)), typeof(T));
    }
    byte[] ObjectToBytes(object obj)
    {
        return Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
    }

    void ValidatePath(string path)
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path));
    }
    void ValidateFile(string path, object def)
    {
        if (!File.Exists(path))
            File.Create(path).Close();
    }

    struct FileRecordStructs
    {
        public struct FR_ItemInstance
        {
            public string id;
            public int count;
            public UnitMetadata metadata;

            public static FR_ItemInstance empty { get => new FR_ItemInstance { count = 0, id = null, metadata = null }; } //;
        }
    }
    public const int ITEM_REGION_COUNT = 1024;

    public ItemInstance[] LoadItemSector(int index)
    {
        var filePath = MakeItemPath(index);

        ValidatePath(filePath);
        ValidateFile(filePath, new FileRecordStructs.FR_ItemInstance[ITEM_REGION_COUNT]);

        var data = File.ReadAllBytes(filePath);

        var fr = BytesToObject<FileRecordStructs.FR_ItemInstance[]>(data);

        if (fr == null)
            fr = Enumerable.Repeat(FileRecordStructs.FR_ItemInstance.empty, ITEM_REGION_COUNT).ToArray();

        var items = new ItemInstance[ITEM_REGION_COUNT];
        for (int i = 0; i < ITEM_REGION_COUNT; i++)
        {
            if (fr[i].count != 0)
                items[i] = new ItemInstance
                {
                    count = fr[i].count,
                    empty = false,
                    metadata = fr[i].metadata,
                    info = ItemsHub.GetItemInfoByStrId(fr[i].id)//BasicConsoleScript.Instance.debugItems.First((x) => x.id.ToString() == fr[i].id)
                };
        }
        return items;
    }
    public void SaveItemSector(int index, ItemInstance[] items)
    {
        var filePath = MakeItemPath(index);

        ValidatePath(filePath);

        var fr = new FileRecordStructs.FR_ItemInstance[ITEM_REGION_COUNT];
        for (int i = 0; i < ITEM_REGION_COUNT; i++)
        {
            if (items[i] != null)
                fr[i] = new FileRecordStructs.FR_ItemInstance
                {
                    count = items[i].count,
                    metadata = items[i].metadata,
                    id = items[i].info.id.ToString()
                };
        }

        var bytes = ObjectToBytes(fr);

        File.WriteAllBytes(filePath, bytes);
    }
}
