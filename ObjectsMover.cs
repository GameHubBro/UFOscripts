using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsMover : MonoBehaviour
{
    [SerializeField] private float minZ = - 500f;
    [SerializeField] private float maxZ = 500f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private bool isMovingRight = true;

    void FixedUpdate()
    {
        if (isMovingRight)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        gameObject.transform.position += Vector3.forward * speed;

        if (gameObject.transform.position.z >= maxZ)
        {
            TeleportObj(minZ);
        }
    }

    private void MoveLeft()
    {
        gameObject.transform.position -= Vector3.forward * speed;

        if (gameObject.transform.position.z <= minZ)
        {
            TeleportObj(maxZ);
        }
    }

    private void TeleportObj(float endPos)
    {
        Vector3 startPos = gameObject.transform.position;
        startPos.z = endPos;
        gameObject.transform.position = startPos;
    }
}
