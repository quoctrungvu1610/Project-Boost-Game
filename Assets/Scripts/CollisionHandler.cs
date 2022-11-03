using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    

    Movement playerMovement;
    AudioSource audioSource;

    bool isTransitioning;
    bool collisionDisable = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Rocket").GetComponent<Movement>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ResponseToDebugKey();
    }

    void ResponseToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable)
        {
            return;
        }
        
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("This thing is friendly");
                    break;
                case "Finish":
                    StartSuccessSequense();
                    break;
                case "Fuel":
                    Debug.Log("You pick up Fuel");
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        
        
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartCrashSequence()
    {
        // to do add SFX upon crash
        //to do add particle effect upon crash
       
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        playerMovement.enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    void StartSuccessSequense()
    {
        
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        playerMovement.enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
}
