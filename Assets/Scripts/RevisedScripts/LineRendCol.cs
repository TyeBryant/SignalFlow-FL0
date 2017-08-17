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

    public AudioClip disconnectSound;

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
        Vector2 c3 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y - lineRend.endWidth / 2)) * 0.7f;
        Vector2 c4 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y + lineRend.endWidth / 2)) * 0.7f;
        polyCol2D.SetPath(0, new Vector2[] { c1, c2, c3, c4 });

        if (isDragging)
        {
            endPoint = conMan.mousePointer.transform.position;
            lineRend.SetPosition(1, endPoint);
        }
    }

    //void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        isDragging = true;
    //        //Disconnect();

    //        conMan.CarrySignal(node.gameObject);
    //        Destroy(this.gameObject);
    //    }
    //}
    
    //Manage Disconnecting

    //void Disconnect()
    //{

    void Disconnect()
    {
        Debug.Log("Disconnected.");
        AudioManager.Instance.PlayClip(disconnectSound, AudioManager.Instance.GetChannel("SFX"));

        node.outputs.Remove(outNode);
        aNode s = outNode.GetComponent<aNode>();


    //    node.outputs.Remove(outNode);
    //    aNode s = outNode.GetComponent<aNode>();

    //    s.sendingSignal = false;
    //    if (!s.GetComponent<aDAW>())
    //    {
    //        //Make sure s is not powered anymore            
    //        s.inputs.Remove(node.gameObject);
    //        if (s.inputs.Count == 0)
    //            s.isPowered = false;
    //    }
    //    else
    //    {
    //        while (s.GetComponent<aDAW>().signalObjs.Contains(node.signalObject))
    //        {
    //            s.connectionRenderers.RemoveAt(s.GetComponent<aDAW>().signalObjs.IndexOf(node.signalObject));
    //            s.GetComponent<aDAW>().signalObjs.Remove(node.signalObject);
    //        }
    //        //Make sure s is not powered anymore            
    //        s.inputs.Remove(node.gameObject);
    //        if (s.inputs.Count == 0)
    //            s.isPowered = false;
    //    }

    }
}
