using OldNetwork;
using UnityEngine;

public class TestNetworkObjectScript : NetworkMonoBehaviour
{
    void Start()
    {
        Utils.Log("LEVEL OBJECT CREATED!!!", 99);
        Utils.Log("hash:" + objectHash, 99);
    }

    private void OnGUI()
    {
        if (!isServer)
        {
            GUILayout.BeginArea(new Rect(20f, 50f, 100, 100));
            if (GUILayout.Button("Invoke"))
            {
                CallOnServer(nameof(Calc), true, 10, 12).Then((obj) =>
                {
                    Utils.Log("FROM S:" + obj);
                });
            }
            GUILayout.EndArea();
        }
    }

    public int Calc(int x, int y)
    {
        var result = x * y;
        Utils.Log(result);
        return result;
    }
}
