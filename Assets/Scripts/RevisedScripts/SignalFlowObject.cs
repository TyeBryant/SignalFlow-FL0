using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;
using System.Linq;

public class SignalFlowObject : MonoBehaviour
{
    //The current node that the signal is on
    //[HideInInspector]
    public GameObject currentNode;

    //The node that the signal came from
    //[HideInInspector]
    public GameObject previousNode;

    //The movement that the signal has to perform to get on the current node
    public Transform nodePoweredTransform;

    //[HideInInspector]
    public List<GameObject> previousNodeList;

    public GameObject signalFlowObjectType;

    public GameManager gameManager;

    //Something that needs to be assigned to new ones as well
    [HideInInspector]
    public GameObject StartingNode;

    //Bools to check if they are on multi-input/output nodes
    //[HideInInspector]
    public bool onDaw;

    //[HideInInspector]
    public bool dawReady;

    //Patch bay bools
    //[HideInInspector]
    public bool onPatchBay;
    //[HideInInspector]
    public bool patchBayReady;

    //Junction box bools
    //[HideInInspector]
    public bool onJunctionBox;
    //[HideInInspector]
    public bool junctionBoxReady;

    //DAW bools
    [HideInInspector]
    public int dawInt;
    [HideInInspector]
    public int patchBayInt;
    [HideInInspector]
    public int junctionBoxInt;

    [HideInInspector]
    public int outputCount;
    [HideInInspector]
    public int genericInt;

    [HideInInspector]
    public bool countCheck;

    public int objectSignalNumber;

    public List<GameObject> deleteList;

    public List<LineRendCol> linesWeNeed = new List<LineRendCol>();

    public List<LineRendCol> conRends;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        onDaw = false;
        onPatchBay = false;
        onJunctionBox = false;
        countCheck = false;
    }

    //void recurse(GameObject current)
    //{
    //    conRends = FindObjectsOfType<LineRendCol>().ToList();

    //        foreach (LineRendCol line in conRends)
    //        {
    //            if (line.outNode.GetComponent<aNode>().prevNodeSignalNumber == objectSignalNumber)
    //            {
    //                if (line.node.gameObject == current)
    //                {
    //                line.node.PlaceSignal(line.outNode);
    //                    /*line.outNode.GetComponent<aNode>().nodeSignalNumber = line.outNode.GetComponent<aNode>().prevNodeSignalNumber;
    //                    previousNodeList.Add(currentNode);
    //                    previousNode = currentNode;*/
    //                    recurse(line.outNode);
    //                    return;
    //                }
    //            }
    //        }
           
    //        transform.position = current.transform.position;

    //        currentNode = current;
    //}

    // Update is called once per frame
    void Update()
    {
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_MICROPHONE)
        {
            previousNodeList.Clear();
        }

        //Some win checking stuff
        if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_DAW || currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY || currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_JUNCTIONBOX)
        {
            currentNode.GetComponent<aNode>().nodeSignalNumber = objectSignalNumber;
        }

        //Checking if the current node is a DAW node
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_DAW)
        {
            onDaw = true;
        }
        else if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_DAW)
        {
            onDaw = false;
            dawReady = false;
        }

        //Checking if the current node is a Patchbay node
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_PATCHBAY)
        {
            onPatchBay = true;
        }
        else if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY)
        {
            onPatchBay = false;
            patchBayReady = false;
        }

        //Checking if the current node is a JunctionBox node
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_JUNCTIONBOX)
        {
            onJunctionBox = true;
        }
        else if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_JUNCTIONBOX)
        {
            onJunctionBox = false;
            junctionBoxReady = false;
        }

        // ---- For single-input/potential multi-output nodes ---- //
        if (currentNode.GetComponent<aNode>().outputs.Count != 0 && onDaw == false && onPatchBay == false && onJunctionBox == false && currentNode.GetComponent<aNode>().outputs[0].GetComponent<aNode>().nodeSignalNumber == 0 && currentNode.GetComponent<aNode>().sendingSignal == true)
        {
            //Make the positions the first index of the output list
            //More outputs can instantiate a signal instead
            nodePoweredTransform = currentNode.GetComponent<aNode>().outputs[0].transform;
            this.transform.position = nodePoweredTransform.position;

            //Setting the previous node as the node we just came from
            previousNode = currentNode;
            previousNodeList.Add(previousNode);

            //Adding things to the game manager - win state shit
            aNode.Type type = previousNode.GetComponent<aNode>().nodeType;

            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[0];
            currentNode = currentNodeHolder;
            currentNode.GetComponent<aNode>().sendingSignal = true;
        }

        //if (currentNode.GetComponent<aNode>().outputs[0].GetComponent<aNode>().nodeSignalNumber == this.objectSignalNumber && onDaw == false && onPatchBay == false && onJunctionBox == false && currentNode.GetComponent<aNode>().isPowered == true)
        //{

        //}

        //for (int i = 0; i < currentNode.GetComponent<aNode>().connectionRenderers.Count; ++i)
        //{




            //conRends = FindObjectsOfType<LineRendCol>().ToList();

            ////aNode nodething = null;



            ////if (nodething != null)
            //{
            //    foreach (LineRendCol line in conRends)
            //    {
            //        if (line.node.gameObject == currentNode)
            //        {
            //            if (line.outNode.GetComponent<aNode>().prevNodeSignalNumber == objectSignalNumber)
            //            {
            //                previousNodeList.Add(currentNode);
            //                line.outNode.GetComponent<aNode>().nodeSignalNumber = line.outNode.GetComponent<aNode>().prevNodeSignalNumber;
            //                transform.position = line.outNode.transform.position;
            //                previousNode = currentNode;
            //                currentNode = line.outNode.GetComponent<aNode>().gameObject;
            //            }
            //        }
            //    }
            //}
        //}
        
        // ---- DAW FUNCTIONALITY ---- //
        if (onDaw == true && dawReady == false)
        {
            //If the cube input number is less that subDaw count, stops recurring list adding
            if (currentNode.GetComponent<aDAW>().cubeInputs.Count < currentNode.GetComponent<aDAW>().subDaws.Count)
            {
                if (!currentNode.GetComponent<aDAW>().signalNumbers.Contains(this.objectSignalNumber))
                {
                    currentNode.GetComponent<aDAW>().signalNumbers.Add(this.objectSignalNumber);
                }
                //Adding one signal cube to the list
                currentNode.GetComponent<aDAW>().cubeInputs.Add(this.gameObject);

                //Signal object moves to the specific miniDAW position
                int i = currentNode.GetComponent<aDAW>().cubeInputs.Count - 1;
                dawInt = i;
                this.transform.position = currentNode.GetComponent<aDAW>().subDaws[i].transform.position;

                dawReady = true;
            }
        }
        if (dawReady == true)
        {
            if (currentNode.GetComponent<aDAW>())
            {
                //If the selected index is the one that you choose
                if (currentNode.GetComponent<aDAW>().selectedIndex == dawInt)
                {
                    if (countCheck == false)
                    {
                        genericInt = currentNode.GetComponent<aDAW>().outputs.Count;
                        countCheck = true;
                    }

                    if (genericInt < currentNode.GetComponent<aDAW>().outputs.Count && countCheck == true)
                    {
                        int i = currentNode.GetComponent<aDAW>().outputs.Count - 1;
                        this.transform.position = currentNode.GetComponent<aDAW>().outputs[i].transform.position;

                        previousNode = currentNode;
                        previousNodeList.Add(previousNode);

                        //Adding things to the game manager - win state shit
                        aNode.Type type = previousNode.GetComponent<aNode>().nodeType;

                        GameObject currentNodeHolder = currentNode.GetComponent<aDAW>().outputs[i];
                        currentNode = currentNodeHolder;

                        onDaw = false;
                        dawReady = false;
                        countCheck = false;
                        dawInt = 0;
                        genericInt = 0;

                        previousNode.GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                    }
                }
                else
                {
                    countCheck = false;
                    genericInt = 0;
                }
            }
        }

        // ---- PATCHBAY FUNCTIONALITY ---- //
        if (onPatchBay == true && patchBayReady == false)
        {
            //If the cube input number is less than mini patchBay count, stops recurring list adding
            if (currentNode.GetComponent<aPatchBay>().cubeInputs.Count < currentNode.GetComponent<aPatchBay>().subNodes.Count)
            {
                if (!currentNode.GetComponent<aPatchBay>().signalNumbers.Contains(this.objectSignalNumber))
                {
                    currentNode.GetComponent<aPatchBay>().signalNumbers.Add(this.objectSignalNumber);
                }
                //Adding one signal cube to the list
                currentNode.GetComponent<aPatchBay>().cubeInputs.Add(this.gameObject);

                //Signal object moves to the specific subNode position
                int i = currentNode.GetComponent<aPatchBay>().cubeInputs.Count - 1;
                patchBayInt = i;
                this.transform.position = currentNode.GetComponent<aPatchBay>().subNodes[i].transform.position;

                patchBayReady = true;
            }
        }

        if (patchBayReady == true)
        {
            if (currentNode.GetComponent<aPatchBay>())
            {
                //If the selected index is the one you choose
                if (currentNode.GetComponent<aPatchBay>().selectedIndex == patchBayInt)
                {
                    if (countCheck == false)
                    {
                        genericInt = currentNode.GetComponent<aPatchBay>().outputs.Count;
                        countCheck = true;
                    }

                    if (genericInt < currentNode.GetComponent<aPatchBay>().outputs.Count && countCheck == true)
                    {
                        int i = currentNode.GetComponent<aPatchBay>().outputs.Count - 1;
                        this.transform.position = currentNode.GetComponent<aPatchBay>().outputs[i].transform.position;

                        previousNode = currentNode;
                        previousNodeList.Add(previousNode);

                        //Adding things to the game manager - win state shit
                        aNode.Type type = previousNode.GetComponent<aNode>().nodeType;

                        GameObject currentNodeHolder = currentNode.GetComponent<aPatchBay>().outputs[i];
                        currentNode = currentNodeHolder;

                        onPatchBay = false;
                        patchBayReady = false;
                        patchBayInt = 0;
                        genericInt = 0;

                        previousNode.GetComponent<aPatchBay>().cubeInputs.Remove(this.gameObject);
                    }
                }
                else
                {
                    countCheck = false;
                    genericInt = 0;
                }
            }

        }

        // ---- JUNCTION BOX FUNCTIONALITY ---- //
        if (onJunctionBox == true && junctionBoxReady == false)
        {
            if(currentNode.GetComponent<aDAW>()==null)
            {
                int dummy = 0;
            }
            if (currentNode.GetComponent<aDAW>().outputs.Count > 0)
            {
                for (int index = 0; index < currentNode.GetComponent<aDAW>().outputs.Count; index++)
                {
                    if (currentNode.GetComponent<aDAW>().outputs[index].GetComponent<aNode>().nodeSignalNumber == this.objectSignalNumber)
                    {
                        currentNode = currentNode.GetComponent<aDAW>().outputs[index];
                    }
                }
            }
            //If the cube input number is less that subDaw count, stops recurring list adding
            if (currentNode.GetComponent<aDAW>().cubeInputs.Count < currentNode.GetComponent<aDAW>().subDaws.Count)
            {
                if (!currentNode.GetComponent<aDAW>().signalNumbers.Contains(this.objectSignalNumber))
                {
                    currentNode.GetComponent<aDAW>().signalNumbers.Add(this.objectSignalNumber);
                }
                //Adding one signal cube to the list
                currentNode.GetComponent<aDAW>().cubeInputs.Add(this.gameObject);

                //Signal object moves to the specific miniDAW position
                int i = currentNode.GetComponent<aDAW>().cubeInputs.Count - 1;
                junctionBoxInt = i;
                this.transform.position = currentNode.GetComponent<aDAW>().subDaws[i].transform.position;

                junctionBoxReady = true;
            }
        }
        if (junctionBoxReady == true)
        {
            if (currentNode.GetComponent<aDAW>())
            {
                //If the selected index is the one that you choose
                if (currentNode.GetComponent<aDAW>().selectedIndex == junctionBoxInt)
                {
                    if (countCheck == false)
                    {
                        genericInt = currentNode.GetComponent<aDAW>().outputs.Count;
                        countCheck = true;
                    }

                    if (genericInt < currentNode.GetComponent<aDAW>().outputs.Count && countCheck == true)
                    {
                        int i = currentNode.GetComponent<aDAW>().outputs.Count - 1;
                        this.transform.position = currentNode.GetComponent<aDAW>().outputs[i].transform.position;

                        previousNode = currentNode;
                        previousNodeList.Add(previousNode);

                        //Adding things to the game manager - win state shit
                        aNode.Type type = previousNode.GetComponent<aNode>().nodeType;

                        GameObject currentNodeHolder = currentNode.GetComponent<aDAW>().outputs[i];
                        currentNode = currentNodeHolder;

                        onJunctionBox = false;
                        junctionBoxReady = false;
                        countCheck = false;
                        junctionBoxInt = 0;
                        genericInt = 0;

                        previousNode.GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                    }
                }
                else
                {
                    countCheck = false;
                    genericInt = 0;
                }
            }
        }

        //If list is empty and current node doesn't equal mic (start), kill yourself
        if (previousNodeList.Count == 0 && currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
        {
            Destroy(this.gameObject);
        }

        //Setting the material of the signal
        currentNode.GetComponent<aNode>().signalObject = signalFlowObjectType;

        //If there is nothing set in this signalflowobjecttype 
        if (signalFlowObjectType == null)
        {
            if (previousNode == null)
            {
                //Get a signal flow type from the node
                signalFlowObjectType = currentNode.GetComponent<aNode>().signalObject;
            }
            else
            {
                signalFlowObjectType = previousNode.GetComponent<aNode>().signalObject;
            }
        }

        //Backtracking?
        if (currentNode.GetComponent<aNode>().nodeType != aNode.Type.ET_MICROPHONE)
        {
            if (currentNode.GetComponent<aNode>().isPowered == false)
            {
                if (onDaw)
                {
                    if (currentNode != null && previousNode != null)
                    {
                        currentNode.GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                        currentNode.GetComponent<aDAW>().signalNumbers.Remove(this.objectSignalNumber);
                    }
                }

                if (onPatchBay)
                {
                    if (currentNode != null && previousNode != null)
                    {
                        currentNode.GetComponent<aPatchBay>().cubeInputs.Remove(this.gameObject);
                        currentNode.GetComponent<aPatchBay>().signalNumbers.Remove(this.objectSignalNumber);
                    }

                }
                if (onJunctionBox)
                {
                    if (currentNode != null && previousNode != null)
                    {
                        currentNode.GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                        currentNode.GetComponent<aDAW>().signalNumbers.Remove(this.objectSignalNumber);
                    }
                }

                int index = previousNodeList.Count - 1;
                if (index >= 0)
                {
                    int removeIndex = previousNodeList.Count;
                    currentNode = previousNodeList[index];
                    this.transform.position = currentNode.transform.position;

                    previousNodeList.RemoveAt(index);
                }
            }
        }

        for (int index = 0; index < previousNodeList.Count; index++)
        {
            if (previousNodeList[index].GetComponent<aNode>().isPowered == false)
            {
                currentNode.GetComponent<aNode>().nodeSignalNumber = 0;

                GameObject transform = previousNodeList[index - 1];
                currentNode = transform;
                this.transform.position = currentNode.transform.position;

                for (int i = index - 1; i < previousNodeList.Count; i++)
                {
                    int count = previousNodeList.Count - (index - 1);
                    if (deleteList.Count < count)
                    {
                        deleteList.Add(previousNodeList[i]);
                    }
                }

                for (int j = 0; j < deleteList.Count; j++)
                {
                    if (previousNodeList.Contains(deleteList[j]))
                    {
                        if (deleteList[j].GetComponent<aNode>().nodeType != aNode.Type.ET_DAW && deleteList[j].GetComponent<aNode>().nodeType != aNode.Type.ET_PATCHBAY && deleteList[j].GetComponent<aNode>().nodeType != aNode.Type.ET_JUNCTIONBOX)
                        {
                            deleteList[j].GetComponent<aNode>().nodeSignalNumber = 0;
                            deleteList[j].GetComponent<aNode>().sendingSignal = false;
                            deleteList[j].GetComponent<aNode>().signalObject = null;
                        }

                        if (deleteList[j].GetComponent<aNode>().nodeType == aNode.Type.ET_DAW)
                        {
                            deleteList[j].GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                            deleteList[j].GetComponent<aDAW>().signalNumbers.Remove(this.objectSignalNumber);
                        }

                        if (deleteList[j].GetComponent<aNode>().nodeType == aNode.Type.ET_PATCHBAY)
                        {
                            deleteList[j].GetComponent<aPatchBay>().cubeInputs.Remove(this.gameObject);
                            deleteList[j].GetComponent<aPatchBay>().signalNumbers.Remove(this.objectSignalNumber);
                        }

                        if (deleteList[j].GetComponent<aNode>().nodeType == aNode.Type.ET_JUNCTIONBOX)
                        {
                            deleteList[j].GetComponent<aDAW>().cubeInputs.Remove(this.gameObject);
                            deleteList[j].GetComponent<aDAW>().signalNumbers.Remove(this.objectSignalNumber);
                        }
                        previousNodeList.Remove(deleteList[j]);
                    }
                }
                deleteList.Clear();
            }
        }
    }
}
