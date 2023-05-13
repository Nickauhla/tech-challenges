using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FixtureNS;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultRightSide : FixturePart, IRightSide
    {
        private DefaultRightSideSerializer m_rightSideData;

        public DefaultRightSide(DefaultRightSideSerializer rightSide)
        {
            this.m_rightSideData = rightSide;
        }

        public float WidthUnityWorld { get => m_rightSideData.width/GameManager.CONVERSION_UNIT;}

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.x = fixtureWidth/2;
            position.y = fixtureHeight/2;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject rightSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Component.Destroy(rightSide.GetComponent<Collider>());
            rightSide.name = "Right Side";
            Transform rigthSideTrsf = rightSide.transform;
            rigthSideTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = rigthSideTrsf.localScale;
            rigthSideTrsf.localScale = new Vector3(WidthUnityWorld, fixture.HeightUnityWorld, fixture.DepthUnityWorld);
            rigthSideTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultRightSideSerializer
    {
        public float width;
    }
}