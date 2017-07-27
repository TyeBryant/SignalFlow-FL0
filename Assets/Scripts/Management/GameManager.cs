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

        [HideInInspector]
        public List<aNode.Type> previousNodeListTypes;
        [HideInInspector]
        public List<GameObject> previousNodeList;

        [HideInInspector]
        public bool gamePaused;

        public bool win;

        // Use this for initialization
        void Start()
        {
            win = false;
        }

        // Update is called once per frame
        void Update()
        {
            win = signalRequirements.All(i => previousNodeListTypes.Contains(i));
        }
    }
}
