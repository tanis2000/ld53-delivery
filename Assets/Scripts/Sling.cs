using System;
using GameBase.Animations.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App
{
    public class Sling : MonoBehaviour
    {
        [SerializeField] private GameObject Pivot;
        [SerializeField] private GameObject PivotFront;
        [SerializeField] private GameObject PivotBack;
        [SerializeField] private GameObject PivotMiddle;

        private bool wasDragging;
        private bool isDragging;
        private Camera mainCamera;
        private SpringJoint2D spring;
        private Rigidbody2D body;
        private bool canDrag = true;
        private LineRenderer line;
        private Stretcher stretcher;

        private void OnEnable()
        {
            mainCamera = Camera.main;
            body = GetComponent<Rigidbody2D>();
            spring = GetComponent<SpringJoint2D>();
            line = GetComponent<LineRenderer>();
            stretcher = GetComponentInChildren<Stretcher>();
        }

        private void DetectTouch()
        {
            if (Mouse.current.press.isPressed)
            {
                isDragging = true;
            }

            if (!Mouse.current.press.isPressed)
            {
                if (isDragging)
                {
                    LaunchShot();
                    return;
                }
            }

            if (canDrag && isDragging)
            {
                isDragging = true;
                body.isKinematic = true;

                var touchPosition = Mouse.current.position.ReadValue();
                var worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
                worldPosition.z = transform.position.z;

                transform.position = worldPosition;
            }
        }

        private void LaunchShot()
        {
            body.isKinematic = false;
            canDrag = false;
            isDragging = false;
        }

        private void DetachFromPivot()
        {
            if (transform.position.x > Pivot.transform.position.x && !body.isKinematic)
            {
                spring.enabled = false;
            }
        }

        private void DrawLine()
        {
            if (!spring.enabled)
            {
                line.SetPosition(1, PivotMiddle.transform.position);
            }
            else
            {
                line.SetPosition(1, transform.position);
            }
            line.SetPosition(0, PivotFront.transform.position);
            line.SetPosition(2, PivotBack.transform.position);
        }

        private void Update()
        {
            DetectTouch();
            DetachFromPivot();
            DrawLine();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            stretcher.Execute();
        }
    }
}