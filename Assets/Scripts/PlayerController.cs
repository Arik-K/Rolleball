using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.IO;

public class PlayerController : MonoBehaviour
{
    // public float speed = 0;
    public float rotationSpeed = 100.0f;
    public float movementSpeed = 5.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    private Rigidbody rb;
    private int count;
    private int bombs;
    private float movementX;
    private float movementY;

    private string filePath;
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody>(); 
        count = 0;
        bombs = 0;
        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        filePath = Path.Combine(Application.dataPath, "myPath.txt");
        writer = File.AppendText(filePath);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

       void SetCountText() 
   {
       countText.text =  "Count: " + count.ToString();
        if (count >= 8)
       {
           winTextObject.SetActive(true);
       }
       if(bombs >= 3)
       {
        loseTextObject.SetActive(true);
       }
   }

    private void FixedUpdate() 
    {
        // Move like a ball
        // Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        // rb.AddForce(movement * speed);

        // Move like a car
        // Rotate left and right
        transform.Rotate(Vector3.forward * movementX * rotationSpeed * Time.deltaTime);
        
        // Move forward and backward
        // if (movementY > 0)
        Vector3 movement = transform.up * movementY * movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Log
        LogPositionAndRotation();

    }

    void LogPositionAndRotation()
    {
        float timestamp = Time.time;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        float rotationY = rotation.eulerAngles.y;
        writer.WriteLine($"{timestamp} {position.x} {position.y} {position.z} {rotationY}");
        writer.Flush();
    }

    void OnTriggerEnter(Collider other) 
   {
         if (other.gameObject.CompareTag("PickUp")) 
       {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
       }

        if (other.gameObject.CompareTag("Bomb")) 
       {
            other.gameObject.SetActive(false);
            count = count - 1;
            bombs = bombs + 1;
            SetCountText();
       }
        
   }

    private void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
