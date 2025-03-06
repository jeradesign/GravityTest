using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] private float thrustFactor = 1000;
    [SerializeField] private float rotationFactor = 10;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction zRotation;
    [SerializeField] private InputAction xRotation;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        thrust.Enable();
        zRotation.Enable();
        xRotation.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();           
    }

    // Update is called once per frame
    void Update()
    {
        if (thrust.IsPressed())
        {
            if (!_audioSource.isPlaying)
            {
                   _audioSource.PlayOneShot(thrustSound);
            }
            _rigidbody.AddRelativeForce(Vector3.up * (Time.deltaTime * thrustFactor), ForceMode.Acceleration);
        }
        else
        {
            _audioSource.Stop();
        }

        float zRotationInput = zRotation.ReadValue<float>();
        if (zRotationInput < 0)
        {
            ApplyRotation(rotationFactor, Vector3.forward);
        } else if (zRotationInput > 0)
        {
            ApplyRotation(-rotationFactor, Vector3.forward);
        }
        
        float xRotationInput = xRotation.ReadValue<float>();
        if (xRotationInput < 0)
        {
            ApplyRotation(rotationFactor, Vector3.right);
        } else if (xRotationInput > 0)
        {
            ApplyRotation(-rotationFactor, Vector3.right);
        }
    }

    private void ApplyRotation(float rotation, Vector3 axis)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(axis, rotation * Time.deltaTime);
        _rigidbody.freezeRotation = false;
    }
}
