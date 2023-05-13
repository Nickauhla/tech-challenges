using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultFooter : FixturePart, IFooter
    {
        private DefaultFooterSerializer m_footerData;

        private float HeightUnityWorld { get => m_footerData.height / GameManager.CONVERSION_UNIT; }

        public DefaultFooter(DefaultFooterSerializer footer)
        {
            m_footerData = footer;
        }

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.y = HeightUnityWorld / 2;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject footer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(footer.GetComponent<Collider>());
            footer.name = "Footer";
            Transform footerTrsf = footer.transform;
            footerTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = footerTrsf.localScale;
            footerTrsf.localScale = new Vector3(fixture.WidthUnityWorld, HeightUnityWorld, fixture.DepthUnityWorld);
            footerTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultFooterSerializer
    {
        public float height;
    }
}