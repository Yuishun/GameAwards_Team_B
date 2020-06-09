using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class RainBleedEffect : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;

    void Awake()
    {
        m_spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        m_spriteRenderer.material.SetFloat("_StartTime", Time.time);
        float animationTime = m_spriteRenderer.material.GetFloat("_AnimationTime");
        float destroyTime = animationTime;
        destroyTime -= m_spriteRenderer.material.GetFloat("_StartWidth") * animationTime;
        destroyTime += m_spriteRenderer.material.GetFloat("_Width") * animationTime;
        Destroy(transform.gameObject, destroyTime);
    }
}
