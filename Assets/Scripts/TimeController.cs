using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject pastMap;
    public GameObject presentMap;
    public static bool isPresent = true;
    public GameObject TimeLeapVFX;
    public Material material;
    private SpriteRenderer spriteRenderer;
    public Camera Maincamera;
    private static Vector3 MainCameraPosition;
    private bool startTransition1 = false; 
    private bool startTransition2 = false; 
    private static bool usedTimeLeap = false;
    private float transitionTime;
    public float duration = 1f; 
    public static bool operatable = true;
    // private float t;
    // public float t1;
    // public float t2;
    // public float t3;
    // public float speed1;
    // public float speed2;
    private static bool setupTime = true;
    void Start()
    {
        transitionTime = 0;
        material = TimeLeapVFX.GetComponent<SpriteRenderer>().material;
        spriteRenderer = TimeLeapVFX.GetComponent<SpriteRenderer>();
        Maincamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (isPresent) {
            presentMap.SetActive(true);
            pastMap.SetActive(false);
        }
        else {
            pastMap.SetActive(true);
            presentMap.SetActive(false);
        }
    }

    public IEnumerator TimeTravel() 
    {
        yield return new WaitForSeconds(2);
        isPresent = !isPresent;
        if (isPresent) {
            presentMap.SetActive(true);
            pastMap.SetActive(false);
        }
        else {
            pastMap.SetActive(true);
            presentMap.SetActive(false);
        }
        startTransition2 = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && setupTime)
        {   
            print("Start1: " + startTransition1);
            print("Start2: " + startTransition2);
        
            setupTime = false;
            transitionTime = 0;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetInteger("direction", 3);
            PlayerMovement.moveable = false;
            print("player moveable: " + PlayerMovement.moveable);
            usedTimeLeap = true;
            startTransition1 = true;
            Vector3 position = Maincamera.transform.position;
            MainCameraPosition = position;
            position.z = 0;
            TimeLeapVFX.transform.position = position;
            TimeLeapVFX.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(TimeTravel());
            // transitionTime = 0;
        }

        if (startTransition1){
            transitionTime += Time.deltaTime;
            float lerpFactor = transitionTime / duration;
            float nonlinearFactor = lerpFactor * lerpFactor * lerpFactor * lerpFactor * lerpFactor * lerpFactor * lerpFactor;
            // print(lerpFactor);
            Color newColor = Color.Lerp(Color.white, Color.black, lerpFactor * lerpFactor);
            // float a = 1;
            float a = Mathf.Lerp(1, 20, nonlinearFactor);
            material.SetFloat("_a", a);
            TimeLeapVFX.GetComponent<SpriteRenderer>().color = newColor;

        
            if (lerpFactor >= 1.0f)
            {
                startTransition1 = false;
                material.SetFloat("_a", 1);
                transitionTime = 0;
            }
        }

        if (startTransition2 && usedTimeLeap){
            PlayerMovement.moveable = false;
            StartCoroutine(Waiting());
            transitionTime += Time.deltaTime;
            float lerpFactor = transitionTime / duration;
            float nonlinearFactor = lerpFactor * lerpFactor  ;
            float a = Mathf.Lerp(-20, 1, nonlinearFactor);
            material.SetFloat("_a", a);
            Color newColor = Color.Lerp(Color.black, Color.white, lerpFactor * lerpFactor * lerpFactor);
            // float a = 1;
            
            TimeLeapVFX.GetComponent<SpriteRenderer>().color = newColor;

        
            if (lerpFactor >= 1.0f)
            {
                startTransition2 = false;
                TimeLeapVFX.GetComponent<SpriteRenderer>().enabled = false;
                setupTime = true;
                PlayerMovement.moveable = true;
                transitionTime = 0;
                print("now you can move");
            }
        }
    }


    private IEnumerator Waiting(){
        yield return new WaitForSeconds(2);
        
    }

    public void OnbuttonClick(){
        if (setupTime)
        {   
            print("Start1: " + startTransition1);
            print("Start2: " + startTransition2);
        
            setupTime = false;
            transitionTime = 0;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetInteger("direction", 3);
            PlayerMovement.moveable = false;
            print("player moveable: " + PlayerMovement.moveable);
            usedTimeLeap = true;
            startTransition1 = true;
            Vector3 position = Maincamera.transform.position;
            MainCameraPosition = position;
            position.z = 0;
            TimeLeapVFX.transform.position = position;
            TimeLeapVFX.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(TimeTravel());
            // transitionTime = 0;
        }
    }
}