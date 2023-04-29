using System;
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

        private void OnEnable()
        {
            mainCamera = Camera.main;
            body = GetComponent<Rigidbody2D>();
            spring = GetComponent<SpringJoint2D>();
            line = GetComponent<LineRenderer>();
            stretcher = GetComponentInChildren<Stretcher>();
            submitScore = FindObjectOfType<SubmitScore>();
            coll = GetComponent<BoxCollider2D>();
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
            if (transform.position.x > Pivot.transform.position.x && !body.isKinematic && spring.enabled)
            {
                spring.enabled = false;
                PlayCueWithCD(SlingCue);
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
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            stretcher.Execute();
            PlayCueWithCD(HitCue);

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
    }
}