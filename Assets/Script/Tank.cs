using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 0.01f;
    
    [SerializeField]
    private float _reloadTime = 0.5f;
    
    [SerializeField]
    private float _bulletSpeed = 1f;
    
    [SerializeField] 
    private Rigidbody RB;
    
    [SerializeField] 
    private GameObject _bulletPrefab;

    private bool _onLeftEdge = false;
    
    void Start()
    {
        transform.position = new Vector3(22f, 3.4f, -0.2f);    
    }

    // Update is called once per frame
    void Update()
    {
        if (_onLeftEdge == false)
        {
            transform.Translate(Vector3.left * (_speed));
            if (transform.position.x < 10.49f)
            {
                _onLeftEdge = true;
                transform.Rotate(0, 180, 0);
            }
            {
             //   Instantiate(_bulletPrefab, transform.position + new Vector3(0f+_bulletSpeed, 0f,0f), Quaternion.identity);
              //  _bulletPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
            }
        }
        if (_onLeftEdge == true)
        {
            transform.Translate(Vector3.right * -(_speed));
            if (transform.position.x > 22.30f)
            {
                _onLeftEdge = false;
                transform.Rotate(0, 180, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_bulletPrefab, transform.position + new Vector3(0f, 0f,0f), Quaternion.identity);
            _bulletPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }

    }
}
