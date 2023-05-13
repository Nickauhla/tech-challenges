using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FixtureNS;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultShelve : FixturePart, IShelve
    {
        private DefaultShelveSerializer m_shelveData;

        public float HeightUnityWorld { get => m_shelveData.height / GameManager.CONVERSION_UNIT; }
        public float YUnityWorld { get => m_shelveData.y / GameManager.CONVERSION_UNIT; }

        public DefaultShelve(DefaultShelveSerializer deserializedShelve)
        {
            m_shelveData = deserializedShelve;
        }

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.y = YUnityWorld - HeightUnityWorld / 2;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject shelve = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(shelve.GetComponent<Collider>());
            shelve.name = "Header";
            Transform shelveTrsf = shelve.transform;
            shelveTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = shelveTrsf.localScale;
            shelveTrsf.localScale = new Vector3(fixture.WidthUnityWorld, HeightUnityWorld, fixture.DepthUnityWorld);
            shelveTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultShelveSerializer
    {
        public float height;
        public int y;
    }
}