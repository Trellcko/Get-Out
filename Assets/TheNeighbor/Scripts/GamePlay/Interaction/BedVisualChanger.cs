using System;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class BedVisualChanger : MonoBehaviour
    {
        [SerializeField] private Material _badBed;
        [SerializeField] private Material _goodBed;

        [SerializeField] private MeshRenderer _meshRenderer;

        [SerializeField] private QuestInteractable _interactable;

        private void OnEnable()
        {
            _interactable.InteractionStarted += OnInteractionStarted;
        }

        private void OnDisable()
        {
            _interactable.InteractionStarted -= OnInteractionStarted;
        }

        private void OnInteractionStarted()
        {
            Material[] materials = _meshRenderer.materials;
            materials[2] = _goodBed;
            _meshRenderer.sharedMaterials= materials;
        }


    }
}