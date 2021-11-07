using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject Player;
    void Start()
    {
        PlayerController.OnGameOverEvent += this.OnGameOverSubscription;
    }

    void OnGameOverSubscription()
    {
        Destroy(this.Player);
    }
}
