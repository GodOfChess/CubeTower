using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDur = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;

    private Vector3 originPos;

    private void Start()
    {
        //Получаем оригинальную позицию камеры
        camTransform = GetComponent<Transform>();
        originPos = camTransform.localPosition;
    }

    private void Update()
    {
        //Если у нас длительность тряски еще не упала ниже нуля, то мы будем перемещать камеру на рандомную точку из сферы радиусом 1
        if (shakeDur > 0)
        {
            camTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            //После тряски возвращаем камеру на оригинальную позицию
            shakeDur = 0;
            camTransform.localPosition = originPos;
        }
    }
}
