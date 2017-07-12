using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Dial
{
    public class DisplayText : MonoBehaviour
    {
        public DialRotation dialRotation;

        public double percentageValue;

        public Text value;

        // Use this for initialization
        void Start()
        {
            dialRotation = gameObject.GetComponent<DialRotation>();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(dialRotation.dialValue);

            percentageValue = System.Math.Round(dialRotation.dialValue * 100f, 1);       

            value.text = percentageValue + "%";
        }
    }
}
