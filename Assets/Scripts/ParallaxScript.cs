using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxScript : MonoBehaviour
{
    Image coverPhoto;
    RawImage userPP;

    private bool parallaxActive = false;
    // Start is called before the first frame update
    void Start()
    {
        parallaxActive = false;
        coverPhoto = gameObject.GetComponent<Image>();
        userPP = GameObject.Find("ProfilePhoto").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.parallaxBool)
        {
            //coverPhoto.transform.position = new Vector3(-2.1f, 187, 0);
            coverPhoto.rectTransform.sizeDelta = new Vector2(850f, 700f);
            parallaxActive = true;
            //userPP.transform.position = new Vector3(0, -17, 0);
        }
        else
        {
            //coverPhoto.transform.position = new Vector3(-2.1f, 267.8f, 0);
            coverPhoto.rectTransform.sizeDelta = new Vector2(850f, 648.6f);
            //userPP.transform.position = new Vector3(0, 17, 0);

        }
    }
}
