﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameManagement
{
    public class GameManager : MonoBehaviour
    {
        public bool gamePaused;

        public GameObject[] signalFlowHolders;

        //Here will be the bools that can be checked according to the level type
        #region Enable Bools
        public bool levelOne;
        public bool levelTwo;
        public bool levelThree;
        public bool levelFour;
        public bool levelFive;
        public bool levelSix;
        public bool levelSeven;
        #endregion

        //Here the bools will coicide with the enabled bools
        #region Bools

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}