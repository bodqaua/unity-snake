using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;

    private int count;
    private int cubeCount;

    private Rigidbody rb;

    private int currentDirection = 0;
    private Vector3[] directions = new[] { Vector3.left, Vector3.forward, Vector3.right, Vector3.back };

    private string GameItemTag = "gameItem";

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.ResetCounter();
    }

    private void Update()
    {
        this.cubeCount = GameObject.FindGameObjectsWithTag(this.GameItemTag).Length;
        this.UpdateCounter();
        this.ReadKeyCodes();
    }

    private void FixedUpdate()
    {
        transform.Translate(this.directions[this.currentDirection] * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CheckCollisionByTag(other, this.GameItemTag))
        {
            Destroy(other.gameObject);
            this.IncreaseCounter();
            this.UpdateCounter();
        }

        if(CheckCollisionByTag(other, "killBox"))
        {
            Destroy(this);
        }
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


    private void UpdateCounter()
    {
        this.countText.text = "Count: " + this.count.ToString();
    }

    private void ReadKeyCodes()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.TurnLeft();
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.TurnRight();
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
