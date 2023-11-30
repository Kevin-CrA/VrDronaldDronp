using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ObjectResizer : MonoBehaviour
{
    public Slider sizeSlider;        // Reference to the UI Slider
    public GameObject objectToResize; // Reference to the object you want to resize
    public Button resetButton;       // Reference to the reset button
    public Text apiText;             // Reference to the UI Text for API data

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private const string apiUrl = "https://pokeapi.co/api/v2/pokemon";

    void Start()
    {
        // Set the slider's initial value to 0.5 (middle)
        sizeSlider.value = 0.5f;

        // Store the original scale and position
        originalScale = objectToResize.transform.localScale;
        originalPosition = objectToResize.transform.position;

        // Call the ResizeObject method when the slider value changes
        sizeSlider.onValueChanged.AddListener(ResizeObject);

        // Call the ResetObject method when the button is clicked
        resetButton.onClick.AddListener(ResetObject);

        // Start the coroutine to update the API text every 5 seconds
        StartCoroutine(UpdateApiTextCoroutine());
    }

    void ResizeObject(float sliderValue)
    {
        // Calculate the new scale based on the slider value
        float scale = Mathf.Pow(5, sliderValue * 2 - 1);

        // Set the object's scale
        objectToResize.transform.localScale = new Vector3(scale, scale, scale);
    }

    void ResetObject()
    {
        // Reset the object to its original scale and position
        objectToResize.transform.localScale = originalScale;
        objectToResize.transform.position = originalPosition;

        // Reset the slider to the middle position
        sizeSlider.value = 0.5f;
    }


    IEnumerator UpdateApiTextCoroutine()
    {
        while (true)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result ==UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode data = SimpleJSON.JSON.Parse(json);
                apiText.text = data["results"][0]["name"];
                Debug.Log("Data: " + data["results"][0]["name"]);
            }
        }

            // Wait for 5 seconds before the next API request
            yield return new WaitForSeconds(5f);
        }
    }
}
