using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Transform> OnTag;

    [SerializeField]
    private SkinnedMeshRenderer skin;
    private Material defaultMaterial;

    private void Start() => defaultMaterial = skin.material;

    private void Update() => gameObject.tag = transform.parent.tag;

    public void Tag() => StartCoroutine(Handling());

    private IEnumerator Handling()
    {
        skin.material = Settings.FlickeringMaterial;
        gameObject.layer = LayerMask.NameToLayer("Ghost");

        yield return new WaitForSeconds(Settings.FlickeringTime);

        skin.material = defaultMaterial;
        gameObject.layer = LayerMask.NameToLayer("Bear");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tagger"))
            OnTag?.Invoke(collision.transform.parent);
    }
}
