using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        if (this.player == null)
        {
            return;
        }
        this.offset = transform.position - this.player.transform.position;
    }

    void LateUpdate()
    {
        if (this.player == null)
        {
            return;
        }
        transform.position = this.player.transform.position + this.offset;
    }
}
