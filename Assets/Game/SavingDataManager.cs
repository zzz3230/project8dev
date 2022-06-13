using System;

public enum SavingDataId
{
    Nickname
}

[Flags]
public enum SavingDataFormat
{
    Binary = 1,
    Json = 2,
    UserProperty = 4,
    WorldProperty = 8,
    SessionProperty = 16,
    GameProperty = 32,
    IndividualFile = 64,
    StandardFile = 128,
    UseCaching = 256
}

public class SavingDataManager
{
    public T Get<T>(SavingDataId id, SavingDataFormat getFormat = SavingDataFormat.UseCaching)
    {
        return Get<T>(id.ToString(), getFormat);
    }

    public T Get<T>(string id, SavingDataFormat getFormat = SavingDataFormat.UseCaching)
    {
        return default;
    }

    public void Set(object data, SavingDataId id, SavingDataFormat format =
        (SavingDataFormat.Json | SavingDataFormat.UserProperty | SavingDataFormat.StandardFile))
    {

    }
}
