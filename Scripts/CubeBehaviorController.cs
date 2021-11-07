using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeBehaviorController : MonoBehaviour
{
    public GameObject CubeBasicObject;
    public GameObject AreaObject;

    private Color[] colors = new Color[7] { 
        Color.red,
        Color.green,
        Color.gray,
        Color.white,
        Color.cyan,
        Color.magenta,
        Color.yellow
    };


    private readonly float yAxis = 0.2f;
    private readonly int wallPadding = 1;

    private float zAxisStart;
    private float zAxisEnd;
    private float xAxisStart;
    private float xAxisEnd;

    void Start()
    {
        this.CountAreaRestrictions();
        this.GenerateRandomCube(null);
    }

    void OnEnable()
    {
        PlayerController.OnGameObjectDestroy += this.GenerateRandomCube;
    }

    void Update()
    {
    }

    private void CountAreaRestrictions()
    {
        Bounds AreaBounds = AreaObject.GetComponent<Renderer>().bounds;
        this.xAxisEnd = AreaBounds.size.x - this.wallPadding;
        this.zAxisEnd = AreaBounds.size.z - this.wallPadding;

        Vector3 AreaCenter = AreaBounds.center;

        this.xAxisStart = this.xAxisEnd - (AreaCenter.x - this.wallPadding) * 2;
        this.zAxisStart = this.zAxisEnd- (AreaCenter.z - this.wallPadding) * 2;
    }

    public void GenerateRandomCube(Color? color)
    {
        float x = Random.Range(this.xAxisStart, this.xAxisEnd);
        float z = Random.Range(this.zAxisStart, this.zAxisEnd);
        GameObject cube = Instantiate(this.CubeBasicObject, new Vector3(x, this.yAxis, z), Quaternion.identity);
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", this.colors[Random.Range(1, 7)]);
    }
}
