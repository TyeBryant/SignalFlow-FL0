using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Dial
{
    public class DialRotation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
    {
        #region Important Variables
        //To get the value of the dial -- FOR SOUND PURPOSES -- 
        public float dialValue;

        //The number of loops that the dial can do.
        public int loops = 1;
        #endregion

        #region Values
        private float currentAngle;
        private float previousValue = 0;
        private float currentLoops = 0;
        public float maxValue;
        #endregion

        #region Initial Saved Data
        private Quaternion initialRotation;
        private Vector2 currentVector;
        private float initialAngle;
        #endregion

        #region Rotation Adjustment
        public bool clampOutput01 = false;

        //Only allows rotation with pointer over the control
        public bool canRotate = false;
        #endregion

        public void OnPointerDown(PointerEventData eventData)
        {
            canRotate = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            canRotate = false;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            canRotate = true;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            canRotate = false;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            SetInitPointerData(eventData);
        }
        void SetInitPointerData(PointerEventData eventData)
        {
            initialRotation = transform.rotation;
            currentVector = eventData.position - (Vector2)transform.position;
            initialAngle = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canRotate)
            {
                SetInitPointerData(eventData);
                return;
            }

            currentVector = eventData.position - (Vector2)transform.position;
            currentAngle = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg;

            Quaternion addRotation = Quaternion.AngleAxis(currentAngle - initialAngle, this.transform.forward);
            addRotation.eulerAngles = new Vector3(0, 0, (addRotation.eulerAngles.z * 20));

            Quaternion finalRotation = initialRotation * addRotation;

            //Times this value by 100 to get a percentage hehe
            dialValue = 1 - (finalRotation.eulerAngles.z / 360f);

            //Prevent overrotation
            if (Mathf.Abs(dialValue - previousValue) > 0.5f)
            {
                if (dialValue < 0.5f && loops > 1 && currentLoops < loops - 1)
                {
                    currentLoops++;
                }
                else if (dialValue > 0.5f && currentLoops >= 1)
                {
                    currentLoops--;
                }
                else
                {
                    if (dialValue > 0.5f && currentLoops == 0)
                    {
                        dialValue = 0;
                        transform.localEulerAngles = Vector3.zero;
                        SetInitPointerData(eventData);
                        return;
                    }
                    else if (dialValue < 0.5f && currentLoops == loops - 1)
                    {
                        dialValue = 1;
                        transform.localEulerAngles = Vector3.zero;
                        SetInitPointerData(eventData);
                        return;
                    }
                }
            }

            //Check for max value
            if (maxValue > 0)
            {
                if (dialValue + currentLoops > maxValue)
                {
                    dialValue = maxValue;
                    float maxAngle = 360f * maxValue;
                    transform.localEulerAngles = new Vector3(0, 0, maxAngle);
                    SetInitPointerData(eventData);
                    return;
                }
            }

            transform.rotation = finalRotation;
            previousValue = dialValue;
        }
    }
}
