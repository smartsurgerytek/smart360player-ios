using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererDebuggger : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [ShowInInspector] private bool useWorldSpace { get => _lineRenderer.useWorldSpace; set {  _lineRenderer.useWorldSpace = value; } }
    private void Reset()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
}