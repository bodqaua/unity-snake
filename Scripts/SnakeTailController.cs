using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeTailController : MonoBehaviour
{
    public GameObject TailObjectPrefab;
    public GameObject SnakeHead;

    private List<SnakeTail> TailObjects = new List<SnakeTail>();
    private Vector3 direction = new Vector3(0, 0, 1);
    private readonly float distance = 0.5f;
    private readonly int InitialSnakeTailLength = 10;
    private Color currentColor = Color.gray;

    void Start()
    {
        PlayerController.OnGameObjectDestroy += this.TailObjectCreationSubscription;
        PlayerController.OnSnakeChangeDirection += this.ChangeDirectionSubscription;
        PlayerController.OnGameOverEvent += this.DestroySubscription;

        for (int i = 0; i < InitialSnakeTailLength; i++)
        {
            this.AddTailObject(false);
        }
        StartCoroutine(this.UpdateTagTimeout());
    }

    void Update()
    {
        this.UpdateTailPositions();
    }

    private IEnumerator UpdateTagTimeout()
    {
        this.updateAlltailsTag("nonTriggerable", true);
        yield return new WaitForSeconds(5);
        this.updateAlltailsTag("killBox", false);
    }

    private void updateAlltailsTag(string tag, bool updateFirst)
    {
        for (int i = updateFirst ? 0 : 1;  i < this.TailObjects.Count; i++)
        {
            this.TailObjects[i].UpdateTag(tag);
        }
    }

    private void TailObjectCreationSubscription(Color? color)
    {
        this.currentColor = (Color)(color != null ? color : this.currentColor);
        Debug.Log(this.currentColor);
        this.AddTailObject(true);
    }

    private void ChangeDirectionSubscription(Vector3 direction)
    {
        this.direction = direction * -1;
    }

    private void UpdateTailPositions()
    {
        if (TailObjects.Count == 0)
        {
            return;
        }
        this.TailObjects[0].UpdateCurrentPositionByObject(this.transform.parent.position, this.distance);
        float distance = Vector3.Distance(this.transform.parent.position, this.TailObjects[0].GetCurrentPosition());
        Vector3 newpos = this.transform.parent.position;
        float T = Time.deltaTime * distance * 4;

        if (T > 0.5f)
            T = 0.5f;
        this.TailObjects[0].UpdateCurrentPosition(newpos, T);

        for (int i = 1; i < this.TailObjects.Count; i++)
        {
            if (TailObjects.ElementAtOrDefault(i) != null)
            {
                this.TailObjects[i].UpdateCurrentPosition(this.TailObjects[i - 1].GetCurrentPosition(), T);
            }
        }
    }

    private void AddTailObject(bool toEnd)
    {
        Vector3 position = toEnd ? this.TailObjects.Last().GetCurrentPosition() : this.transform.parent.position;
       GameObject tailSection = Instantiate<GameObject>(this.TailObjectPrefab, position, Quaternion.identity);
        Renderer renderer = tailSection.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", this.currentColor);
        this.TailObjects.Add(new SnakeTail(tailSection));

        this.ReColorSnake();
    }

    private void ReColorSnake()
    {
        for (int i = 0; i < this.TailObjects.Count; i++)
        {
            this.TailObjects[i].UpdateColor(this.currentColor);
        }
    }

    private void DestroySubscription()
    {
        for (int i = 0; i < this.TailObjects.Count; i++)
        {
            Destroy(this.TailObjects[i].GetGameObject());
        }
        this.TailObjects.Clear();
    }
}

class SnakeTail
{
    Vector3 LastPosition { get; set; }
    GameObject TailSection { get; set; }

    public SnakeTail(GameObject tailSection)
    {
        this.TailSection = tailSection;
        this.UpdateLastPosition();
    }

    public void UpdateCurrentPositionByObject(Vector3 objectPosition, float distance)
    {
        if (this.TailSection == null) return;
        this.UpdateLastPosition();
    }


    public void UpdateCurrentPosition(Vector3 position, float T)
    {
        this.TailSection.gameObject.transform.position = Vector3.Slerp(this.TailSection.gameObject.transform.position, position, T);

    }

    public void UpdateLastPosition()
    {
        if (this.TailSection == null) return;
        this.LastPosition = this.TailSection.transform.position;
    }

    public Vector3 GetLastPosition()
    {
        return this.LastPosition;
    }

    public Vector3 GetCurrentPosition()
    {
        return this.TailSection.transform.position;
    }

    public void TranslateGameObject(Vector3 direction)
    {
        this.TailSection.transform.position += direction;
    }

    public void UpdateTag(string tag)
    {
        this.TailSection.transform.tag = tag;
    }
    
    public GameObject GetGameObject()
    {
        return this.TailSection;
    }

    public void UpdateColor(Color color)
    {
        Renderer renderer = this.TailSection.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }
}
