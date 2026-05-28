using UnityEngine;

namespace Trellcko.Gameplay.House
{
    public class GlitchWall : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _renderers;
        
        public void SetRendererLayerMask(uint mask)
        {
            foreach (MeshRenderer renderer in _renderers)
            {
                renderer.renderingLayerMask = mask;
            }
        }
    }
}