using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;

    private Vector3 initialLocalPos;
    private Vector3 initialPos;
    public Vector3 threshold;
    private Vector3 offset;
    public float resetSpeed = 10;
    public Vector3 disapear;
    public Transform pin;

    private Transform pokeAttachTransform;

    private bool freeze = false;


    private XRBaseInteractable interactable;
    private bool isFollowing = false;

    public AudioSource audioSource; 
    public AudioClip pushSound;    
    private bool soundPlayed = false; // Used to play the sound only once per pin push event.

    // Start is called before the first frame update
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;
        initialPos = visualTarget.position;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            freeze = false;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (freeze)
        {
            PlayPushSound();
            pin.position = pin.TransformPoint(disapear);
            return;
        }
        if (visualTarget.position.y >= initialPos.y + threshold.y)
        {
            PlayPushSound();
            pin.position = pin.TransformPoint(disapear);
            return;
        }

        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);

            Vector3 contrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            visualTarget.position = visualTarget.TransformPoint(contrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }

    // This method plays the push sound once.
    private void PlayPushSound()
    {
        if (!soundPlayed && audioSource != null && pushSound != null)
        {
            audioSource.PlayOneShot(pushSound);
            soundPlayed = true;
        }
    }
}