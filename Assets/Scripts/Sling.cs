using System;
using System.Collections.Generic;
using GameBase.Animations;
using GameBase.Animations.Actors;
using GameBase.Audio;
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
        [SerializeField] private RaceType Race;
        [SerializeField] private AudioCue SlingCue;
        [SerializeField] private AudioCue HitCue;
        [SerializeField] private GameObject TrajectoryPointPrefeb;
        [SerializeField] private float Power = 10;


        private bool wasDragging;
        private bool isDragging;
        private Camera mainCamera;
        private SpringJoint2D spring;
        private Rigidbody2D body;
        private bool canDrag = true;
        private LineRenderer line;
        private Stretcher stretcher;
        private SubmitScore submitScore;
        private bool isSelected;
        private BoxCollider2D coll;
        private LevelSystem levelSystem;
        private bool goalReached;
        private float audioCooldown;
        private int numOfTrajectoryPoints = 30;
        private List<GameObject> trajectoryPoints = new List<GameObject>();
        private CameraPan cameraPan;
        private CameraFollow cameraFollow;


        private void OnEnable()
        {
            mainCamera = Camera.main;
            body = GetComponent<Rigidbody2D>();
            spring = GetComponent<SpringJoint2D>();
            line = GetComponent<LineRenderer>();
            stretcher = GetComponentInChildren<Stretcher>();
            submitScore = FindObjectOfType<SubmitScore>();
            coll = GetComponent<BoxCollider2D>();
            cameraPan = FindObjectOfType<CameraPan>();
            cameraFollow = FindObjectOfType<CameraFollow>();

            for (var i = 0; i < numOfTrajectoryPoints; i++)
            {
                GameObject dot = Instantiate(TrajectoryPointPrefeb, transform.parent);
                dot.GetComponent<Renderer>().enabled = false;
                trajectoryPoints.Insert(i, dot);
            }

        }

        private void DetectTouch()
        {
            if (Mouse.current.press.isPressed)
            {
                var touchPosition = Mouse.current.position.ReadValue();
                var worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
                var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if (hit.rigidbody == body)
                {
                    isDragging = true;
                    cameraPan.DisablePan();
                    coll.enabled = false;
                }
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
                
                Vector3 vel = GetForceFrom(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Pivot.transform.position);
                float angle = Mathf.Atan2(vel.y,vel.x)* Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0,0,angle);
                SetTrajectoryPoints(transform.position, vel/GetComponent<Rigidbody2D>().mass);

            }
        }

        private void LaunchShot()
        {
            body.isKinematic = false;
            canDrag = false;
            isDragging = false;
            spring.enabled = false;
            body.AddForce(GetForceFrom(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Pivot.transform.position),ForceMode2D.Impulse);
            PlayCueWithCD(SlingCue);
            cameraFollow.Enable();
            cameraFollow.Target = transform;
        }

        private void DetachFromPivot()
        {
            if (transform.position.x > Pivot.transform.position.x && !body.isKinematic && spring.enabled)
            {
                spring.enabled = false;
                PlayCueWithCD(SlingCue);
            }
        }

        private void EnableCollisions()
        {
            if (transform.position.x > Pivot.transform.position.x && !isDragging)
            {
                coll.enabled = true;
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
            if (!isSelected)
            {
                return;
            }
            DetectTouch();
            DetachFromPivot();
            DrawLine();
            DetectStop();
            UpdateAudioCooldown();
            EnableCollisions();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            stretcher.Execute();
            PlayCueWithCD(HitCue);
            RemoveTrajectoryPoints();

            var goal = other.gameObject.GetComponent<Goal>();
            if (goal != null && !goalReached && isSelected)
            {
                //body.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);
                body.velocity = Vector2.zero;
                goal.Hit(this);
                goalReached = true;
                if (goal.GetRace() == Race)
                {
                    submitScore.IncrementScore(3);
                }
                else
                {
                    submitScore.IncrementScore();
                }
                cameraFollow.Disable();
                cameraPan.EnablePan();
                levelSystem.Next(true);
            }
        }

        public RaceType GetRace()
        {
            return Race;
        }

        public void StartGame(LevelSystem ls)
        {
            levelSystem = ls;
            isSelected = true;
            transform.position = PivotMiddle.transform.position;
            coll.enabled = true;
        }

        public void StopGame()
        {
            isSelected = false;
            coll.enabled = false;
        }

        private void DetectStop()
        {
            if (isSelected && !spring.enabled && body.velocity.magnitude <= 0.01f)
            {
                Debug.Log("Stopped");
                isSelected = false;
                cameraFollow.Disable();
                cameraPan.EnablePan();
                if (!goalReached)
                {
                    levelSystem.Next(false);
                }
            }
        }

        public void OutOfBoundsStop()
        {
            body.velocity = Vector2.zero;
        }

        private void PlayCueWithCD(AudioCue cue)
        {
            if (audioCooldown > 0)
            {
                return;
            }
            audioCooldown = 0.5f;
            AudioSystem.Instance().Play(cue);
        }

        private void UpdateAudioCooldown()
        {
            audioCooldown -= Time.deltaTime;
        }
        
        private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
        {
            return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*Power;
        }
        
        private void SetTrajectoryPoints(Vector3 pStartPosition , Vector3 pVelocity )
        {
            float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
            float angle = Mathf.Rad2Deg*(Mathf.Atan2(pVelocity.y , pVelocity.x));
            float fTime = 0;
        
            fTime += 0.1f;
            for (int i = 0 ; i < trajectoryPoints.Count ; i++)
            {
                float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
                float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
                Vector3 pos = new Vector3(pStartPosition.x + dx , pStartPosition.y + dy ,2);
                trajectoryPoints[i].transform.position = pos;
                trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                trajectoryPoints[i].transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude)*fTime,pVelocity.x)*Mathf.Rad2Deg);
                fTime += 0.1f;
            }
        }

        public void RemoveTrajectoryPoints()
        {
            foreach (var trajectoryPoint in trajectoryPoints)
            {
                Destroy(trajectoryPoint.gameObject);
            }
            trajectoryPoints.Clear();
        }
    }
}