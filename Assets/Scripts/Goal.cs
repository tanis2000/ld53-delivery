using System;
using System.Collections;
using System.Collections.Generic;
using GameBase.Audio;
using UnityEngine;

namespace App
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private RaceType Race;
        [SerializeField] private GameObject Balloon;
        [SerializeField] private Sprite OkSprite;
        [SerializeField] private Sprite QuestionSprite;
        [SerializeField] private Sprite WarningSprite;
        [SerializeField] private AudioCue HitCue;

        private Coroutine showingBalloon;
        private SpriteRenderer spriteRenderer;
        private float audioCooldown;

        private void OnEnable()
        {
            Balloon.SetActive(false);
            spriteRenderer = Balloon.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAudioCooldown();
        }

        public void Hit(Sling sling)
        {
            if (showingBalloon != null)
            {
                StopCoroutine(showingBalloon);
                Balloon.SetActive(false);
            }

            Sprite spriteType = QuestionSprite;
            if (sling.GetRace() == Race)
            {
                spriteType = OkSprite;
            }

            showingBalloon = StartCoroutine(ShowBalloon(spriteType));
            PlayCueWithCD(HitCue);
        }

        private IEnumerator ShowBalloon(Sprite spriteType)
        {
            spriteRenderer.sprite = spriteType;
            Balloon.SetActive(true);
            yield return new WaitForSeconds(2);
            Balloon.SetActive(false);
        }

        public RaceType GetRace()
        {
            return Race;
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