using UnityEngine;

public class Rails : MonoBehaviour {

    public Transform pathObject;

    private Transform myTransform;
    private Transform[] nodes;
    private Vector3 endPosition;
    private float fraction;
    private float distance;
    private float distanceCovered;
    private float posY;
    private int currentNode = 0;
    private int nodesCount;

    void Start()
    {
        myTransform = transform;
        nodesCount = pathObject.childCount;
        nodes = new Transform[nodesCount];
        for(int i = 0; i < nodesCount; i++)
        {
            nodes[i] = pathObject.GetChild(i);
        }
    }

    void Update()
    {
        if (currentNode < nodesCount - 1)
        {
            if (myTransform.position.x > nodes[currentNode].position.x)
            {
                currentNode++;
                distance = nodes[currentNode].position.x - nodes[currentNode - 1].position.x;
            }
            distanceCovered = myTransform.position.x - nodes[currentNode - 1].position.x;
            fraction = distanceCovered / distance;
            Debug.Log(fraction);
            posY = Mathf.Lerp(nodes[currentNode - 1].position.y, nodes[currentNode].position.y, fraction);
        }        
        endPosition = new Vector3(myTransform.position.x, posY, myTransform.position.z);
    }

    void LateUpdate()
    {
        transform.position = endPosition;
    }
}
