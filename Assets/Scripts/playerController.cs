//using UnityEngine;

//public class playerController : MonoBehaviour
//{
//    public float speed;

//    Rigidbody rb;
//    Vector3 velocity;
//    // Use this for initialization
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKey("w"))
//        {
//            velocity.x += speed * Time.deltaTime;
//        }
//        if (Input.GetKey("d"))
//        {
//            velocity.z -= speed * Time.deltaTime;
//        }
//        if (Input.GetKey("s"))
//        {
//            velocity.x -= speed * Time.deltaTime;
//        }
//        if (Input.GetKey("a"))
//        {
//            velocity.z += speed * Time.deltaTime;
//        }
//        rb.velocity = velocity;
//    }
//}
