using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float speed = 5f;
    private Transform rotator;
    
    private void Start()
    {
        rotator = GetComponent<Transform>();
    }

    private void Update()
    {
        //вращение камеры по оси Y, центром оси является центр платформы
        rotator.Rotate(0, speed * Time.deltaTime, 0);
    }
}
