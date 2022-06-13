using OldNetwork;
using UnityEngine;

public class BasePlayerScript : NetworkMonoBehaviour
{
    [SerializeField] Camera _camera;
    public new Camera camera { get { return _camera; } }

    [SerializeField] Rigidbody _rb;
    [SerializeField] CharacterController _controller;
    [SerializeField] GroundCollisionCheckerScript _collisionChecker;

    Vector2 _cameraRotation;
    int _currentSlot = 1;

    public PlayerHUDWidgetScript playerHUDOriginal;
    public PlayerHUDWidgetScript playerHUD;

    public PlayerInventoryWidgetScript playerInventoryWidgetOriginal;
    public PlayerInventoryWidgetScript playerInv;

    public DefBuilder defBuilderOriginal;
    public DefBuilder defBuilder;

    public float playerSpeed = 200;
    public bool isInputMouse = true;

    public int slotsCount = 10;
    public override void GameStart()
    {
        if (!isOwner)
            return;

        playerHUD = Utils.SpawnWidget(playerHUDOriginal);
        playerHUD.slotsCount = slotsCount;

        playerInv = Utils.SpawnWidget(playerInventoryWidgetOriginal);
        playerInv.Hide();

        defBuilder = Utils.SpawnBuilder(defBuilderOriginal);

        //print(Utils.ArrToStr(GetComponents<MonoBehaviour>()));

        //Cursor.lockState = CursorLockMode.Locked;
        LockMouse();
    }

    Vector3 s_move;
    void S_MovePlayer(SerVector3 move)
    {
        s_move = move.GetVector3();
    }

    Vector3 lastSendedMove;
    public override void GameFixedUpdate()
    {
        //Debug.Log(1f/Global.networkManager.tickRate*100);
        //Debug.Log(1f);
        //if (isOwner)
        //{
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.Translate(move * Time.deltaTime * 30);
        //_rb.velocity = Quaternion.AngleAxis(transform.position.y, move * Time.deltaTime * 12).eulerAngles;

        //transform.Translate(move * Time.deltaTime * 30);\
        //if (!isServer)
        //{
        //    if (move != lastSendedMove)
        ////    {
        //        CallOnServer(nameof(S_MovePlayer), false, move.GetSerVector3());
        //        lastSendedMove = move;
        //    }
        //}
        //else
        s_move = move;

        //}




        //if (isServer)
        //{
        var moveRot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //print(_collisionChecker.contactCount);
        if (_collisionChecker.onGround)
            _rb.velocity =
                    //_rb.MovePosition(
                    //    _rb.position + 
                    //transform.Translate(
                    //Vector3.ProjectOnPlane
                    //(
                    moveRot *
                    s_move
                    //, 
                    //_collisionChecker.groundNormal
                    //) 
                    * Time.deltaTime * playerSpeed
            //)
            ;
        //}

    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInputMouse = true;
    }
    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isInputMouse = false;
    }

    public override void GameUpdate()
    {
        if (isOwner)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                UnlockMouse();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LockMouse();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                playerInv.SwitchVisible();
            }

            #region -move | +camera

            //Debug.Log(moveRot *
            //        move);

            if (isInputMouse)
            {
                var mouse = new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));

                _cameraRotation.x += mouse.x;

                _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90, 90);

                _camera.transform.localRotation = Quaternion.Euler(_cameraRotation);

                transform.Rotate(0, mouse.y, 0);
            }

            #endregion

            #region slots scroll
            float mw = Input.GetAxis("Mouse ScrollWheel");
            //print(mw);
            _currentSlot = (int)Mathf.Clamp(_currentSlot + mw * -10, 0, slotsCount - 1);
            playerHUD.Set—urrentSlot(_currentSlot);
            #endregion
        }
    }
    [SerializeField] BaseBuildingPrefabClass pref;
    private void OnGUI()
    {
        if (GUILayout.Button("_______________________________________________Start"))
        {
            //GameObject x = new GameObject("b", typeof(DefBuilder));
            //x.GetComponent<DefBuilder>().BeginBuilding(this, pref);
            //defBuilder.BeginBuilding(this, pref);

        }
    }
}
