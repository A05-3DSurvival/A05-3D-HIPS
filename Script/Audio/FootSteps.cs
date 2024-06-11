using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private AudioManager audioManager; // AudioManager ������ ������ ����

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // AudioManager �ν��Ͻ� ����
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

                    // FootSteps�� �Ҹ� ��� �� ���� ����
                    float sfxVolume = audioManager.GetVolume("SFX");
                    audioSource.volume = sfxVolume;
                }
            }
        }
    }
}