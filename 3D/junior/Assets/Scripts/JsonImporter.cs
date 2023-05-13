using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.FixtureNS;
using UnityEngine;

namespace Assets.Scripts
{
    public class JsonImporter
    {
        private Fixture m_prefab;

        public JsonImporter(Fixture prefab)
        {
            m_prefab = prefab;
        }

        public FixtureSerializer ParseJSON(FileInfo path)
        {
            try
            {
                string fileContent = File.ReadAllText(path.FullName);
                FixtureSerializer deserializedFixture = JsonUtility.FromJson<FixtureSerializer>(fileContent);
                return deserializedFixture;
            }
            catch (Exception e)
            {
                // In order to be really clean, we should discriminate each possible exception
                // regarding its type, but it appears a bit overkill to me here.
                Debug.LogError($"Error loading Json File: {e.ToString()}");
                return null;
            }
        }

        public void Import(FixtureSerializer deserializedFixture, string name)
        {
            Fixture fixture = GameObject.Instantiate<Fixture>(m_prefab);
            fixture.gameObject.name = name;
            fixture.gameObject.tag = "Furniture";
            fixture.Initialize(deserializedFixture);
        }
    }
}