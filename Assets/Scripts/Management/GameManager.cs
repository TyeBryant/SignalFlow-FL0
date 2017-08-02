using System.Collections;
using System.Collections.Generic;
using nodeFunctionality;
using System.Linq;
using UnityEngine;

namespace gameManagement
{
    public class GameManager : MonoBehaviour
    {
        public List<aNode.Type> signalRequirements;

        public List<aNode.Type> banList;

        //[HideInInspector]
        public List<aNode.Type> previousNodeListTypes;
        //[HideInInspector]
        public List<GameObject> previousNodeList;
        [HideInInspector]
        public List<GameObject> signalNodes;
        [HideInInspector]
        public bool gamePaused;

        public int numSignalWins;

        [HideInInspector]
        public int numSignalCheck;

        public bool win;

        [HideInInspector]
        public bool notOkay;

        // Use this for initialization
        void Start()
        {
            signalNodes = GameObject.FindGameObjectsWithTag("SignalPosition").ToList();

            win = false;
        }

        // Update is called once per frame
        void Update()
        {
            numSignalCheck = 0;
            for (int index = 0; index < signalNodes.Count; ++index)
            {
                if (signalNodes[index].GetComponent<SignalFlowObject>().currentNode.GetComponent<aNode>().nodeType == aNode.Type.ET_MONITOR)
                {
                    numSignalCheck += 1;
                    Debug.Log("Yes");
                    if (numSignalCheck == numSignalWins)
                    {
                        CheckWin();
                    }
                }
            }
        }

        public void CheckWin()
        {
            notOkay = banList.Any(i => previousNodeListTypes.Contains(i));
            if (notOkay)
            {
                Debug.Log("no");
            }
            if (!notOkay)
            {
                win = signalRequirements.All(i => previousNodeListTypes.Contains(i));
            }
        }
    }
}
