using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caliberation : MonoBehaviour
{
    public GameObject Nback, mirror, dome;
    public float duration = 2f;
    public float farDistance = 15f, closeDistance=0.6f;
    public Material black;

    private GameObject maincamera;
    private Material skybox;


    // Start is called before the first frame update
    void Start()
    {
        maincamera = GameObject.Find("Main Camera");
        skybox = RenderSettings.skybox;
        ClearUp();
    }

    public void ClearUp()
    {
        ShowNback(false);
        //ShowEnvironment(false);
        //RenderSettings.skybox = black;
        mirror.transform.position = maincamera.transform.position + maincamera.transform.forward * farDistance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) ActivateMirror();
        else if(Input.GetKeyDown(KeyCode.D)) DeActivateMirror();
    }

    public void ShowNback(bool value)
    {
        if (value) Nback.SetActive(true);
        else Nback.SetActive(false);
    }

    public void StartNback()
    {
        Nback.GetComponent<NBackManager>().StartTest();
    }
    public void ResetNBack()
    {
        Nback.GetComponent<NBackManager>().Reset();
    }
    public void SetNback(int n)
    {
        Nback.GetComponent<NBackManager>().settings.n = n;
    }

    public void ShowEnvironment(bool value)
    {
        if (value) dome.SetActive(true);
        else dome.SetActive(false);
    }

    public void ActivateMirror()
    {
        Vector3 startPosition = maincamera.transform.position + maincamera.transform.forward * farDistance;
        Vector3 endPosition = maincamera.transform.position + maincamera.transform.forward * closeDistance;
        mirror.transform.SetPositionAndRotation(startPosition, mirror.transform.rotation);
        mirror.SetActive(true);
        StartCoroutine(BringCloser(startPosition, endPosition, black));
    }

    public void DeActivateMirror()
    {
        RenderSettings.skybox = skybox;
        Vector3 startPosition = maincamera.transform.position + maincamera.transform.forward * closeDistance;
        Vector3 endPosition = maincamera.transform.position + maincamera.transform.forward * farDistance;
        StartCoroutine(BringCloser(startPosition, endPosition, skybox));
    }

    IEnumerator BringCloser(Vector3 startPosition, Vector3 endPosition, Material material)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t/duration);
            mirror.transform.SetPositionAndRotation(interpolatedPosition, mirror.transform.rotation);
            yield return null;
        }
        RenderSettings.skybox = material;
    }

    public void FadeInObject(GameObject obj)
    {
        Color startColor = obj.GetComponent<MeshRenderer>().material.color;
        Color endColor = startColor;
        endColor.a = 1;
        FadeIn(startColor, endColor, obj.GetComponent<MeshRenderer>().material);
    }

    IEnumerator FadeIn(Color starColor, Color endColor, Material material)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            material.color = Color.Lerp(starColor, endColor, t / duration);
            yield return null;
        }
    }

}
