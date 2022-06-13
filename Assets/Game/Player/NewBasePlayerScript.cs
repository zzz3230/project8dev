using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public enum Move { Moveable, NotMoveable }
public enum GameWidget { Hud, Inventory, Menu, Console }

public class NewBasePlayerScript : Character
{
    #region Singletone

    public static NewBasePlayerScript Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this.gameObject);
    }

    #endregion

    #region Public Properties

    public bool Moveable { get; set; } = true;

    public bool IsMoving
    {
        get => Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.z) > Mathf.Epsilon;
    }

    public float Speed
    {
        get => Input.GetKey(runKeyCode) ? runSpeed : walkSpeed;
    }

    #endregion

    Vector3 velocity = Vector3.zero;

    [Header("References")]
    public CharacterController controller;
    public Animator animator;
    public Camera _camera;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 8f;
    public Move air = Move.Moveable;
    public Move land = Move.NotMoveable;

    public KeyCode jumpKeyCode = KeyCode.Space;
    public KeyCode runKeyCode = KeyCode.LeftShift;


    Vector2 _cameraRotation;
    int _currentSlot = 0;

    public void DecrementCountInHand(int c = 1)
    {
        var item = inventoryManager.GetSlot(_currentSlot).itemPointer[0];
        item.count -= c;

        //Log.Ms(item);

        if (item.count <= 0)
            item.empty = true;

        inventoryManager.GetSlot(_currentSlot).ViewUpdate();
        UpdateHUD();
    }

    /*public PlayerHUD_UI_Manager hudUIManager;
    public UIManager uiManager;
    public UIManager uiInventory;
    
    public PlayerInventoryUIManager inventoryUIManager;

    public UIManager uiPauseMenu;
    public PauseMenuUIManager pauseMenuUIManager;

    public UIManager uiConsoleMenu;
    public PauseMenuUIManager consoleUIManager;*/
    public PlayerInventoryManager inventoryManager;

    public PlayerHUDWidgetScript playerHUDWidgetScript;
    public PlayerInventoryWidgetScript playerInventoryWidgetScript;
    public PauseMenuWidgetScript pauseMenuWidgetScript;
    public ConsoleWidgetScript consoleWidgetScript;
    public PlayerDebugUIWidgetScript debugWidgetScript;
    bool isDebugWidgetActive;

    public HandItemManager handItemManager;


    //public float playerSpeed = 200;
    public bool isInputMouse = true;

    public int slotsCount = 27;

    NetworkVariable<Vector3> serverPosition = new NetworkVariable<Vector3>();

    [Header("Animator Settings")]
    [Range(.1f, 5f)] public float animatorMovementSpeed = 1f;

    GameWidget focusWidget = GameWidget.Hud;

    void ChangeGameState(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            HidePauseMenu();

        }
        else if (newState == GameState.Paused)
        {

            ShowPauseMenu();
        }
    }

    public void GiveItem(int slotIndex, string strId, int count)
    {
        inventoryManager.GetSlot(slotIndex).GiveItem(strId, count);
    }

    public void Start()
    {
        //Application.targetFrameRate = 60;
        //Debug.LogError(System.IO.Directory.GetCurrentDirectory());
        //Debug.LogError(System.IO.Directory.GetCurrentDirectory());
        //Debug.LogError(System.IO.Directory.GetCurrentDirectory());
        //handItemManager.defBuilderOriginal = d

        //pauseMenuUIManager = uiPauseMenu.Q<PauseMenuUIManager>("menu");
        RefHub.playerScript = this;
        Global.gameManager.gameStateChanged = ChangeGameState;

        ConsoleWidgetScript.instance.AddClassInstance(this, "player");

        playerInventoryWidgetScript.Init(this);

        //print(gameObject.GetUUID());

        AttachCommands();

        if (animator)
            animator.SetFloat("MovementSpeed", animatorMovementSpeed);

        //hudUIManager = uiManager.Q<PlayerHUD_UI_Manager>("hud");
        //inventoryUIManager = uiInventory.Q<PlayerInventoryUIManager>("inv");

        ChangeGameState(GameState.Playing);
        HideInventory();
        HidePauseMenu();
        HideConsoleWidget();

        //inventoryUIManager.loaded += (obj) =>
        //{

        SlotManager[] slots = new SlotManager[slotsCount];
        for (int i = 0; i < slots.Length; i++)
        {
            var itemPointer = ItemsManager.Allocate(1);
            var widget = playerInventoryWidgetScript.GetSlot(i);

            slots[i] = new SlotManager
            {
                itemPointer = itemPointer,
                widget = widget,
                playerScript = this
            };
            //print(itemPointer);
            widget.manager = slots[i];
        }

        inventoryManager.Init(
            playerInventoryWidgetScript,
            slots.Select(s => s.itemPointer).ToArray(),
            slots
            );

        //slots[1].itemPointer[0].info = BasicConsoleScript.Instance.debugItems[0];
        //slots[1].itemPointer[0].count = 23;
        //slots[1].itemPointer[0].empty = false;
        slots[1].GiveItem(SIID.game_testitem_1.ToString(), 23);
        slots[1].itemPointer[0].metadata = new UnitMetadata();
        slots[1].itemPointer[0].metadata.durability = 50;
        slots[1].itemPointer[0].metadata.maxDurability = 150;


        //slots[6].itemPointer[0].info = BasicConsoleScript.Instance.debugItems[1];
        //slots[6].itemPointer[0].count = 23;
        //slots[6].itemPointer[0].empty = false;
        slots[6].GiveItem(SIID.game_testitem_place_2.ToString(), 23);
        //slots[7].itemPointer[0].info = BasicConsoleScript.Instance.debugItems[2];
        //slots[7].itemPointer[0].count = 23;
        //slots[7].itemPointer[0].empty = false;
        slots[7].GiveItem(SIID.game_testitem_python_3.ToString(), 23);

        slots[0].GiveItem(SIID.game_brick_wall_01.ToString(), 10);

        slots[4].GiveItem(SIID.game_testitem_again_4.ToString(), 1);

        SlotManager[] slots2 = new SlotManager[9];
        for (int i = 0; i < slots2.Length; i++)
        {
            //var itemPointer = ItemsManager.Allocate(1);
            //print(i);
            var visualElement = playerHUDWidgetScript.GetSlot(i);

            slots2[i] = inventoryManager.GetSlot(i).NewShadow(visualElement);

        }

        foreach (var s in slots)
        {
            s.ViewUpdate();
            // Log.Ms(s.itemPointer.offset);
        }


        inventoryManager.MoveItem(slots[1], slots[2], 9);


        //hudUIManager.loaded += (obj) =>
        //{SlotManager[] 

        //inventoryManager.Init(
        //   hudUIManager, 
        //   slots.Select(s => s.itemPointer).ToArray(),
        //   slots
        //   );

        //}; 

        //inventoryUIManager.style.display = UnityEngine.UIElements.DisplayStyle.None;
        //};
        HideDebugWidget();

        ChangeCurrentHUDSlot(_currentSlot);

        LockMouse();
    }

    void AttachCommands()
    {
        //BasicConsole.AttachCom("beginbuild", () => defBuilder.BeginBuilding(this, pref));
        BasicConsole.AttachCom("log_clients", () =>
        {
            var res = "";
            var clients = Unity.Netcode.NetworkManager.Singleton.ConnectedClients;

            foreach (var c in clients)
            {
                res += $"dict_id {c.Key} : (c_id {c.Value.ClientId}; c_obj {c.Value.PlayerObject})\n";
            }
            print(res);
        });
    }
    public void SwitchDebugWidgetActive()
    {
        if (isDebugWidgetActive)
            HideDebugWidget();
        else
            ShowDebugWidget();
    }
    public void HideDebugWidget()
    {
        debugWidgetScript.Hide();
        isDebugWidgetActive = false;
    }
    public void ShowDebugWidget()
    {
        debugWidgetScript.Show();
        isDebugWidgetActive = true;
    }
    public void HideInventory()
    {
        playerInventoryWidgetScript.Hide();
        focusWidget = GameWidget.Hud;
        LockMouse();
    }
    public void ShowInventory()
    {
        playerInventoryWidgetScript.Show();
        focusWidget = GameWidget.Inventory;
        UnlockMouse();
    }
    void ShowPauseMenu()
    {
        HideFocuseWidget();
        //if(focusWidget == GameWidget.Inventory)
        //    HideInventory();
        //else
        //{
        pauseMenuWidgetScript.Show();
        focusWidget = GameWidget.Menu;
        UnlockMouse();
        //}
    }
    void HidePauseMenu()
    {
        pauseMenuWidgetScript.Hide();
        focusWidget = GameWidget.Hud;
        LockMouse();
    }
    void HideConsoleWidget()
    {
        consoleWidgetScript.Hide();
        focusWidget = GameWidget.Hud;
        LockMouse();
    }
    void ShowConsoleWidget()
    {
        consoleWidgetScript.Show();
        focusWidget = GameWidget.Console;
        UnlockMouse();
    }
    void HideFocuseWidget()
    {
        StInput.isTextFieldEditing = false;
        switch (focusWidget)
        {
            case GameWidget.Hud:
                return;
            //break;
            case GameWidget.Inventory:
                HideInventory();
                break;
            case GameWidget.Menu:
                HidePauseMenu();
                break;
            case GameWidget.Console:
                HideConsoleWidget();
                break;
        }
    }
    void ApplyItemUse(MouseButton btn)
    {
        handItemManager.UseItem(btn);
    }
    //int test_tomove = 8;
    //int test_index = 2;
    public new void Update()
    {
        base.Update();

        Gravity();

        if (air == Move.Moveable || IsGrounded)
            Movement();

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            inventoryManager.MoveItem(test_index, ++test_index, test_tomove);
            print("mving: " + test_tomove);
            test_tomove -= 2;
        }*/

        /*var prt = "";
        for (int i = 0; i < 27; i++)
        {
            prt += inventoryManager.GetSlot(i).itemPointer[0]?.count + "";
        }
        print(prt);*/
        if (Input.GetMouseButtonDown(0))
            ApplyItemUse(MouseButton.Left);
        else if (Input.GetMouseButtonDown(1))
            ApplyItemUse(MouseButton.Right);
        else if (Input.GetMouseButtonDown(2))
            ApplyItemUse(MouseButton.Middle);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (focusWidget == GameWidget.Inventory || focusWidget == GameWidget.Hud)
                ShowPauseMenu();
            else
                //HidePauseMenu();
                HideFocuseWidget();
        }

        if (StInput.GetKeyDown(KeyCode.T))
        {
            ShowConsoleWidget();
        }

        if (StInput.GetKeyDown(KeyCode.I))
        {
            if (focusWidget == GameWidget.Hud)
            {
                ShowInventory();
            }
            else if (focusWidget == GameWidget.Inventory)
            {
                HideInventory();
            }
        }


        if (StInput.GetKeyDown(KeyCode.N))
        {
            UnlockMouse();
        }
        else if (StInput.GetKeyDown(KeyCode.L))
        {
            LockMouse();
        }

        if (StInput.GetKeyDown(KeyCode.E))
        {
            SwitchMouseLocking();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SwitchDebugWidgetActive();
        }

        #region -move | +camera

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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            handItemManager.AppendBuilderRotation(mw * 90);
        }
        else
        {
            var newSlot = (int)Mathf.Clamp(_currentSlot + mw * -10, 0, 9 - 1);
            if (_currentSlot != newSlot)
            {
                ChangeCurrentHUDSlot(newSlot);
            }

        }
        #endregion
    }

    void ChangeCurrentHUDSlot(int newSlot)
    {
        void HandItemUpdated()
        {
            //print("Hand item updated");
            HUDSlotChanged();
        }

        inventoryManager.GetSlot(_currentSlot).UnbindOnUpdate(HandItemUpdated);
        inventoryManager.GetSlot(newSlot).BindOnUpdate(HandItemUpdated);

        _currentSlot = newSlot;

        HUDSlotChanged();

        playerHUDWidgetScript.SetÑurrentSlot(_currentSlot);
    }

    public void UpdateHUDView()
    {
        for (int i = 0; i < playerHUDWidgetScript.slotsCount; i++)
        {
            //playerInventoryWidgetScript.GetSlot(i).
            inventoryManager.GetSlot(i).ViewUpdate();
        }
    }
    public void UpdateItemDurabilityInHUD(bool onlyInHand)
    {
        if (onlyInHand)
            inventoryManager.GetSlot(_currentSlot).ViewUpdateDurability();
    }
    public void UpdateHUD()
    {
        HUDSlotChanged();
    }
    void HUDSlotChanged()
    {
        handItemManager.SetCurrentInHandItem(inventoryManager.GetSlot(_currentSlot).itemPointer);
        //print(inventoryManager.GetSlot(_currentSlot).itemPointer[0]?.ToString());
    }

    /*
    public void FixedUpdate()
    {
    }
   */
    void SwitchMouseLocking()
    {
        if (isInputMouse)
            UnlockMouse();
        else
            LockMouse();
    }
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInputMouse = true;
    }
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isInputMouse = false;
    }


    //[ServerRpc]
    void Gravity_ServerRpc_(bool jump)
    {
        //print(IsGrounded + " " + WasGrounded + " " + jump);
        //if (jump)
        //    print("true");
        float GROUNDED_VELOCITY = Physics.gravity.y;

        if (IsGrounded && velocity.y < 0)
            velocity.y = GROUNDED_VELOCITY;

        if (!IsGrounded && WasGrounded && velocity.y < 0)
            velocity.y = 0f;

        if (jump && IsGrounded)
        {
            //if (jumpClipsData.clips.Length > 0) { /*sound*/}
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (IsGrounded && !WasGrounded)
        {
            //if (landClipsData.clips.Length > 0)
            //{ /*sound*/ }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //return IsGrounded;
    }
    void Gravity()
    {
        //if (!isServer)
        //    CallOnServer(nameof(S_Gravity), true, Input.GetButtonDown("Jump"))
        //        .Then((o) => IsGrounded = (bool)o);
        //else
        //    S_Gravity(Input.GetButtonDown("Jump"));


        var jump = StInput.GetButtonDown("Jump");
        //if (jump) print("j");
        Gravity_ServerRpc_(jump);

        if (animator)
            animator.SetBool("Float", !IsGrounded);
    }


    //[ServerRpc]
    void Movement_ServerRpc_(float x, float y)
    {
        var movement = transform.right * x + transform.forward * y;
        controller.Move(movement * Speed * Time.deltaTime);
    }

    void Movement()
    {
        if (Moveable)
        {
            var x = StInput.GetAxis("Horizontal");
            var y = StInput.GetAxis("Vertical");

            //if(!IsServer)
            //CallOnServer(nameof(S_Movement), false, x, y);
            //else
            //    S_Movement(x, y);
            Movement_ServerRpc_(x, y);

            if (Mathf.Abs(x) + Mathf.Abs(y) != 0f && IsGrounded)
            {
                // sound
            }

            if (animator)
            {
                var strength = (Speed == walkSpeed ? 1f : 2f);
                animator.SetFloat("Movement", y * strength);
                animator.SetFloat("Direction", x * strength);
            }
        }
    }


    [SerializeField] public BaseBuildingPrefabClass pref;
}
