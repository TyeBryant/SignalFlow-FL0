using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendCol : MonoBehaviour
{

    //[HideInInspector]
    public Vector3 startPoint, endPoint;

    public LineRenderer lineRend;

    public aNode node;
    public GameObject outNode;
    private bool isDragging = false;

    private PolygonCollider2D polyCol2D;

    aConnectionManager conMan;

    // Use this for initialization
    void Start()
    {
        polyCol2D = gameObject.AddComponent<PolygonCollider2D>();
        conMan = FindObjectOfType<aConnectionManager>();
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
            endPoint = conMan.mousePointer.transform.position;
            lineRend.SetPosition(1, endPoint);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            Disconnect();

            conMan.CarrySignal(node.gameObject);
            Destroy(this.gameObject);
        }
    }
    
    //Manage Disconnecting
    void Disconnect()
    {
        node.outputs.Remove(outNode);

        //Make sure s is not powered anymore
        aNode s = outNode.GetComponent<aNode>();
        s.inputs.Remove(node.gameObject);
        if(s.inputs.Count == 0)
            s.isPowered = false;
    }
}
