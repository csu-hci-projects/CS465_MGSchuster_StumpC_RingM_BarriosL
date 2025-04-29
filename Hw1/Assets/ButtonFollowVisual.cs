using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.IO;
using System;


public class ButtonFollowVisual : MonoBehaviour
{
    public String pinNumber;
    public String timeText;
    public float _currentTime;
    public GameObject taskFinishedPanel;

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
    private XRBaseInteractable interactable;
    private bool isFollowing = false;

    public AudioSource audioSource;
    public AudioClip pushSound;
    private bool soundPlayed = false;


    void Start()
    {
        HideTaskFinished();
        initialLocalPos = visualTarget.localPosition;
        initialPos = visualTarget.position;
        _currentTime = 0;
        

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
        }
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
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

    private void ShowTaskFinished()
    {
        taskFinishedPanel.SetActive(true); // Show the panel or text
    }

    void HideTaskFinished()
    {
        taskFinishedPanel.SetActive(false);
    }


    private void PlayPushSound()
    {
        if (!soundPlayed && audioSource != null && pushSound != null)
        {
            audioSource.PlayOneShot(pushSound);
            soundPlayed = true;

            timeText = _currentTime.ToString("F2");
            string numberofPin = pinNumber.ToString(); 
            string filePath = @"C:\Users\Public\time.txt";
            string content = timeText + " PEN PIN: " + numberofPin + "\n";
            File.AppendAllText(filePath, content);

            ShowTaskFinished();

        }
    }
}

    