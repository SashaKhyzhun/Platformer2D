using UnityEngine;

public class Rails : MonoBehaviour {

    public Transform pathObject;

    private Transform myTransform;
    private Transform[] nodes;
    private Vector3 translation;
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

    void LateUpdate()
    {
        if (currentNode < nodesCount)
        {
            if (myTransform.position.x > nodes[currentNode].position.x)
            {
                currentNode++;
            }
        }
        if (currentNode > 0)
        {
            if (myTransform.position.x < nodes[currentNode - 1].position.x)
            {
                currentNode--;
            }
        }
        if (currentNode < nodesCount && currentNode > 0)
        {
            distance = nodes[currentNode].position.x - nodes[currentNode - 1].position.x;
            distanceCovered = myTransform.position.x - nodes[currentNode - 1].position.x;
            fraction = distanceCovered / distance;
            posY = Mathf.Lerp(nodes[currentNode - 1].position.y, nodes[currentNode].position.y, fraction) - myTransform.position.y;
            Debug.Log(currentNode);
            translation = new Vector3(0, posY, 0);
            transform.Translate(translation);
        }
    }
}
