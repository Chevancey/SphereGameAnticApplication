using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private GameObject triangleCursor;
    [SerializeField] private float radious;
    [SerializeField, Range(0, 1)] private float deceleration;
    [SerializeField] private float movementThreshold = 0.01f;

    private GameObject cursorInstance;
    private bool isMoving = false;

    // Vector variables for impulse
    private float impulseForce;
    private Vector3 impulseDirection;
    private float impulseDeceleration;
    private Vector3 previousPosition;

    //Point Attribution;
    public Color playerColor;
    public int totalPoints { get; private set; }
    private bool isAddUpPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        transform.localScale = new Vector3(radious, radious, radious);
        cursorInstance = Instantiate(triangleCursor);
        impulseDeceleration = 1 - deceleration;
        previousPosition = transform.position;

        Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        GetComponent<Renderer>().material.color = randomColor;
        playerColor = randomColor;
    }

    private void Update()
    {
        if (!PauseManager.Instance.isPaused || !GameManager.Instance.isGameOver)
        {
            Cursor.visible = false;
            UpdateCursorPosition();
            cursorInstance.SetActive(true);
        
            if (!isMoving && isAddUpPoints)
            {
                int subtractedPoints = 0;

                int addedPoints = EnemyManager.Instance.GetColorPoints()[playerColor];
                EnemyManager.Instance.GetColorPoints()[playerColor] = 0;

                foreach (var score in EnemyManager.Instance.GetColorPoints().Values)
                {
                    subtractedPoints += score;
                }

                totalPoints += addedPoints - subtractedPoints;
                GameManager.Instance.TurnCompleted();
                isAddUpPoints = false;
            }

            if (!isMoving && Input.GetMouseButtonUp(0))
            {
                EnemyManager.Instance.ClearColorPoints();
                PrepareImpulse();
            }

            if (isMoving)
            {
                ApplyImpulse();
            }

            previousPosition = transform.position;
        }
        else
        {
            Cursor.visible = false;
            cursorInstance.SetActive(false);
        }
    }

    private void PrepareImpulse()
    {
        Vector3 targetPosition = cursorInstance.transform.position;
        targetPosition.y = transform.position.y;

        impulseForce = Vector3.Distance(transform.position, targetPosition);
        impulseDirection = (targetPosition - transform.position).normalized;

        isMoving = true;
    }

    private void ApplyImpulse()
    {
        MoveObject(impulseForce, impulseDirection);
        impulseForce *= impulseDeceleration;

        if (!IsObjectMoving() || IsOutOfCameraView())
        {
            isMoving = false;
            isAddUpPoints = true;
        }
    }

    private void MoveObject(float force, Vector3 direction)
    {
        transform.position += force * direction * Time.deltaTime;
    }

    private void UpdateCursorPosition()
    {
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 1f;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        cursorInstance.transform.position = worldPosition;

        cursorInstance.transform.LookAt(transform.position);
    }

    private bool IsObjectMoving()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        return distanceMoved > movementThreshold;
    }

    private bool IsOutOfCameraView()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }

}

