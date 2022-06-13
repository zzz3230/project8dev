using OldNetwork;
using UnityEngine;

public class TEST_cube : NetworkMonoBehaviour
{
    // Start is called before the first frame update
    override public void GameStart()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public Camera cam;

    // Update is called once per frame
    override public void GameUpdate()
    {
        if (isOwner)
        {
            if (Input.GetMouseButton(0))
            {
                //var cam = Camera.current;
                //cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //пускаем луч
                Physics.Raycast(ray, out hit, 10);

                transform.position = hit.point;
            }

        }
    }
}
