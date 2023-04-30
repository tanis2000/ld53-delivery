using System;
using GameBase.Audio;
using UnityEngine;

namespace App
{
    public class Pooper : MonoBehaviour
    {
        [SerializeField] private float ChanceOverSecond = 0.1f;
        [SerializeField] private AudioCue FartCue;

        private float chanceCooldown;
        private ParticleSystem ps;

        private void OnEnable()
        {
            chanceCooldown = 1;
            ps = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            chanceCooldown -= Time.deltaTime;
            if (chanceCooldown <= 0)
            {
                if (Game.Instance.GetRnd().Float() < ChanceOverSecond)
                {
                    Poop();
                }
                chanceCooldown = 1;
            }
        }

        private void Poop()
        {
            ps.Stop();
            ps.Play();
            AudioSystem.Instance().Play(FartCue);
        }
    }
}