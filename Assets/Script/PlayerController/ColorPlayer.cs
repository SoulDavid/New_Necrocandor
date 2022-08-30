using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class ColorPlayer : MonoBehaviour
{
    //[SerializeField] SpriteMeshInstance sInstance;
    [SerializeField] Color color;
    [SerializeField] int id;
    // Start is called before the first frame update
    void Start()
    {
        id = transform.root.GetComponent<State>().GetId();
        color = GameManager.Instance.GetColorPlayer(id);
    }

    SpriteMeshInstance m_SpriteMeshInstance = null;
    SpriteMeshInstance spriteMeshInstance
    {
        get
        {
            if (!m_SpriteMeshInstance)
            {
                m_SpriteMeshInstance = GetComponent<SpriteMeshInstance>();
            }
            return m_SpriteMeshInstance;
        }
    }

    MaterialPropertyBlock m_MaterialPropertyBlock;
    MaterialPropertyBlock materialPropertyBlock
    {
        get
        {
            if (m_MaterialPropertyBlock == null)
            {
                m_MaterialPropertyBlock = new MaterialPropertyBlock();
            }

            return m_MaterialPropertyBlock;
        }
    }

    void OnWillRenderObject()
    {
        spriteMeshInstance.cachedRenderer.GetPropertyBlock(materialPropertyBlock);

        materialPropertyBlock.SetColor("_Color", color);

        spriteMeshInstance.cachedRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
