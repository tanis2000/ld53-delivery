using System;
using System.Collections;
using System.Collections.Generic;
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

        private Coroutine showingBalloon;
        private SpriteRenderer spriteRenderer;
        
        private void OnEnable()
        {
            Balloon.SetActive(false);
            spriteRenderer = Balloon.GetComponent<SpriteRenderer>();
        }

        public void Hit(Sling sling)
        {
            if (showingBalloon != null)
            {
                StopCoroutine(showingBalloon);
                Balloon.SetActive(false);
            }

            Sprite spriteType = WarningSprite;
            if (sling.GetRace() == Race)
            {
                spriteType = OkSprite;
            }

            showingBalloon = StartCoroutine(ShowBalloon(spriteType));
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
    }
}