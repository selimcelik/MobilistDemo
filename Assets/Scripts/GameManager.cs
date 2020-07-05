using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct ProfileList
    {
        public User user;
        public List<Feed> feed;
    }

    [Serializable]
    public struct Feed
    {
        public string photo;
        public string Name;
        public string FollowerCount;
        public string CreatedAt;
    }

    [Serializable]
    public struct User
    {
        public string profilePhoto;
        public string coverPhoto;
        public string id;
        public string name;
        public string email;
        public string bio;

    }
    public static GameManager instance;


    RawImage membersPP;
    Text membersUserName;
    Text membersFollowers;
    Text membersTime;

    RawImage userProfilePhoto;
    Text userName;
    Text userBio;

    public ScrollRect scrollView;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;


    public ProfileList profileList;

    float waveCondition = 880;
    int paginationRowCount = 10;

    List<Feed> totalFeedList = new List<Feed>();

    public static bool parallaxBool = false;
    public static bool scroollBool = false;

    public static bool dateBool = false;
    public static bool followerBool = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        scrollItemPrefab.transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(GetInfos());
        scroollBool = false;
        parallaxBool = false;
        //followerBool = false;
        //dateBool = false;
    }

    private void Update()
    {

        //if (ButtonController.sortByDateBool)
        //{
        //    Debug.Log(ButtonController.sortByDateBool);
        //    sortByDate();
        //}
    }

    public void scrolling()
    {
        scroollBool = true;
    }


    public void waveFonc()
    {
        if (scrollContent.transform.position.y >= waveCondition)
        {
            StartCoroutine(getPaginationData(paginationRowCount.ToString()));
            waveCondition = waveCondition * 1.3f;
            paginationRowCount = paginationRowCount + 10;
            parallaxBool = false;
        }

        if(scrollContent.transform.position.y <= -300)
        {
            parallaxBool = true;
            scroollBool = false;


        }


    }

    IEnumerator GetInfos()
    {
        string url = "http://wamp.mobilist.com.tr/challenge/index.php";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.chunkedTransfer = false;
        yield return request.Send();

        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("http://wamp.mobilist.com.tr/challenge/images/profilePhoto.png");
        yield return uwr.SendWebRequest();

        userProfilePhoto = scrollItemPrefab.transform.GetChild(1).FindChild("ProfilePhoto").GetComponent<RawImage>();

        userProfilePhoto.texture = DownloadHandlerTexture.GetContent(uwr);




        //if (request.isError)
        //{
        //    Debug.Log("Hata");

        //    //show message "no internet "
        //}
        //else
        //{
            if (request.isDone)
            {
               // Debug.Log("Oldu"+request.responseCode);
                profileList = JsonUtility.FromJson<ProfileList>(request.downloadHandler.text);
            //Debug.Log("değer = " + profileList.feed[0].Name);

                if (dateBool)
                {
                    profileList.feed.Sort((f1, f2) => System.DateTime.Parse(f1.CreatedAt).CompareTo(System.DateTime.Parse(f2.CreatedAt)));

                    profileList.feed.Reverse();
                }

                if (followerBool)
                {
                    profileList.feed.Sort((f1, f2) => Convert.ToInt32(f1.FollowerCount).CompareTo(Convert.ToInt32(f2.FollowerCount)));

                    profileList.feed.Reverse();
                }

                for (int i = 0; i< profileList.feed.ToArray().Length; i++)
                {
                    totalFeedList.Add(profileList.feed[i]);
                }

                generateList();
            }
        
    }

    void generateList()
    {
        userProfilePhoto = scrollItemPrefab.transform.GetChild(1).FindChild("ProfilePhoto").GetComponent<RawImage>();
       // Debug.Log(userProfilePhoto);
        userName = userProfilePhoto.transform.GetChild(0).GetComponent<Text>();
       // Debug.Log(userName);
        userBio = userProfilePhoto.transform.GetChild(1).GetComponent<Text>();
       // Debug.Log(userBio);

        //Texture2D texture = userProfilePhoto.canvasRenderer.GetMaterial().mainTexture as Texture2D;
        userName.text = profileList.user.name;
        userBio.text = profileList.user.bio;

        StartCoroutine(getPaginationData(""));

        
    }

    IEnumerator getPaginationData(string rowPerPage)
    {

        if (rowPerPage != "")
        {
            string url = "http://wamp.mobilist.com.tr/challenge/index.php?start="+rowPerPage;
            UnityWebRequest request2 = UnityWebRequest.Get(url);
            request2.chunkedTransfer = false;
            yield return request2.Send();

            if (request2.isDone)
            {
              //  Debug.Log("Oldu" + request2.responseCode);
                profileList = JsonUtility.FromJson<ProfileList>(request2.downloadHandler.text);


                //Date'a göre sıralama
                if (dateBool)
                {
                    profileList.feed.Sort((f1, f2) => System.DateTime.Parse(f1.CreatedAt).CompareTo(System.DateTime.Parse(f2.CreatedAt)));

                    profileList.feed.Reverse();

                }

                //Follower'a göre sıralama
                if (followerBool)
                {
                    profileList.feed.Sort((f1, f2) => Convert.ToInt32(f1.FollowerCount).CompareTo(Convert.ToInt32(f2.FollowerCount)));

                    profileList.feed.Reverse();
                }

                for (int i = 0; i < profileList.feed.ToArray().Length; i++)
                {
                    totalFeedList.Add(profileList.feed[i]);
                }

            }
        }

        Debug.Log("length = " + profileList.feed.ToArray().Length);
        for (int i = 1; i <= profileList.feed.ToArray().Length; i++)
        {


            membersPP = scrollItemPrefab.transform.GetChild(0).FindChild("Pp").GetComponent<RawImage>();
          //  Debug.Log(membersPP);
            membersUserName = scrollItemPrefab.transform.GetChild(0).FindChild("UserName").GetComponent<Text>();
           // Debug.Log(membersUserName);
            membersFollowers = scrollItemPrefab.transform.GetChild(0).FindChild("Followers").GetComponent<Text>();
           // Debug.Log(membersFollowers);
            membersTime = scrollItemPrefab.transform.GetChild(0).FindChild("Time").GetComponent<Text>();
           // Debug.Log(membersTime);

            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(profileList.feed[i - 1].photo);
            yield return uwr.SendWebRequest();

            membersPP.texture = DownloadHandlerTexture.GetContent(uwr);
            membersUserName.text = profileList.feed[i - 1].Name;
            membersFollowers.text = profileList.feed[i - 1].FollowerCount;
            membersTime.text = profileList.feed[i - 1].CreatedAt;


            if (i > 1)
            {
                GameObject SIP = scrollItemPrefab.transform.GetChild(1).gameObject;
                SIP.SetActive(false);
            }

            generateItem();


        }

        //scrollView.verticalNormalizedPosition = 1;
    }

    //private IComparer<Feed> Comparer()
    //{
    //    throw new NotImplementedException();
    //}

    //private IComparer<Feed> Comparer(object f1, object f2)
    //{
    //    throw new NotImplementedException();
    //}

    private void generateItem()
    {
        GameObject scrollItemObj = Instantiate(scrollItemPrefab);

        scrollItemObj.transform.SetParent(scrollContent.transform, false);
    }

    //public void sortByDate()
    //{
    //    totalFeedList.Sort((f1, f2) => System.DateTime.Parse(f1.CreatedAt).CompareTo(System.DateTime.Parse(f2.CreatedAt)));

    //    totalFeedList.Reverse();

    //    profileList.feed = totalFeedList;


    //}

    //public void sortByFollowerCount()
    //{
    //    totalFeedList.Sort((f1, f2) => System.DateTime.Parse(f1.CreatedAt).CompareTo(System.DateTime.Parse(f2.CreatedAt)));

    //    totalFeedList.Reverse();
    //}
}
