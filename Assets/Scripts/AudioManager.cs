using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource effectsSource;

	// Needed to alter pitch randomly
	private float lowPitchRange = 0.95f;
	private float highPitchRange = 1.05f;

	// Singleton instance.
	public static AudioManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void Play(AudioClip clip)
	{
		effectsSource.clip = clip;
		effectsSource.PlayOneShot(effectsSource.clip);
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}

	// Play a random clip from an array of AudioClips, slight pitch variance to reduce monotony
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		effectsSource.pitch = randomPitch;
		effectsSource.clip = clips[randomIndex];
		effectsSource.PlayOneShot(effectsSource.clip);
	}

	//  Here's how to use it inside a gameobject:
	//	public class YourGameObject : MonoBehaviour
	//	{
	//    public AudioClip soundfx;
	//    public AudioClip music;

	//	  void Start()
	//    {
	//      AudioManager.Instance.PlayMusic(music);
	//      AudioManager.Instance.Play(soundfx);
	//    }
	//  }
}
