using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FixtureNS
{
    public abstract class FixturePart
    {
        protected GameObject m_representation;
        public abstract void CreateInstance(Fixture fixture);

        public abstract Vector3 CalculatePositionRelativeToFixture(Fixture fixture);
    }
}