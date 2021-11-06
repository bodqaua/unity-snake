using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;

    private int count;
    private int cubeCount;

    private Rigidbody rb;

    private string GameItemTag = "gameItem";

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.ResetCounter();
        this.cubeCount = GameObject.FindGameObjectsWithTag(this.GameItemTag).Length;
        this.UpdateCounter();
    }
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CheckCollisionByTag(other, this.GameItemTag))
        {
            Destroy(other.gameObject);
            this.IncreaseCounter();
            this.UpdateCounter();
        }
    }

    private bool CheckCollisionByTag(Collider collider, string type)
    {
        return collider.tag == type;
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
        this.countText.text = "Count: " + count.ToString();
    }
}
