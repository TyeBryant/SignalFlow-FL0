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
        Vector2 c3 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y - lineRend.endWidth / 2)) * 0.7f;
        Vector2 c4 = transform.InverseTransformPoint(new Vector2(endPoint.x, endPoint.y + lineRend.endWidth / 2)) * 0.7f;
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
        //GameObject[] sigPos = GameObject.FindGameObjectsWithTag("SignalPosition");
        //for (int index = 0; index < sigPos.Length; index++)
        //{
        //    sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aNode>().isPowered = false;
        //    if (sigPos[index].GetComponent<SignalFlowObject>().objectSignalNumber == node.nodeSignalNumber)
        //    {
        //        for (int i = 0; i < sigPos[index].GetComponent<SignalFlowObject>().previousNodeList.Count; ++i)
        //        {
        //            if (sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().inputs.Count <= 1 && sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i] != node)
        //            {
        //                sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().sendingSignal = false;
        //                sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().prevNodeSignalNumber = sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().nodeSignalNumber;
        //                sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().nodeSignalNumber = 0;
        //                if (!sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aDAW>() && !sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aPatchBay>())
        //                    sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().nodeSignalNumber = 0;
        //                else
        //                {
        //                    sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aNode>().prevNodeSignalNumber = sigPos[index].GetComponent<SignalFlowObject>().objectSignalNumber;

        //                    if (sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aDAW>())
        //                            sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aDAW>().signalNumbers.Remove(sigPos[index].GetComponent<SignalFlowObject>().objectSignalNumber);
        //                    else if (sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aPatchBay>())
        //                            sigPos[index].GetComponent<SignalFlowObject>().previousNodeList[i].GetComponent<aPatchBay>().signalNumbers.Remove(sigPos[index].GetComponent<SignalFlowObject>().objectSignalNumber);
        //                }
        //            }
        //        }

        //        sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aNode>().sendingSignal = false;

        //        if (sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aDAW>())
        //            sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aDAW>().cubeInputs.Remove(sigPos[index]);

        //        if (sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aPatchBay>())
        //            sigPos[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aPatchBay>().cubeInputs.Remove(sigPos[index]);

        //        sigPos[index].GetComponent<SignalFlowObject>().currentNode = node.gameObject;
        //        sigPos[index].transform.position = node.gameObject.transform.position;

        //        node.sendingSignal = true;
        //    }
        //}

        node.outputs.Remove(outNode);
        aNode s = outNode.GetComponent<aNode>();

        s.sendingSignal = false;
        if (!s.GetComponent<aDAW>())
        {
            //Make sure s is not powered anymore            
            s.inputs.Remove(node.gameObject);
            if (s.inputs.Count == 0)
                s.isPowered = false;
        }
        else
        {
            while (s.GetComponent<aDAW>().signalObjs.Contains(node.signalObject))
            {
                s.connectionRenderers.RemoveAt(s.GetComponent<aDAW>().signalObjs.IndexOf(node.signalObject));
                s.GetComponent<aDAW>().signalObjs.Remove(node.signalObject);
            }
            //Make sure s is not powered anymore            
            s.inputs.Remove(node.gameObject);
            if (s.inputs.Count == 0)
                s.isPowered = false;
        }

    }
}
