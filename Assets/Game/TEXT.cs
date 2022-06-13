[System.Serializable]
public class TEXT
{
    public string uuid;
    public string _text;

    public TEXT(string _en = "empty", string key = "auto")
    {
        _text = _en;
    }

    public string GetStringOnLang(string lang)
    {
        return _text;
    }

    public override string ToString()
    {
        return _text;
    }
}
