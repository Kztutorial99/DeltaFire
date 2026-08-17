using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using DeltaFire.Player;
using DeltaFire.Combat;
using DeltaFire.AI;
using DeltaFire.Core;
using DeltaFire.World;

namespace DeltaFire.Editor
{
    public static class DeltaFirePrototypeGenerator
    {
        private const string ScenePath = "Assets/Scenes/DeltaFirePrototype.unity";
        private const string BotPrefabPath = "Assets/Prefabs/Bot.prefab";

        [MenuItem("Tools/DeltaFire/Create Prototype Scene")]
        public static void Create()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory("Assets/Prefabs");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateWorld();
            GameObject player = CreatePlayer();
            GameObject botPrefab = CreateBotPrefab();
            Transform[] spawnPoints = CreateSpawnPoints();
            CreateMatchManager(botPrefab, spawnPoints);
            CreateSafeZone();

            Selection.activeGameObject = player;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("DeltaFire prototype created: " + ScenePath);
        }

        private static void CreateWorld()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Battlefield";
            ground.transform.localScale = Vector3.one * 25f;

            for (int i = 0; i < 18; i++)
            {
                GameObject cover = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cover.name = "Cover_" + i;
                cover.transform.position = new Vector3((i % 6 - 2.5f) * 18f, 1f, (i / 6 - 1f) * 22f);
                cover.transform.localScale = new Vector3(5f, 2f, 5f);
            }

            GameObject light = new GameObject("Directional Light");
            Light sun = light.AddComponent<Light>();
            sun.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static GameObject CreatePlayer()
        {
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            player.transform.position = Vector3.up * 2f;
            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = .35f;
            player.AddComponent<Health>();
            player.AddComponent<FpsController>();

            GameObject cameraObject = new GameObject("PlayerCamera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform);
            cameraObject.transform.localPosition = new Vector3(0f, .65f, 0f);
            cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();

            GameObject weapon = GameObject.CreatePrimitive(PrimitiveType.Cube);
            weapon.name = "Rifle";
            weapon.transform.SetParent(cameraObject.transform);
            weapon.transform.localPosition = new Vector3(.28f, -.22f, .55f);
            weapon.transform.localScale = new Vector3(.12f, .12f, .65f);
            weapon.AddComponent<Weapon>();

            return player;
        }

        private static GameObject CreateBotPrefab()
        {
            GameObject bot = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            bot.name = "Bot";
            bot.transform.position = new Vector3(0f, 1f, 25f);
            bot.AddComponent<Health>();
            bot.AddComponent<BotController>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(bot, BotPrefabPath);
            Object.DestroyImmediate(bot);
            return prefab;
        }

        private static void CreateMatchManager(GameObject botPrefab, Transform[] spawnPoints)
        {
            GameObject manager = new GameObject("MatchManager");
            MatchManager match = manager.AddComponent<MatchManager>();
            SerializedObject serialized = new SerializedObject(match);
            serialized.FindProperty("botPrefab").objectReferenceValue = botPrefab;
            serialized.FindProperty("targetBots").intValue = 12;
            SerializedProperty array = serialized.FindProperty("spawnPoints");
            array.arraySize = spawnPoints.Length;
            for (int i = 0; i < spawnPoints.Length; i++) array.GetArrayElementAtIndex(i).objectReferenceValue = spawnPoints[i];
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateSafeZone()
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            zone.name = "SafeZone";
            zone.transform.position = new Vector3(0f, .03f, 0f);
            zone.GetComponent<Collider>().enabled = false;
            zone.AddComponent<SafeZone>();
        }

        private static Transform[] CreateSpawnPoints()
        {
            Transform[] points = new Transform[8];
            for (int i = 0; i < points.Length; i++)
            {
                GameObject spawn = new GameObject("Spawn_" + i);
                float angle = i * Mathf.PI * 2f / points.Length;
                spawn.transform.position = new Vector3(Mathf.Cos(angle) * 55f, 1f, Mathf.Sin(angle) * 55f);
                points[i] = spawn.transform;
            }
            return points;
        }
    }
}
