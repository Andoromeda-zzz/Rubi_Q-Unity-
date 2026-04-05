using UnityEngine;

public class SEScript : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip Click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(Click);
    }
}
