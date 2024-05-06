using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract class MaterialModifierBase : UIBehaviour, IMaterialModifier
{
    [NonSerialized]
    protected Material targetMaterial;

    [NonSerialized]
    private Graphic graphic;
    public Graphic graphicProp => graphic ? graphic : graphic = GetComponent<Graphic>();

    protected override void OnEnable()
    {
        base.OnEnable();
        if (graphicProp == null) return;
        graphicProp.SetMaterialDirty();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (targetMaterial != null)
        {
            DestroyImmediate(targetMaterial);
        }
        targetMaterial = null;

        if (graphicProp != null)
        {
            graphicProp.SetMaterialDirty();
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (!IsActive() || graphicProp == null)
        {
            
            return;
        }

        graphicProp.SetMaterialDirty();
    }
#endif

    protected override void OnDidApplyAnimationProperties()
    {
        base.OnDidApplyAnimationProperties();
        if (!IsActive() || graphicProp == null)
        {
            return;
        }

        graphicProp.SetMaterialDirty();
    }

    Material IMaterialModifier.GetModifiedMaterial(Material baseMaterial)
    {
        var isSameShader = baseMaterial.shader.name == graphicProp.material.shader.name;
        if (!IsActive() || graphic == null || !isSameShader)
        {
            return baseMaterial;
        }

        if (targetMaterial == null)
        {
            targetMaterial = new Material(baseMaterial);
            targetMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        targetMaterial.CopyPropertiesFromMaterial(baseMaterial);

        SetProperties();

        return targetMaterial;
    }

    abstract public void SetProperties();
}
