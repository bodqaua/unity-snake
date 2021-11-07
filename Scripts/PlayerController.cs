using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText, GameOverText;

    private int count;


    private int currentDirection = 0;
    private Vector3[] directions = new[] { Vector3.left, Vector3.forward, Vector3.right, Vector3.back };

    public delegate void GameObjectDestroyAction(Color? color);
    public delegate void SnakeChangeDirection(Vector3 direction);
    public delegate void GameOverAction();
    public static event GameObjectDestroyAction OnGameObjectDestroy;
    public static event SnakeChangeDirection OnSnakeChangeDirection;
    public static event GameOverAction OnGameOverEvent;

    private string GameItemTag = "gameItem";
    private string KillBoxTag = "killBox";

    void Start()
    {
        this.ResetCounter();
    }

    private void Update()
    {
        this.UpdateCounter();
        this.ReadKeyCodes();
    }

    private void FixedUpdate()
    {
        transform.Translate(this.directions[this.currentDirection] * speed * Time.smoothDeltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(CheckCollisionByTag(other, this.GameItemTag))
        {
            Destroy(other.gameObject);
            this.IncreaseCounter();
            this.UpdateCounter();
            this.DispatchDestroyCube(other);
        }

        if(CheckCollisionByTag(other, this.KillBoxTag))
        {
            this.GameOver();
            OnGameOverEvent?.Invoke();
        }
    }

    private void DispatchDestroyCube(Collider other)
    {
        Renderer cubeRenderer = other.GetComponent<Renderer>();
        Renderer playerRenderer = this.GetComponent<Renderer>();

        Color color = cubeRenderer.material.GetColor("_Color");
        playerRenderer.material.SetColor("_Color", color);
        OnGameObjectDestroy?.Invoke(color);
    }

    private bool CheckCollisionByTag(Collider collider, string type)
    {
        return collider.tag.ToLower() == type.ToLower();
    }

    private void ResetCounter()
    {
        this.count = 0;
    }

    private void IncreaseCounter()
    {
        this.count++;
    }

    private void GameOver()
    {
        this.GameOverText.text = "Your score: " + this.count.ToString();
        this.countText.text = "";

    }


    private void UpdateCounter()
    {
        this.countText.text = "Count: " + this.count.ToString();
    }

    private void ReadKeyCodes()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.TurnLeft();
            OnSnakeChangeDirection?.Invoke(this.directions[this.currentDirection]);
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.TurnRight();
            OnSnakeChangeDirection?.Invoke(this.directions[this.currentDirection]);
            return;
        }
    }

    private void TurnLeft()
    {
        if(this.currentDirection - 1 >= 0)
        {
            this.currentDirection--;
        } else
        {
            this.currentDirection = 3;
        }
    }

    private void TurnRight()
    {
        if (this.currentDirection + 1 <= this.directions.Length - 1)
        {
            this.currentDirection++;
        }
        else
        {
            this.currentDirection = 0;
        }
    }
}
