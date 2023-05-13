using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultHeader : FixturePart, IHeader
    {
        private DefaultHeaderSerializer m_headerData;
        private float HeightUnityWorld { get => m_headerData.height / GameManager.CONVERSION_UNIT; }

        public DefaultHeader(DefaultHeaderSerializer data)
        {
            m_headerData = data;
        }

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.y = fixtureHeight - HeightUnityWorld / 2;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject header = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(header.GetComponent<Collider>());
            header.name = "Header";
            Transform headerTrsf = header.transform;
            headerTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = headerTrsf.localScale;
            headerTrsf.localScale = new Vector3(fixture.WidthUnityWorld, HeightUnityWorld, fixture.DepthUnityWorld);
            headerTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultHeaderSerializer
    {
        public float height;
    }
}