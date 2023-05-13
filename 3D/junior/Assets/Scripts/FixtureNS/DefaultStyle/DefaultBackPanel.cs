using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FixtureNS.DefaultStyle
{
    public class DefaultBackPanel : FixturePart, IBackPanel
    {
        DefaultBackPanelSerializer m_backpanelData;

        public float DepthUnityWorld { get => m_backpanelData.depth / GameManager.CONVERSION_UNIT; }

        public DefaultBackPanel(DefaultBackPanelSerializer backpanelData)
        {
            m_backpanelData = backpanelData;
        }

        public override Vector3 CalculatePositionRelativeToFixture(Fixture fixture)
        {
            float fixtureWidth = fixture.WidthUnityWorld;
            float fixtureHeight = fixture.HeightUnityWorld;
            float fixtureDepth = fixture.DepthUnityWorld;

            Vector3 position = new Vector3();
            position.y = fixtureHeight / 2;
            position.z = fixtureDepth / 2 - DepthUnityWorld;
            return position;
        }

        public override void CreateInstance(Fixture fixture)
        {
            GameObject backPanel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backPanel.name = "BackPanel";
            UnityEngine.Object.Destroy(backPanel.GetComponent<Collider>());
            Transform backPanelTrsf = backPanel.transform;
            backPanelTrsf.SetParent(fixture.transform, false);
            Vector3 currentSize = backPanelTrsf.localScale;
            backPanelTrsf.localScale = new Vector3(fixture.WidthUnityWorld, fixture.HeightUnityWorld, m_backpanelData.depth / GameManager.CONVERSION_UNIT);
            backPanelTrsf.localPosition = CalculatePositionRelativeToFixture(fixture);
        }
    }

    [Serializable]
    public class DefaultBackPanelSerializer
    {
        public float depth;
    }
}