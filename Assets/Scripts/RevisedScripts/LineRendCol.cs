using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendCol : MonoBehaviour
{

    //[HideInInspector]
    public Vector3 startPoint, endPoint;

    public LineRenderer lineRend;

    public aNode node;

    private bool isDragging = false;

    private PolygonCollider2D polyCol2D;

    // Use this for initialization
    void Start()
    {
        polyCol2D = gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 c1 = transform.InverseTransformPoint(new Vector2(startPoint.x, startPoint.y + lineRend.startWidth / 2));
        Vector2 c2 = transform.InverseTransformPoint(new Vector2(startPoint.x, startPoint.y - lineRend.startWidth / 2));
        Vector2 c3 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y - lineRend.endWidth / 2));
        Vector2 c4 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y + lineRend.endWidth / 2));
        polyCol2D.SetPath(0, new Vector2[] { c1, c2, c3, c4 });

        if (isDragging)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRend.SetPosition(1, endPoint);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            node.Disconnect();
        }
        else if (Input.GetMouseButtonDown(1))
            Destroy(this.gameObject);
    }
}
