using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Здесь мы храним нашу главную деталь Planetarny reductor
    public GameObject planetarnyReductor;
    // В данной переменной храним объект, на котором сфокусирована камера после применения
    // метода FocunOn()
    private GameObject focusedObject;
    // Список для компонентов главной детали, чтобы в процессе применения метода FocusOn()
    // делать невидимыми все остальные компоненты, кроме того, на котором сфокусированы
    public List<GameObject> components;
    // Булевая переменная для определения, кликает ли юзер на кнопку второй раз, чтобы совершить
    // обратное действие от приближения к компоненту
    private bool componentDoubleClicked;

    // В данную переменную записываем расстояние от камеры до объекта фокусировки, чтобы
    // он полностью входил в экран в формате
    private Vector3 distanceFromObjectToFitIt;
    // Z-координата того же расстояния для последующего ее использования в орбитальном осмотре
    private float zDistanceFromObjectToFitIt;
    // Переменная для хранения границ объекта, с помощью которой мы рассчитываем расстояние, на
    // котором объект будет полностью входить в экран
    private Bounds boundsOfObjectToFit;
    // Переменная для хранения позиции камеры, которую мы используем при орбитальном осмотре в
    // методе Update()
    private Vector3 previousCamPosition;

    // Переменная скорости анимированного приближения камеры к объекту фокусировки
    private float camSpeed = 10.0f;
    // Переменная для определения находится ли камера в процессе анимированного подлета/отлета от
    // объекта фокусировки или нет, чтобы адекватно обрабатывать процессы подлета и орбитального осмотра
    private bool camIsMoving = false;

    void Awake()
    {   // При подгрузке камеры на сцену фокусируемся на главную деталь
        FocusOn(planetarnyReductor);
        
        // Создаем и заполняем список компонентов главной детали
        components = new List<GameObject>();
        for(int i = 0; i < planetarnyReductor.transform.childCount; i++)
        {   
            components.Add(planetarnyReductor.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {    
        // Реализация анимированного подлета с помощью метода MoveTowards()
        if(camIsMoving)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, distanceFromObjectToFitIt, camSpeed * Time.deltaTime);
            if(Vector3.Distance(Camera.main.transform.position, distanceFromObjectToFitIt) < 0.001f)
            {
                camIsMoving = false;
            }
        }
        else
        {
            // В двух if-ах реализован орбитальный осмотр объектов, когда камера завершила свой 
            // анимированный подлет/отлет
            if(Input.GetMouseButtonDown(2))
            {
                previousCamPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if(Input.GetMouseButton(2))
            {
                Vector3 direction = previousCamPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Camera.main.transform.position = boundsOfObjectToFit.center;
                Camera.main.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                Camera.main.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
                Camera.main.transform.Translate(new Vector3(0, 0, -zDistanceFromObjectToFitIt));

                previousCamPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        }  
    }

    // Вспомогательная функция для FocusOn(), которая возвращает границы переданного объекта
    public Bounds GetBoundsWithChildren(GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();

        for(int i = 1; i < renderers.Length; i++)
        {
            if(renderers[i].enabled)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }

        return bounds;
    }

    // Функция для вмещения объекта в рамки экрана
    public void FocusOn(GameObject gameObject)
    {
        if(focusedObject != gameObject || gameObject == planetarnyReductor)
        {
            componentDoubleClicked = false;
            // Скрываем все детали кроме той, на которую сфокусированы
            foreach (var component in components)
            {
                if(gameObject.name != component.name && gameObject.name != planetarnyReductor.name)
                {
                    component.SetActive(false);
                }
                else if(gameObject.name == planetarnyReductor.name)
                {
                    component.SetActive(true);
                }
            }
            gameObject.SetActive(true);

            // Запоминаем в переменной, чтобы использовать для орбитального осмотра объекта
            boundsOfObjectToFit = GetBoundsWithChildren(gameObject);
            
            // Математические вычисления оптимального росстояния от объекта фокуса до камеры, 
            // чтобы объект полностью вмещался в экран
            float cameraDistance = 2.0f;
            Vector3 objectSizes = boundsOfObjectToFit.max - boundsOfObjectToFit.min;
            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView);
            zDistanceFromObjectToFitIt = cameraDistance * objectSize / cameraView;
            zDistanceFromObjectToFitIt += 0.5f * objectSize;
            distanceFromObjectToFitIt = boundsOfObjectToFit.center - zDistanceFromObjectToFitIt * Camera.main.transform.forward;

            // Позволяем совершить анимированный подлет к объекту фокусировки после рассчета
            // оптимального расстояние до камеры
            camIsMoving = true;
        }
        else
        {
            FocusOn(planetarnyReductor);
            componentDoubleClicked = true;
        }

        if(componentDoubleClicked)
        {
            focusedObject = planetarnyReductor;  
        }
        else
        {
            focusedObject = gameObject;
        }
    }
}
