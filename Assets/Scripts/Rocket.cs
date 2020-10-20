using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelLoad;
    [SerializeField] AudioClip death;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }

    int level = 0;

    State state = State.Alive;
    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){  
        ProcessInput();
        
    }

    private void ProcessInput(){
        if(state == State.Alive){
        RespondToThrustInput();
        RespondToRotateInput();
        }
    }

    private void RespondToThrustInput(){
        
        if(Input.GetKey(KeyCode.Space)){
           ApplyThrust();
        }
            else {
                audioSource.Stop();
            }
    }

    private void ApplyThrust(){
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if(!audioSource.isPlaying){
            audioSource.PlayOneShot(mainEngine);
            }
    }

    private void RespondToRotateInput(){

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
        if (state != State.Alive){ // ignore collisions when you're dead.
            return;
        }
        switch (collision.gameObject.tag){
            case "Friendly":
            // do nothing
            break;
            case "Finish":
            StartSuccessSequence();
            break;
            default:
            print("You die.");
            StartDeathSequence();
            break;
        }
    }

    private void StartSuccessSequence(){
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelLoad);
        Invoke ("LoadNextScene", 1f);
            
    }
    private void StartDeathSequence(){
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadNextScene(){
        level++;
        
        SceneManager.LoadScene(level); 
    }

    private void LoadFirstLevel(){
        level = 0;
        state = State.Alive;
        SceneManager.LoadScene(level);
    }
}