using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isNight = false;
    [SerializeField]
    private Light sun;
    [SerializeField]
    private Light discoLightOne;
    [SerializeField]
    private Light discoLightTwo;
    [SerializeField]
    private Light discoLightThree;
    [SerializeField]
    private Light streetLight;
    [SerializeField]
    private Light streetLight2;
    [SerializeField]
    private Light streetLight3;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isNight = !isNight;
            if (isNight)
            {
                var rotationVector = sun.transform.rotation.eulerAngles;
                rotationVector.x = 0;
                sun.transform.rotation = Quaternion.Euler(rotationVector);
                discoLightOne.gameObject.SetActive(true);
                discoLightTwo.gameObject.SetActive(true);
                discoLightThree.gameObject.SetActive(true);
                streetLight.gameObject.SetActive(true);
                streetLight2.gameObject.SetActive(true);
                streetLight3.gameObject.SetActive(true);
            }
            else
            {
                var rotationVector = sun.transform.rotation.eulerAngles;
                rotationVector.x = 35;
                sun.transform.rotation = Quaternion.Euler(rotationVector);
                discoLightOne.gameObject.SetActive(false);
                discoLightTwo.gameObject.SetActive(false);
                discoLightThree.gameObject.SetActive(false);
                streetLight.gameObject.SetActive(false);
                streetLight2.gameObject.SetActive(false);
                streetLight3.gameObject.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
