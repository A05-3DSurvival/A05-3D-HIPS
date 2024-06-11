using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private AudioManager audioManager; // AudioManager 참조를 저장할 변수

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // AudioManager 인스턴스 참조
        audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if (_rigidbody.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footStepTime > footstepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);

                    // FootSteps의 소리 재생 시 볼륨 조절
                    float sfxVolume = audioManager.GetVolume("SFX");
                    audioSource.volume = sfxVolume;
                }
            }
        }
    }
}
