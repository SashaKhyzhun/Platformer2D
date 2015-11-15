using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform[] backgrounds;
    public float smoothing = 1f;
    public float offset;

    private Transform cam;
    private Vector3 previousCamPos;
    private float[] parallaxScales;
    private float[] bgExtents;
    private float camPosX;
    private float camExtent;
    private float rightSpriteBorder;
    //private float leftNextSpriteBorder;

    public float leftCameraBorder { get; set; }
    public float rightCameraBorder{ get; set; }

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
        camExtent = Camera.main.orthographicSize * Camera.main.aspect;

        parallaxScales = new float[backgrounds.Length];
        bgExtents = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].localPosition.z > 0)
            {
                parallaxScales[i] = 1 / backgrounds[i].localPosition.z;
                bgExtents[i] = backgrounds[i].GetComponent<Renderer>().bounds.extents.x;
            }
        }
    }
    
    void Update()
    {
        camPosX = cam.position.x;

        leftCameraBorder = camPosX - camExtent - offset;
        rightCameraBorder = camPosX + camExtent + offset;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing);

            // Manage Background
            rightSpriteBorder = backgrounds[i].position.x + bgExtents[i];
            //leftNextSpriteBorder = rightSpriteBorder - 0.05f;

            if (leftCameraBorder >= rightSpriteBorder)
            {
                backgrounds[i].position = backgrounds[i].position + new Vector3(4 * bgExtents[i] - 0.05f, 0);
            }
            //if (rightCameraBorder <= leftNextSpriteBorder)
            //{
            //    backgrounds[i].position = backgrounds[i].position - new Vector3(4 * bgExtents[i] + 0.05f, 0);
            //}
        }
        previousCamPos = cam.position;
        //ManageBackground(); 
    }

    //void ManageBackground()
    //{
    //    for (int i = 0; i < backgrounds.Length; i++)
    //    {
            
    //    }
    //}
}
