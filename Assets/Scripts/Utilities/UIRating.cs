using UnityEngine;
using System.Collections;

public class UIRating : MonoBehaviour
{
    public void Rate()
    {
        Application.OpenURL("market://details?id=" + Application.bundleIdentifier);
    }
}
