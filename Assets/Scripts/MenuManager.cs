using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Date()
    {
        GameManager.dateBool = true;
        SceneManager.LoadScene("SampleScene");
    }

    public void Follower()
    {
        GameManager.followerBool = true;
        SceneManager.LoadScene("SampleScene");
    }
}
