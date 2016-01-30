using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBScript : MonoBehaviour {

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUserName;
    public GameObject DialogUserPic;

    void Awake() {
        FB.Init(SetInit, OnHideUnity);

    }


    void SetInit() {

        if (FB.IsLoggedIn) {
            Debug.Log("FB is logged in");
        } else {
            Debug.Log("FB is not logged in");
        }

        DealWithMenus(FB.IsLoggedIn);
    }

    void OnHideUnity(bool isGameShown) {

        if (!isGameShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }

    }

    public void FBlogin() {

        List<string> permissions = new List<string>();
        permissions.Add("public_profile");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result) {

        if (result.Error != null) {
            Debug.Log(result.Error);
        } else {
            if (FB.IsLoggedIn) {
                Debug.Log("FB is logged in");
            } else {
                Debug.Log("FB is not logged in");
            }

            DealWithMenus(FB.IsLoggedIn);
        }

    }

    void DealWithMenus(bool isLoggedIn) {

        if (isLoggedIn) {
            DialogLoggedIn.SetActive(true);
            DialogLoggedOut.SetActive(false);

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            
        } else {
            DialogLoggedIn.SetActive(false);
            DialogLoggedOut.SetActive(true);
        }

    }

    void DisplayUserName(IResult result) {

        Text UserName = DialogUserName.GetComponent<Text>();

        if (result.Error == null) {
            UserName.text = "Hi there, " + result.ResultDictionary["first_name"];
        } else {
            Debug.Log(result.Error);
        }

    }

    void DisplayProfilePic(IGraphResult result) {

        if (result.Texture != null) { 
            Image UserPic = DialogUserPic.GetComponent<Image>();

            UserPic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());


        } 

    }



}
