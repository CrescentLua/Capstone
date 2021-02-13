﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyPlayer : MonoBehaviourPun
{
    List<string> Inventory = new List<string>();
    
    public Camera camera; 
    CharacterController characterController; 
    Vector3 playerVel;
    float WalkSpeed = 8.0f;
    float gravity = -9.81f;
    float thickness = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();

        SceneManager.activeSceneChanged += ChangedActiveScene; //Call the ChangedActiveScene Method whenever the scene changes 
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && base.photonView.IsMine)
        {
            float XAxis = Input.GetAxis("Horizontal");
            float ZAxis = Input.GetAxis("Vertical");

            Vector3 move = transform.right * XAxis + transform.forward * ZAxis;
            characterController.Move(move * Time.deltaTime * WalkSpeed);

            playerVel.y += gravity * Time.deltaTime;
            characterController.Move(playerVel * Time.deltaTime);
          
        }

        else if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && !base.photonView.IsMine)
        {
            Destroy(characterController);
            Destroy(camera.transform.parent.GetComponent<CameraRotation>());
        }

        else
        {
            float XAxis = Input.GetAxis("Horizontal");
            float ZAxis = Input.GetAxis("Vertical");

            Vector3 move = transform.right * XAxis + transform.forward * ZAxis;
            characterController.Move(move * Time.deltaTime * WalkSpeed);

            playerVel.y += gravity * Time.deltaTime;
            characterController.Move(playerVel * Time.deltaTime);
        }
    }

   public void AddToInventory(string item_)
    {
        Inventory.Add(item_);
    }

    public bool CheckInventory(string item_)
    {
        if (Inventory.Contains(item_))
        {
            return true; 
        }

        else
        {
            return false;
        }
    }


    private void ChangedActiveScene(Scene current, Scene next)
    {
        if(next.name.Contains("Escape"))
        {

            Transform SpawnPoint = GameObject.Find("Spawn").transform;
            Vector2 offset = Random.insideUnitCircle * 3.0f;
            Vector3 newposition = new Vector3(SpawnPoint.position.x + offset.x, SpawnPoint.position.y, SpawnPoint.position.z);
            
            transform.position = new Vector3(newposition.x, newposition.y, newposition.z);
        }
    }
}

