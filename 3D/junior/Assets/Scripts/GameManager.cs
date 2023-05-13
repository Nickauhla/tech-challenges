using System.Collections.Generic;
using System.IO;
using Assets.Scripts.FixtureNS;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private Fixture m_defaultFixturePrefab;

        public static float CONVERSION_UNIT = 1000;

        public void Start()
        {
            DirectoryInfo streamingAssetsDir = new DirectoryInfo(Application.streamingAssetsPath);

            // We could add some more abstraction in order to import from several file formats
            FileInfo[] jsonFiles = streamingAssetsDir.GetFiles("*.json");
            ImportFromJson(jsonFiles);
        }

        private void ImportFromJson(FileInfo[] jsonFiles)
        {
            JsonImporter importer = new JsonImporter(m_defaultFixturePrefab);

            foreach (FileInfo jsonFile in jsonFiles)
            {
                FixtureSerializer deserializedFixture = importer.ParseJSON(jsonFile);
                if (deserializedFixture == null)
                {
                    Debug.LogError("Deserialized file is null!");
                    return;
                }
                importer.Import(deserializedFixture, jsonFile.Name);
                
            }
        }
    }
}