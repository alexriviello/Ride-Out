using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput(){
        Thrust();
        Rotate();
    }

    private void Thrust(){
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if(!audioSource.isPlaying){
            audioSource.Play();
            }
        }
            else {
                audioSource.Stop();
            }

    }

    private void Rotate(){

        rigidBody.freezeRotation = true; // take manual control of rotation for a bit
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // return physics control to normal
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag){
            case "Friendly":
            print("Friendly!");
            // do nothing
            break;
            default:
            print("You die.");
            break;
        }

      //   foreach (ContactPoint contact in collision.contacts)
       // {
         //   Debug.DrawRay(contact.point, contact.normal, Color.white);
       // }
        //if (collision.relativeVelocity.magnitude > 2)
        //    audioSource.Play();
    }
}