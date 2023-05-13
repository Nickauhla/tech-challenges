using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FixtureNS;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultLeftSide : FixturePart, ILeftSide
    {
        private DefaultLeftSideSerializer m_leftSideData;

        public DefaultLeftSide(DefaultLeftSideSerializer leftSide)
        {
            this.m_leftSideData = leftSide;
        }

        public float WidthUnityWorld {get => m_leftSideData.width/GameManager.CONVERSION_UNIT; }

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.x = -fixtureWidth/2;
            position.y = fixtureHeight/2;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject leftSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Component.Destroy(leftSide.GetComponent<Collider>());
            leftSide.name = "Left Side";
            Transform leftSideTrsf = leftSide.transform;
            leftSideTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = leftSideTrsf.localScale;
            leftSideTrsf.localScale = new Vector3(WidthUnityWorld, fixture.HeightUnityWorld, fixture.DepthUnityWorld);
            leftSideTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultLeftSideSerializer
    {
        public float width;
    }
}
