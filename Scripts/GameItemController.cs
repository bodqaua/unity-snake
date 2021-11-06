using UnityEngine;

public class GameItemController : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 25, 0) * Time.deltaTime);
    }
}
