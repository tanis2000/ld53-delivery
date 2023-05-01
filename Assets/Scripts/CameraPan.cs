using System;
using GameBase.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField] private float DragSpeed = 2;
        [SerializeField] private float TranslateSmooth = 1;
        [SerializeField] private float MinBound = 0;
        [SerializeField] private float MaxBound = 50;
        
        private Camera mainCamera;
        private bool isDragging;
        private Vector3 dragOrigin;
        private Vector3 originalPosition;
        private Vector3 desiredPosition;
        private bool canPan = true;

        private void OnEnable()
        {
            mainCamera = Camera.main;
            originalPosition = transform.position;
            desiredPosition = originalPosition;
        }

        private void Update()
        {
            DetectPan();
        }

        private void DetectPan()
        {
            if (!canPan)
            {
                return;
            }
            
            if (Mouse.current.press.wasPressedThisFrame)
            {
                    isDragging = true;
                    dragOrigin = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    return;
            }

            if (!Mouse.current.press.isPressed)
            {
                if (isDragging)
                {
                    isDragging = false;
                    desiredPosition = originalPosition;
                }
            }

            if (isDragging)
            {
                var touchPosition = Mouse.current.position.ReadValue();
                var worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

                var move = (dragOrigin - worldPosition).FlatY().FlatZ() * DragSpeed;
                transform.Translate(move);
                if (transform.position.x < MinBound)
                {
                    transform.position = transform.position.WhereX(MinBound);
                } else if (transform.position.x > MaxBound)
                {
                    transform.position = transform.position.WhereX(MaxBound);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * TranslateSmooth);
            }
        }

        public void DisablePan()
        {
            canPan = false;
        }

        public void EnablePan()
        {
            canPan = true;
        }
    }
}