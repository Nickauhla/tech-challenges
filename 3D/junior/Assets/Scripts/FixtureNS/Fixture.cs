using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.FixtureNS.DefaultStyle;

namespace Assets.Scripts.FixtureNS
{
    public class Fixture : MonoBehaviour, IFurniture
    {
        [SerializeField] private Material m_defaultMaterial;
        [SerializeField] private Material m_selectedMaterial;

        public bool IsInEditMode { get; set; } = false;

        public BoxCollider FurnitureBoudingBoxCollider { get; private set; }
        public Vector3 Position => transform.position;
        private float m_width;
        public float WidthUnityWorld { get => m_width / GameManager.CONVERSION_UNIT; }

        private float m_height;
        public float HeightUnityWorld { get => m_height / GameManager.CONVERSION_UNIT; }

        private float m_depth;
        public float DepthUnityWorld { get => m_depth / GameManager.CONVERSION_UNIT; }

        private List<FixturePart> m_parts = new List<FixturePart>();
        private List<Renderer> m_renderers;
        private Vector3 m_oldPosition;

        public virtual void Initialize(FixtureSerializer data)
        {
            Deserialize(data);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            m_parts.ForEach(part => part.CreateInstance(this));
            m_renderers = GetComponentsInChildren<Renderer>().ToList();
            m_renderers.ForEach(renderer => renderer.material = m_defaultMaterial);

            CalculatesAndSaveBounds(boxCollider);

            StartCoroutine(AvoidOverlap());
        }

        private void CalculatesAndSaveBounds(BoxCollider boxCollider)
        {
            // We should test if renderers is null before doing that, but in this specific case
            // it wouldn't make any sense to have a void array, as we should have at least 1 renderer in the fixture.
            // I have to pre-initialize bounds because Unity does not allow me to have initialized or null bounds.
            Bounds bounds = m_renderers[0].bounds;

            // However, event though it is out of the scope of the exercice, we could imagine a single part fixture
            // So we test it 
            if (m_renderers.Count > 1)
            {
                for (int i = 1; i < m_renderers.Count; i++)
                {
                    Renderer rend = m_renderers[i];
                    bounds.Encapsulate(rend.bounds);
                }
            }
            boxCollider.size = bounds.size;
            FurnitureBoudingBoxCollider = boxCollider;
        }

        public IEnumerator AvoidOverlap()
        {
            GameObject[] furnituresGO = GameObject.FindGameObjectsWithTag("Furniture");
            List<IFurniture> furnitures = furnituresGO.Select(furniture => furniture.GetComponent<IFurniture>()).ToList();
            bool isOverlapping = true;
            while (isOverlapping)
            {
                isOverlapping = false;
                foreach (IFurniture furniture in furnitures)
                {
                    System.Random rand = new System.Random(Time.frameCount);
                    if (Equals(furniture)) continue; // it always intersects with himself. We want to avoid that.
                    if (FurnitureBoudingBoxCollider.bounds.Intersects(furniture.FurnitureBoudingBoxCollider.bounds))
                    {
                        TranslateFurnitureOnPlan(furniture, rand);
                        isOverlapping = true;
                    }
                }
                yield return null;
            }
            StartCoroutine(AvoidOverlap());
            yield break;
        }

        private void TranslateFurnitureOnPlan(IFurniture furniture, System.Random rand)
        {
            Vector3 direction = furniture.Position - transform.position;

            // We randomize a direction if they are exactly at the same center, to initiate a mouvement
            if (direction == Vector3.zero) direction = new Vector3((float)rand.NextDouble() * 2, 0, (float)rand.NextDouble() * 2);
            direction.y = 0;
            transform.Translate(-direction * Time.deltaTime, Space.World);
        }

        public void OnSelection(bool editMode)
        {
            IsInEditMode = editMode;
            if (IsInEditMode)
            {
                m_renderers.ForEach(renderer => renderer.material = m_selectedMaterial);
                m_oldPosition = transform.position;
                StartCoroutine(StickToMousePosition());
            }
            else
            {
                m_renderers.ForEach(renderer => renderer.material = m_defaultMaterial);
                StartCoroutine(AvoidOverlap());
            }
        }

        private IEnumerator StickToMousePosition()
        {
            while (IsInEditMode)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hitInfo))
                {
                    transform.position = hitInfo.point;
                    FurnitureBoudingBoxCollider.enabled = false;
                }
                yield return null;
            }
            FurnitureBoudingBoxCollider.enabled = true;
            yield break;
        }

        public void CancelEditMode()
        {
            transform.position = m_oldPosition;
            OnSelection(false);
        }

        protected virtual void Deserialize(FixtureSerializer data)
        {
            m_width = data.width;
            m_height = data.height;
            m_depth = data.depth;

            if (data.backPanel != null)
                m_parts.Add(new DefaultBackPanel(data.backPanel));
            if (data.header != null)
                m_parts.Add(new DefaultHeader(data.header));
            if (data.footer != null)
                m_parts.Add(new DefaultFooter(data.footer));
            if (data.leftSide != null)
                m_parts.Add(new DefaultLeftSide(data.leftSide));
            if (data.rightSide != null)
                m_parts.Add(new DefaultRightSide(data.rightSide));
            if (data.shelves != null && data.shelves.Count > 0)
                m_parts.AddRange(data.shelves.Select(deserializedShelve => new DefaultShelve(deserializedShelve)));
        }

    }

    [Serializable]
    public class FixtureSerializer
    {
        public float width;
        public float height;
        public float depth;
        public DefaultBackPanelSerializer backPanel;
        public DefaultHeaderSerializer header;
        public DefaultFooterSerializer footer;
        public DefaultLeftSideSerializer leftSide;
        public DefaultRightSideSerializer rightSide;
        public List<DefaultShelveSerializer> shelves;
    }
}