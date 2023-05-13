namespace Assets.Scripts.FixtureNS
{
    internal interface IFurniture
    {
        public UnityEngine.BoxCollider FurnitureBoudingBoxCollider { get; }

        public UnityEngine.Vector3 Position { get; }
    }
}