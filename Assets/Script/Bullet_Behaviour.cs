using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _bulletSpeed * Time.deltaTime);
        if (transform.position.y > 10f)
        {
            Destroy(this.gameObject);
        }
    }
    
}
