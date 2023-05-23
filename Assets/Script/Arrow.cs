using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(8.17f, 11f, 0f); 
    }

    [SerializeField] private float _arrowSpeed = 0.03f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _arrowSpeed);
        if (transform.position.y < 1f)
        {
            transform.position = new Vector3(8.17f, 11f, 0f);
        }
    }
}
