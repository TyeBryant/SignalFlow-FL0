using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

public class SignalFlowObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject currentNode;

    [HideInInspector]
    public GameObject previousNode;

    public Transform nodePoweredTransform;

    [HideInInspector]
    public List<GameObject> previousNodeList;

    public GameObject signalFlowObjectType;

    public GameManager gameManager;

    public bool onDaw;
    public bool onPatchBay;

    public int dawInt;
    public int patchBayInt;

    public int outputCount;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        onDaw = false;
        onPatchBay = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentNode.GetComponent<aNode>().outputs == null)
        //{
        //    return;
        //}

        if (previousNodeList == null)
        {
            return;
        }
        if (onDaw == false || onPatchBay == false)
        {
            if (currentNode.GetComponent<aNode>().outputs != null && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_DAW || currentNode.GetComponent<aNode>().outputs != null && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY)
            {
                for (int index = 0; index < currentNode.GetComponent<aNode>().outputs.Count; index++)
                {
                    if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType != aNode.Type.ET_DAW && currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY)
                    {
                        if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().signalObject == signalFlowObjectType || currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().signalObject == null)
                        {
                            nodePoweredTransform = currentNode.GetComponent<aNode>().outputs[index].transform;
                            this.transform.position = nodePoweredTransform.position;

                            previousNode = currentNode;
                            previousNodeList.Add(previousNode);

                            //Some game manager stuff
                            gameManager.previousNodeList.Add(previousNode);
                            aNode.Type type = previousNode.GetComponent<aNode>().nodeType;
                            gameManager.previousNodeListTypes.Add(type);

                            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[index];
                            currentNode = currentNodeHolder;
                        }
                    }
                    else if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType == aNode.Type.ET_DAW)
                    {
                        Debug.Log("I am a DAW");
                        if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().cubeInputs.Count < currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().subDaws.Count)
                        {
                            //Signal object moves to the specific mini DAW position
                            currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().cubeInputs.Add(this.gameObject);
                            int i = (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().cubeInputs.Count - 1);
                            dawInt = i;

                            this.transform.position = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().subDaws[i].transform.position;

                            //Adds appropriate DAW positions to the game manager win state
                            gameManager.previousNodeList.Add(currentNode.GetComponent<aNode>().outputs[index]);
                            aNode.Type type = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType;
                            gameManager.previousNodeListTypes.Add(type);

                            previousNode = currentNode;
                            previousNodeList.Add(previousNode);

                            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aDAW>().subDaws[i];
                            currentNode = currentNodeHolder;

                            onDaw = true;
                        }
                    }

                    else if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType == aNode.Type.ET_PATCHBAY)
                    {
                        // ----- NEED TO CHANGE PATCH BAY TO MIRROR DAW FUNCTIONALITY ----- //
                        Debug.Log("I'm a PatchBay");

                        if (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().cubeInputs.Count < currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().subNodes.Count)
                        {
                            //Signal object moves to the specific mini DAW position
                            currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().cubeInputs.Add(this.gameObject);
                            int i = (currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().cubeInputs.Count - 1);
                            patchBayInt = i;

                            this.transform.position = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().subNodes[i].transform.position;

                            //Adds appropriate subPatchBay positions to the game manager win state
                            gameManager.previousNodeList.Add(currentNode.GetComponent<aNode>().outputs[index]);
                            aNode.Type type = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aNode>().nodeType;
                            gameManager.previousNodeListTypes.Add(type);

                            previousNode = currentNode;
                            previousNodeList.Add(previousNode);

                            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[index].GetComponent<aPatchBay>().subNodes[i];
                            currentNode = currentNodeHolder;

                            onPatchBay = true;
                        }
                    }
                }
            }

            if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE && previousNodeList.Count != 0)
            {
                if (previousNode.GetComponent<aNode>().outputs[0] != currentNode)
                {
                    int index = previousNodeList.Count - 1;
                    int removeIndex = previousNodeList.Count;
                    currentNode = previousNodeList[index];
                    this.transform.position = currentNode.transform.position;

                    GameObject pn = previousNodeList[index];
                    aNode.Type pnType = pn.GetComponent<aNode>().nodeType;
                    previousNodeList.RemoveAt(index);

                    gameManager.previousNodeList.Remove(pn);
                    gameManager.previousNodeListTypes.Remove(pnType);
                }
            }
            //If list is empty and current node doesn't equal mic, kill yourself
            if (previousNodeList.Count == 0 && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
            {
                Destroy(this.gameObject);
            }

            //Setting the material of the signal
            currentNode.GetComponent<aNode>().signalObject = signalFlowObjectType;

            if (signalFlowObjectType == null)
            {
                if (previousNode == null)
                {
                    signalFlowObjectType = currentNode.GetComponent<aNode>().signalObject;
                }
                else
                {
                    signalFlowObjectType = previousNode.GetComponent<aNode>().signalObject;
                }
            }
        }

        if (onDaw == true)
        {
            if (currentNode.gameObject.tag == "subDAW")
            {
                GameObject currentNodeHolder = currentNode.GetComponent<SubDAW>().hubDaw.GetComponent<aDAW>().gameObject;
                currentNode = currentNodeHolder;
                outputCount = currentNode.GetComponent<aDAW>().outputs.Count;
            }

            if (currentNode.GetComponent<aDAW>().selectedIndex == dawInt)
            {
                if (outputCount < currentNode.GetComponent<aDAW>().outputs.Count)
                {
                    int i = currentNode.GetComponent<aDAW>().outputs.Count - 1;
                    this.transform.position = currentNode.GetComponent<aDAW>().outputs[i].transform.position;
                }
            }
        }

        if (onPatchBay == true)
        {
            if (currentNode.gameObject.tag == "subPatchBay")
            {
                GameObject currentNodeHolder = currentNode.GetComponent<subPB>().pb.GetComponent<aPatchBay>().gameObject;
                currentNode = currentNodeHolder;
                outputCount = currentNode.GetComponent<aPatchBay>().outputs.Count;
            }

            if (currentNode.GetComponent<aPatchBay>().selectedIndex == patchBayInt)
            {
                if (outputCount < currentNode.GetComponent<aPatchBay>().outputs.Count)
                {
                    int i = currentNode.GetComponent<aPatchBay>().outputs.Count - 1;
                    this.transform.position = currentNode.GetComponent<aPatchBay>().outputs[i].transform.position;
                }
            }
        }
    }
}
