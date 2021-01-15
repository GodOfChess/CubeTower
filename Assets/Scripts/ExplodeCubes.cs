using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject restartButton, explosion;
    private bool collisionSet;

   private void OnCollisionEnter(Collision collision)
    {
        // если упавший объект это кубик, и ни один кубик еще не упал на землю
        if(collision.gameObject.tag == "Cube" && !collisionSet)
        {
            // из списка AllCubes берем все кубики, начиная с конца, так как они будут удаляться из за функции SetParent
            for(int i = collision.transform.childCount-1; i>=0; i--)
            {
                //Берем каждый кубик и добавляем ему физику RigidBody, добавляем эффект взрыва после падения, и удаляем из группы AllCubes
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                //1 параметр - сила взрыва, 2 - вектор силы по y , 3 - радиус взрыва
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                child.SetParent(null);
            }
            //Включаем кнопку рестарт
            restartButton.SetActive(true);
            //После проигрыша отдаляем камеру по оси z
            Camera.main.transform.position -= new Vector3(0, 0, 3);
            //Добавляем эффект тряски из созданного класса CameraShake
            Camera.main.gameObject.AddComponent<CameraShake>();

            //Создаем эффект взрыва в точке падения первого кубика
            Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity);

            Destroy(collision.gameObject);
            collisionSet = true;
        }
    }

}
