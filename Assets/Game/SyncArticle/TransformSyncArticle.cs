using UnityEngine;

public class TransformSyncArticle : BaseSyncArticle
{
    [System.Serializable]
    public struct TransformSyncArticleData
    {
        public SerVector3? pos;
        public Quaternion? rot;
        public SerVector3? scale;

    }

    public bool syncPosition = true;
    public bool syncRotation = true;
    public bool syncScale = true;

    public override bool S_ValueUpdated()
    {
        if (lastSendedValue == null)
            return true;

        return lastSendedValue.Equals(S_GetValue());
    }
    public override void C_SetValue(object value)
    {
        var data = (TransformSyncArticleData)value;

        if (data.pos != null)
            transform.position = data.pos.Value.GetVector3();
        if (data.rot != null)
            transform.rotation = data.rot.Value;
        if (data.scale != null)
            transform.localScale = data.scale.Value.GetVector3();
    }
    public override object S_GetValue()
    {
        var data = new TransformSyncArticleData();

        if (syncPosition)
            data.pos = new SerVector3(transform.position);
        else
            data.pos = null;

        if (syncRotation)
            data.rot = transform.rotation;
        else
            data.rot = null;

        if (syncScale)
            data.scale = new SerVector3(transform.localScale);
        else
            data.scale = null;

        return data;
    }
}
