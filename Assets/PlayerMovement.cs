using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference actionInput;
    [SerializeField] private InputActionReference attackInput;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket
    }
    public ToolType currentTool;

    public float toolWaitTime = .5f;
    public float toolWaitCounter;

    public Transform toolIndicator;
    public float toolRange = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UIController.instance.SwitchTool((int)currentTool);
    }

    // Update is called once per frame
    void Update()
    {
        if(UIController.instance.pauseScreen.gameObject.activeSelf == true)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (toolWaitCounter > 0)
        {
            toolWaitCounter -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }

        if(Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;
        }

        if(actionInput.action.WasPressedThisFrame())
        {
            UseTool();

            if((int)currentTool >= 4)
            {
                currentTool = ToolType.plough;
            }
        }

        bool hasSwitchedTool = false;
        
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            {
                currentTool = ToolType.plough;
            }
            
            hasSwitchedTool = true;
        }
        
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringCan;

            hasSwitchedTool = true;
        }
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;

            hasSwitchedTool = true;
        }
        if(Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;

            hasSwitchedTool = true;
        }

        if (hasSwitchedTool == true)
        {
            UIController.instance.SwitchTool((int)currentTool);
        }

        if (GridController.instance != null)
        {
            if (actionInput.action.WasPressedThisFrame())
            {
                UseTool();
            }

            toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y,0f);
            
            if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
            {
                Vector2 direction = toolIndicator.position - transform.position;
                direction = direction.normalized * toolRange;
                toolIndicator.position = transform.position + new Vector3(direction.x,direction.y,0f);
            }

            toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + .5f, 
            Mathf.FloorToInt(toolIndicator.position.y) + .5f, 
            0f);
        } else
        {
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }
    }

    void UseTool()
    {
        GrowBlock block = null;

        //block = FindFirstObjectByType<GrowBlock>();

        //block.PloughSoil();

        block = GridController.instance.GetBlock(toolIndicator.position.x - .5f, toolIndicator.position.y -.5f);

        toolWaitCounter = toolWaitTime;

        if(block != null)
        {
            switch(currentTool)
            {
                case ToolType.plough:

                block.PloughSoil();
                
                break;

                case ToolType.wateringCan:

                block.WaterSoil();


                break;

                case ToolType.seeds:

                block.PlantCrop();


                break;

                case ToolType.basket:

                block.HarvestCrop();


                break;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }
}
