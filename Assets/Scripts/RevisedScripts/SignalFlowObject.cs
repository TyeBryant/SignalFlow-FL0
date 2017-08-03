using System.Collections;
using System.Collections.Generic;
using gameManagement;
using UnityEngine;

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

    [HideInInspector]
    public List<GameObject> previousNodeList;

    public GameObject signalFlowObjectType;

    public GameManager gameManager;

    //Bools to check if they are on multi-input/output nodes
    public bool onDaw;
    public bool dawReady;

    public bool onPatchBay;
    public bool patchBayReady;

    public int dawInt;
    public int patchBayInt;

    public int outputCount;
    public int genericInt;

    public bool countCheck;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        onDaw = false;
        onPatchBay = false;
        countCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the current node is a DAW node
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_DAW)
        {
            onDaw = true;
        }
        else
        {
            onDaw = false;
        }

        //Checking if the current node is a Patchbay node
        if (currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_PATCHBAY)
        {
            onPatchBay = true;
        }
        else
        {
            onPatchBay = false;
        }

        // ---- For single-input/potential multi-output nodes ---- //
        if (currentNode.GetComponent<aNode>().outputs.Count != 0 && onDaw == false && onPatchBay == false)
        {
            //Make the positions the first index of the output list
            //More outputs can instantiate a signal instead
            nodePoweredTransform = currentNode.GetComponent<aNode>().outputs[0].transform;
            this.transform.position = nodePoweredTransform.position;

            //Setting the previous node as the node we just came from
            previousNode = currentNode;
            previousNodeList.Add(previousNode);

            //Adding things to the game manager - win state shit
            gameManager.previousNodeList.Add(previousNode);
            aNode.Type type = previousNode.GetComponent<aNode>().nodeType;
            gameManager.previousNodeListTypes.Add(type);

            GameObject currentNodeHolder = currentNode.GetComponent<aNode>().outputs[0];
            currentNode = currentNodeHolder;
        }
        
        // ---- DAW FUNCTIONALITY ---- //
        if (onDaw == true && dawReady == false)
        {
            //If the cube input number is less that subDaw count, stops recurring list adding
            if (currentNode.GetComponent<aDAW>().cubeInputs.Count < currentNode.GetComponent<aDAW>().subDaws.Count)
            {
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
                    gameManager.previousNodeList.Add(previousNode);
                    aNode.Type type = previousNode.GetComponent<aNode>().nodeType;
                    gameManager.previousNodeListTypes.Add(type);

                    GameObject currentNodeHolder = currentNode.GetComponent<aDAW>().outputs[i];
                    currentNode = currentNodeHolder;

                    onDaw = false;
                    dawReady = false;
                    countCheck = false;
                    dawInt = 0;
                    genericInt = 0;
                }
            }
            if (currentNode.GetComponent<aDAW>().selectedIndex != dawInt)
            {
                countCheck = false;
                genericInt = 0;
            }
        }

        // ---- PATCHBAY FUNCTIONALITY ---- //
        if (onPatchBay == true && patchBayReady == false)
        {
            //If the cube input number is less than mini patchBay count, stops recurring list adding
            if (currentNode.GetComponent<aPatchBay>().cubeInputs.Count < currentNode.GetComponent<aPatchBay>().subNodes.Count)
            {
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
                    gameManager.previousNodeList.Add(previousNode);
                    aNode.Type type = previousNode.GetComponent<aNode>().nodeType;
                    gameManager.previousNodeListTypes.Add(type);

                    GameObject currentNodeHolder = currentNode.GetComponent<aPatchBay>().outputs[i];
                    currentNode = currentNodeHolder;

                    onPatchBay = false;
                    patchBayReady = false;
                    patchBayInt = 0;
                    genericInt = 0;
                }
            }

            if (currentNode.GetComponent<aPatchBay>().selectedIndex != patchBayInt)
            {
                countCheck = false;
                genericInt = 0;
            }
        }
    }
}
