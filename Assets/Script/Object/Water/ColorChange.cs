using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField]
    Color Change;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))
        {
            collision.GetComponent<MetaballParticleClass>().
                spRend.color = Change;
        }
    }

}
