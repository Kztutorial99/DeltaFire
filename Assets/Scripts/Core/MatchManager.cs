using UnityEngine;
using DeltaFire.Player;

namespace DeltaFire.Core
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private int targetBots = 12;
        [SerializeField] private GameObject botPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private void Start()
        {
            SpawnBots();
        }

        private void SpawnBots()
        {
            if (!botPrefab || spawnPoints == null || spawnPoints.Length == 0) return;
            for (int i = 0; i < targetBots; i++)
            {
                Transform spawn = spawnPoints[i % spawnPoints.Length];
                Instantiate(botPrefab, spawn.position, spawn.rotation);
            }
        }

        private void Update()
        {
            int alive = 0;
            foreach (Health health in FindObjectsOfType<Health>())
                if (!health.IsDead) alive++;

            if (alive <= 1) Debug.Log("DeltaFire: Match complete - last survivor wins.");
        }
    }
}
