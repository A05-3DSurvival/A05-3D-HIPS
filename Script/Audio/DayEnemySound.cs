using UnityEngine;

public class DayEnemySound : MonoBehaviour
{
    public AudioClip dayEnemySoundEffect;
    public float detectionRadius;
    private AudioSource audioSource;
    private Player player;
    private AudioManager audioManager;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = dayEnemySoundEffect;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioManager = AudioManager.Instance;

        player = CharacterManager.Instance.Player;

        if (player == null)
        {
            Debug.LogWarning("Player object not found!");
        }

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = detectionRadius;
    }

    private void Update()
    {
        float sfxVolume = audioManager.GetVolume("SFX");
        audioSource.volume = sfxVolume;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            PlaySound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            StopSound();
        }
    }

    private void PlaySound()
    {
        int randomIndex = Random.Range(0, 10);
        if (audioSource != null && !audioSource.isPlaying && randomIndex > 4)
        {
            audioSource.Play();
        }
    }

    private void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
