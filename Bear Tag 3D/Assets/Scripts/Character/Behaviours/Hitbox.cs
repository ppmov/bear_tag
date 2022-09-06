using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent onDealing;
    public UnityEvent onTaking;

    [SerializeField]
    private SkinnedMeshRenderer skin;
    private Material defaultMaterial;

    private void Start() => defaultMaterial = skin.material;

    private void Tag() => StartCoroutine(Handling());

    private IEnumerator Handling()
    {
        skin.material = Settings.FlickeringMaterial;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        onTaking?.Invoke();

        yield return new WaitForSeconds(Settings.FlickeringTime);

        skin.material = defaultMaterial;
        gameObject.layer = LayerMask.NameToLayer("Bear");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Tagger"))
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bear"))
            {
                collision.gameObject.GetComponent<Hitbox>().Tag();
                onDealing?.Invoke();
            }
    }
}
