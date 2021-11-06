using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeBehaviorController : MonoBehaviour
{
    public GameObject CubeBasicObject;
    public GameObject AreaObject;

    private readonly float yAxis = 0.2f;
    private readonly int wallPadding = 1;
    private readonly float generateTimeRange = 3;
    private float generateTimeRangeCounter = 3;

    private float zAxisStart;
    private float zAxisEnd;
    private float xAxisStart;
    private float xAxisEnd;

    void Start()
    {
        this.CountAreaRestrictions();
        this.GenerateRandomCube();
    }

    void Update()
    {
        this.GenerateRandomCubeTimeoout();
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

    private void GenerateRandomCube()
    {
        float x = Random.Range(this.xAxisStart, this.xAxisEnd);
        float z = Random.Range(this.zAxisStart, this.zAxisEnd);
        Instantiate(this.CubeBasicObject, new Vector3(x, this.yAxis, z), Quaternion.identity);
    }

    private void GenerateRandomCubeTimeoout()
    {
        if (this.generateTimeRangeCounter > 0)
        {
            this.generateTimeRangeCounter -= Time.deltaTime;
            return;
        }

        this.generateTimeRangeCounter = this.generateTimeRange;
        this.GenerateRandomCube();
    }
}
