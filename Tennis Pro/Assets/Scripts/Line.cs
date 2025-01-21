using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    [SerializeField] private HealthSlider _slider;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("hola");
            
            // Get the position of the line's transform
            Vector3 linePosition = transform.position;

            // Cast a ray from the line's position in the negative y-axis direction
            Ray ray = new Ray(linePosition, Vector3.down);
            RaycastHit hit;

            // Check if the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                HandleHitObject(hit);
            }
            gameObject.SetActive(false);
        }
    }

    private void HandleHitObject(RaycastHit hit)
    {
        // Check the tag of the hit object and update the slider accordingly
        if (hit.collider.CompareTag("one"))
        {
            Debug.Log("one");
            _slider.sliderValue += 0.2f;
        }
        else if (hit.collider.CompareTag("oneANDhalf"))
        {
            Debug.Log("1.5");
            _slider.sliderValue += 0.3f;
        }
        else if (hit.collider.CompareTag("two"))
        {
            Debug.Log("two");
            _slider.sliderValue += 0.5f;
        }
    }
   /* private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("one") && Input.GetMouseButtonDown(0))
        {
            
            Debug.Log("one");
            _slider.sliderValue += 0.1f;
            //onemultiplier = true;


        }
        else if (other.gameObject.CompareTag("oneANDhalf") && Input.GetMouseButtonDown(0))
        {
            Debug.Log("1.5");
            _slider.sliderValue += 0.25f;
            //oneandhalfmultiplier = true;
        }
        else if (other.gameObject.CompareTag("two") && Input.GetMouseButtonDown(0))
        {
            Debug.Log("two");
            _slider.sliderValue += 0.34f;
            //twomultiplier = true;
        }
       
    }*/
}
