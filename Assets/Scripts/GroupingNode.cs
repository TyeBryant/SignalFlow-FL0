using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeFunctionality
{
    public class GroupingNode : Node
    {
        public Camera worldCam;

        private Vector3 originalWorldCamPos;
        private CircleCollider2D cC2D;

        public bool groupSelected = false;
        public float cC2DRadius;

        // Use this for initialization
        new void Start()
        {
            cC2D = GetComponent<CircleCollider2D>();
            cC2DRadius = cC2D.bounds.extents.x;
            originalWorldCamPos = worldCam.transform.position;
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            if (!groupSelected)
                base.Update();

            if (Input.GetMouseButtonDown(1))
            {
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) < cC2DRadius)
                {
                    groupSelected = true;
                    worldCam.transform.position = this.transform.position - transform.forward * 10;
                    worldCam.orthographicSize = 0.65f;
                }
                else
                {
                    groupSelected = false;
                    worldCam.transform.position = originalWorldCamPos;
                    worldCam.orthographicSize = 5f;
                    cC2D.enabled = true;
                }
            }
            else if (Input.GetMouseButtonDown(0))
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) > cC2DRadius && groupSelected)
                    cC2D.enabled = !cC2D.enabled;

        }
    }
}
