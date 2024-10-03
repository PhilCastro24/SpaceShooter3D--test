using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Keyboard & Mouse Keybindings")]
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;

    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based on the player input")]
    [SerializeField] float playerSpeed = 10f;
    [Tooltip("How fast ship moves horizontally")]
    [SerializeField] float xRange = 10f;
    [Tooltip("How fast ship moves verticaly")]
    [SerializeField] float yRange = 7f;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;

    [Header("Player input based tuning")]
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -20f;

    [Header("Laser projectiles array")]
    [Tooltip("Add lasers here")]
    [SerializeField] GameObject[] lasers;

    float xThrow, yThrow;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessTranslation()
    {
        Vector2 inputVector = movement.ReadValue<Vector2>();

        xThrow = Mathf.Lerp(xThrow, inputVector.x, Time.deltaTime * playerSpeed);
        //Debug.Log(xThrow);

        yThrow = Mathf.Lerp(yThrow, inputVector.y, Time.deltaTime * playerSpeed);
        //Debug.Log(yThrow);

        float xOffset = xThrow * Time.deltaTime * playerSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * playerSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        float pitchDueToPositon = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float pitch = pitchDueToPositon + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring()
    {
        if (fire.ReadValue<float>() > 0.5f)
        {
            SetLasersActive(true);
            //Debug.Log("Player is shooting");
        }
        else
        {
            SetLasersActive(false);
            //Debug.Log("Player is not shooting");
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
