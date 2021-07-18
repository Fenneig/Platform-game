using UnityEngine;
using UnityEngine.Audio;

public class CaveEchoComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private AudioMixer _roomEffectScale;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_tag)) _roomEffectScale.SetFloat("CaveEcho", 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_tag)) _roomEffectScale.SetFloat("CaveEcho", -10000f);
    }

}
