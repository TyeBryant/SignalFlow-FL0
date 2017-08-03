using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aDAW : aNode {

    public GameObject subDawObj;
    public float nodeRadiusSpacing;

    //[HideInInspector]
    public List<GameObject> signalObjs = new List<GameObject>();

    //[HideInInspector]
    public List<GameObject> subDaws = new List<GameObject>();
    public List<GameObject> cubeInputs = new List<GameObject>();

    //[HideInInspector]
    public int selectedIndex;

    // Update is called once per frame
    void Update()
    {
        //Only execute if this is above 0
        if (outputs.Count > 0)
        {
            ShowConnections();
        }
        
        if (connectionRenderers != null)
        {
            for (int index = 0; index < connectionRenderers.Count; index++)
            {
                counter += Time.deltaTime;
                if (counter > 0.5f && signalObjs.Count > 0)
                {
                    counter = 0;

                    signalObject = signalObjs[index];
                    GameObject tri1 = Instantiate(signalObject, this.gameObject.transform.position, Quaternion.identity);
                    tri1.GetComponent<LineShape>().positionA = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(0);
                    tri1.GetComponent<LineShape>().positionB = connectionRenderers[index].GetComponent<LineRenderer>().GetPosition(1);
                }
            }
        }
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();
        if (Input.GetMouseButtonUp(0))
        {

            foreach (GameObject obj in subDaws)
                Destroy(obj);

            subDaws.Clear();

            /*
            float angleStep = 2*Mathf.PI / inputs.Count;
            float angle = 0;

            for (int i = 0; i < inputs.Count; ++i)
            {

                Vector3 spawnPos = new Vector3(Mathf.Sin(angle)*nodeRadiusSpacing, Mathf.Cos(angle)*nodeRadiusSpacing, 1);
                GameObject obj = Instantiate(subDawObj, spawnPos, Quaternion.identity);
                obj.GetComponent<SubDAW>().hubDaw = GetComponent<aDAW>();
                obj.GetComponent<SubDAW>().selectedIndex = i;
                subDaws.Add(obj);
                angle += angleStep;
            }
            */

            for (int i = 0; i < inputs.Count; ++i)
            {

                Vector3 spawnPos = (inputs[i].transform.position - transform.position).normalized * nodeRadiusSpacing;
                GameObject obj = Instantiate(subDawObj, transform.position + spawnPos, Quaternion.identity);
                obj.GetComponent<SubDAW>().hubDaw = GetComponent<aDAW>();
                obj.GetComponent<SubDAW>().selectedIndex = i;
                subDaws.Add(obj);

            }
        }
    }

    public override void PlaceSignal(GameObject _outputTo)
    {
        if (inputs.Count > 0)
            signalObjs.Add(inputs[selectedIndex].GetComponent<aNode>().signalObject);
        base.PlaceSignal(_outputTo);
    }
}
