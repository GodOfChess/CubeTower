using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;

    public GameObject allCubes , cubeEffect, restartButton;
    private Rigidbody UpdatePhysic;
    public GameObject[] CanvasStartPage;

    public GameObject[] cubesToCreate;

    private bool IsLose, firstTouch;
    private Coroutine showCubePlace;
    private float deltaY;
    private Transform mainCam;
    private float CameraSpeed = 2f;
    private int prevCountMaxHorizontal = 0;

    public Text scoreText;

    private Color toCameraColor;
    public Color[] Colors;

    private List<GameObject> possibleCubesToCreate = new List<GameObject>();

    private List<Vector3> allCubesPositions = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1)
    };

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < 5)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
        }
        else if (PlayerPrefs.GetInt("score") < 10)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
        }
        else if (PlayerPrefs.GetInt("score") < 15)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
        }
        else if (PlayerPrefs.GetInt("score") < 20)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
        }
        else if (PlayerPrefs.GetInt("score") < 25)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
        }
        else if (PlayerPrefs.GetInt("score") < 50)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
            possibleCubesToCreate.Add(cubesToCreate[5]);
        }
        else if (PlayerPrefs.GetInt("score") < 100)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
            possibleCubesToCreate.Add(cubesToCreate[5]);
            possibleCubesToCreate.Add(cubesToCreate[6]);
        }
        else if (PlayerPrefs.GetInt("score") < 150)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
            possibleCubesToCreate.Add(cubesToCreate[5]);
            possibleCubesToCreate.Add(cubesToCreate[6]);
            possibleCubesToCreate.Add(cubesToCreate[7]);
        }
        else if (PlayerPrefs.GetInt("score") < 200)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
            possibleCubesToCreate.Add(cubesToCreate[5]);
            possibleCubesToCreate.Add(cubesToCreate[6]);
            possibleCubesToCreate.Add(cubesToCreate[7]);
            possibleCubesToCreate.Add(cubesToCreate[8]);
        }
        else
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
            possibleCubesToCreate.Add(cubesToCreate[1]);
            possibleCubesToCreate.Add(cubesToCreate[2]);
            possibleCubesToCreate.Add(cubesToCreate[3]);
            possibleCubesToCreate.Add(cubesToCreate[4]);
            possibleCubesToCreate.Add(cubesToCreate[5]);
            possibleCubesToCreate.Add(cubesToCreate[6]);
            possibleCubesToCreate.Add(cubesToCreate[7]);
            possibleCubesToCreate.Add(cubesToCreate[8]);
            possibleCubesToCreate.Add(cubesToCreate[9]);
        }

        //Устанавливаем значение заднего фона, первоначальному значению из юнити
        toCameraColor = Camera.main.backgroundColor;
        // получаем изначальную позицию камеры
        mainCam = Camera.main.transform;
        //устаналиваем изначальное значение deltaY для последующего использования в движении камеры вверх
        deltaY = 5.3f + nowCube.y - 1f;
        
        //Устанавливаем изначальное значение лучшего счета
        scoreText.text = " Best : " + PlayerPrefs.GetInt("score") + " " + "\nNow : 0 ";

        //Все кубы получают физику
        UpdatePhysic = allCubes.GetComponent<Rigidbody>();

        showCubePlace = StartCoroutine(ShowCubePlace());
    }  

    private void Update()
    {   
        //Если нажата левая кнопка мыши, или пальцев на экране больше чем 1, и есть хотя бы один кубик на экране
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace !=null && allCubes !=null && !EventSystem.current.IsPointerOverGameObject())
        {
            //Если запускаем не в юнити
#if !UNITY_EDITOR
            if(Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif
            // Пока не произойдет нажатия не на кнопку, игра не начнется. Если нажали не на кнопку, уничтожаем кнопки в списке который передаем в юнити
            if (!firstTouch)
            {
                firstTouch = true;
                foreach (GameObject obj in CanvasStartPage)
                    Destroy(obj);
            }

            GameObject createCube = null;
            if (possibleCubesToCreate.Count == 1)
            {
                createCube = possibleCubesToCreate[0];
            }
            else
                createCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];

            // Создаем объект на основе первоначального кубика, на изначальном месте, которые мы задаем
            GameObject newCube = Instantiate(createCube, cubeToPlace.position, Quaternion.identity) as GameObject;

            // Пихаем этот кубик в AllCubes
            newCube.transform.SetParent(allCubes.transform);
            // Получаем вектор, где этот кубик поставился
            nowCube.setVector(cubeToPlace.position);
            // Пихаем в занятые позиции этот вектор
            allCubesPositions.Add(nowCube.getVector());

            //Создаем эффект в том месте где ставим куб
            Instantiate(cubeEffect, newCube.transform.position, Quaternion.identity);

            //Обновляем физику кубиков
            UpdatePhysic.isKinematic = true;
            UpdatePhysic.isKinematic = false;

            // Показываем возможные позиции для куба, меняем цвет бекграунда
            SpawnPositions();
            MoveCameraChangeBg();
        }

        // Если еще не проиграли, и вся башня из кубиков пошатнулась
        if(!IsLose && UpdatePhysic.velocity.magnitude > 0.1f)
        {
            //Уничтожаем кубик-позицию(синий), и говорим что произошел луз. Останаливаем курутину.
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);
        }

        //Поднимаем камеру вверх или вниз, в зависимости от того куда поставился куб.
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, deltaY, mainCam.localPosition.z), CameraSpeed * Time.deltaTime) ;
        // Если надо поменять цвет бекграунда
        if (Camera.main.backgroundColor != toCameraColor)
        {
            // Меняем цвет плавно
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }
    }

    //Куратина
    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            //Все время спавним позиции для синего кубика
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        //Добавляем в список все возможные позиции для синего кубика
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x){
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        }

        //Если хоть одна позиция есть, то показываем рандомную
        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        // Если некуда поставить то луз
        else if (positions.Count == 0)
        {
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
            restartButton.SetActive(true);
            StopCoroutine(showCubePlace);
        }
        // Если одна позиция, то взять из списка первое значение
        else
            cubeToPlace.position = positions[0];

    }

    private bool IsPositionEmpty(Vector3 pos)
    {
        // если хотим поставить кубик ниже платформы то запрещаем
        if (pos.y == 0) return false;
        foreach(Vector3 CubePosition in allCubesPositions)
        {
            if (CubePosition.x == pos.x && CubePosition.y == pos.y && CubePosition.z == pos.z)
            {
                return false;
            }
        }
        return true;
    }

    private void MoveCameraChangeBg()
    {
        // задаем изначальные значения
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;
        int maxHor;

        foreach(Vector3 pos in allCubesPositions)
        {
            //Ищем максимальное значение по x,y,z из всех занятых позиций
            if(Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
            {
                maxX = Convert.ToInt32(pos.x);
            }

            if (Convert.ToInt32(pos.y) > maxY)
            {
                maxY = Convert.ToInt32(pos.y);
            }

            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
            {
                maxZ = Convert.ToInt32(pos.z);
            }
        }

        //Если рекорд больше чем Best, то устанавливаем новое значение
        if (PlayerPrefs.GetInt("score") < maxY-1)
        {
            PlayerPrefs.SetInt("score", maxY-1);
        }

        scoreText.text = " Best : " + PlayerPrefs.GetInt("score") + " " + "\nNow:  " + Convert.ToInt32(maxY-1) + " ";

        // Обновляем разницу с y
        deltaY = 5.3f + nowCube.y - 1f;
        //Ищем максимальное горизонтальное значение
        if (maxX > maxZ)
        {
            maxHor = maxX;
        }
        else
        {
            maxHor = maxZ;
        }
        
        // Каждый второй блок поставленный по горизонтали будем отдалять по оси z
        if (maxX %2 == 0 && prevCountMaxHorizontal != maxHor)
        {
            mainCam.localPosition += new Vector3(0, 0, -2.5f);
            prevCountMaxHorizontal = maxHor;
        }

        // если максимальное значение по y достигает определенных значений, то меняем цвет бекграунда из списка, который передаем в юнити
        if( maxY >= 7)
        {
            toCameraColor = Colors[2];
        }
        else if ( maxY >= 5)
        {
            toCameraColor = Colors[1];
        }
        else if (maxY >= 2)
        {
            toCameraColor = Colors[0];
        }
    }
}

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }

    public void setVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}