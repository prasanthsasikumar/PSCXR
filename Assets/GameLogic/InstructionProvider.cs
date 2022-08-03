using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionProvider : MonoBehaviour
{
    public float distance = 1f;
    public bool placeInFront = false;
    private Camera playerCamera;

    public Text InstructionText;
    public RawImage InstructionImage;
    public GameObject completedOverlay;

    // Update is called once per frame
    public void PlaceInstructionInfront()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        this.transform.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
        this.transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);
    }

    public void ToggleVisibility()
    {
        if (this.transform.gameObject.activeSelf) this.transform.gameObject.SetActive(false);
        else {
            this.transform.gameObject.SetActive(true);
            if(placeInFront) PlaceInstructionInfront();
        }
    }

    public void HideInstruction()
    {
        this.transform.gameObject.SetActive(false);
    }
    public void ShowInstruction()
    {
        this.transform.gameObject.SetActive(true);
    }

    public void SetInstruction(string description, Texture texture)
    {
        InstructionText.text = description;
        InstructionImage.texture = texture;
    }

    public void InstructionComplete()
    {
        completedOverlay.SetActive(true);
    }

}