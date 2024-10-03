using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;

    [SerializeField] ParticleSystem crashVFX;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        StartCrashSequence();
        Debug.Log($"{this.name} triggered by {other.gameObject.name}");
    }

    void StartCrashSequence()
    {
        crashVFX.Play();

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
        
        GetComponent<PlayerController>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke(nameof(ReloadLevel), loadDelay);
    }

    void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
