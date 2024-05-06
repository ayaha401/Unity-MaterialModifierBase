using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
class Sample : MaterialModifierBase
{
    [SerializeField]
    private Vector2 leftPos = Vector2.left;

    [SerializeField]
    private Vector2 rightPos = Vector2.right;

    private readonly int rotPropertyId = Shader.PropertyToID("_Rot");

    private float rad;

    public override void SetProperties()
    {
        CalcRotation();
        targetMaterial.SetFloat(rotPropertyId, rad);    }

    private void CalcRotation()
    {
        Vector2 dt = rightPos - leftPos;
        rad = Mathf.Atan2(dt.y, dt.x);
    }
}
