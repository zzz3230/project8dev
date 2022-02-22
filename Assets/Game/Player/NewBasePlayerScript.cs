using Packtool;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum Move { Moveable, NotMoveable }

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
    //float movementSoundFXLength = 0f;

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
    int _currentSlot = 1;

    //public PlayerHUDWidgetScript playerHUDOriginal;
    //public PlayerHUDWidgetScript playerHUD;

    //public PlayerInventoryWidgetScript playerInventoryWidgetOriginal;
    //public PlayerInventoryWidgetScript playerInv;

    public PlayerHUD_UI_Manager hudUIManager;
    public UIManager uiManager;

    public DefBuilder defBuilderOriginal;
    public DefBuilder defBuilder;

    //public float playerSpeed = 200;
    public bool isInputMouse = true;

    public int slotsCount = 9;

    NetworkVariable<Vector3> serverPosition = new NetworkVariable<Vector3>();

    //[Header("Sound FXs")]
    //public AudioClipsData walkClipsData;
    //public AudioClipsData runClipsData;
    //public AudioClipsData landClipsData;
    //public AudioClipsData jumpClipsData;

    [Header("Animator Settings")]
    [Range(.1f, 5f)] public float animatorMovementSpeed = 1f;

    public void Start()
    {
        //if (!IsOwner) 
        //    return;

        print(gameObject.GetUUID());

        AttachCommands();

        if (animator)
            animator.SetFloat("MovementSpeed", animatorMovementSpeed);

        //playerHUD = Utils.SpawnWidget(playerHUDOriginal);
        //playerHUD.slotsCount = slotsCount;

        //playerInv = Utils.SpawnWidget(playerInventoryWidgetOriginal);
        //playerInv.Hide();

        hudUIManager = uiManager.Q<PlayerHUD_UI_Manager>("hud");



        defBuilder = Utils.SpawnBuilder(defBuilderOriginal);

        LockMouse();
    }

    void AttachCommands()
    {
        BasicConsole.AttachCom("beginbuild", () => defBuilder.BeginBuilding(this, pref));
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
    

    public void Update()
    {
        //if(Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count > 1)
        //    print(Unity.Netcode.NetworkManager.Singleton.ConnectedClients[1].PlayerObject.name);


        base.Update();

        //if (IsServer)
        //    serverPosition.Value = transform.position;

       // if (!IsOwner)
       //     return;


        //transform.position = serverPosition.Value;

        Gravity();

        if (air == Move.Moveable || IsGrounded)
            Movement();



        if (Input.GetKeyDown(KeyCode.N))
        {
            UnlockMouse();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LockMouse();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //playerInv.SwitchVisible();
            SwitchMouseLocking();
        }

        #region -move | +camera

        if (isInputMouse)
        {
            //print("cam move");
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
            var rot = defBuilder.rotation.eulerAngles;
            rot.y += mw * 15;
            defBuilder.rotation = Quaternion.Euler(rot);
        }
        else
        {
            _currentSlot = (int)Mathf.Clamp(_currentSlot + mw * -10, 0, slotsCount - 1);
            hudUIManager.Set—urrentSlot(_currentSlot);
        }
        #endregion
    }

    public void FixedUpdate()
    {
        //if(isServer)
        //    base.FixedUpdate();
    }
   
    void SwitchMouseLocking()
    {
        if (isInputMouse)
            UnlockMouse();
        else
            LockMouse();
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


        var jump = Input.GetButtonDown("Jump");
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
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

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


    [SerializeField] BaseBuildingPrefabClass pref;
    //private void OnGUI()
    //{
    //    if (GUILayout.Button("_______________________________________________Start"))
    //    {
    //        defBuilder.BeginBuilding(this, pref);
    //    }
    //}
}
