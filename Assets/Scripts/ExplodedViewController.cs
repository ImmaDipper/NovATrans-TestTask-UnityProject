using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для компонентов нашего редуктора, внутри него запоминаем исходную позицию компонента и 
// позицию компонента при разлете, а так же сам компонент 
public class SubMeshes
{
    public MeshRenderer meshRenderer;
    public Vector3 originalPosition;
    public Vector3 explodedPosition;
}

public class ExplodedViewController : MonoBehaviour
{
    // Переменная, чтобы вызвать метод FocusOn() из скрипта CameraController на главную деталь
    // после нажатия кнопки главной детали и окончания процесса плавного слета/разлета компонентов
    public GameObject cameraControllerScript;
    // Переменная для хранения объекта нашей главной детали, для последующего ее использования методом
    // FocusOn()
    public GameObject planetarnyReductor;
    // Список объектов класса SubMeshes с нашими компонентами их исходными позициями и позициями при
    // разлете
    private List<SubMeshes> childMeshRenderers;
    // Булевая переменная для определения фазы слета/разлета компонентов
    private bool isInExplodedView = false;
    // Булевая переменная для определенния фазы движения компонентов при слете/разлете
    private bool isMoving = false;
    // Переменная скорости слета/разлета компонентов
    private float explosionSpeed = 5.0f;

    public void Awake()
    {
        childMeshRenderers = new List<SubMeshes>();

        // В данном цикле мы достаем дочерние объекты класса MeshRenderer нашей главной детали для
        // определения им начальной позиции и позиция при разлете и добавляем в объявленный выше список
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            SubMeshes mesh = new SubMeshes();
            mesh.meshRenderer = item;
            mesh.originalPosition = item.transform.position;
            switch(item.name)
            {
                case "Sun gear":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, -0.25f, 0);
                    break;
                case "Planetary Gears":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, 0.75f, 0);
                    break;
                case "Crown gear":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, 1.5f, 0);
                    break;
                case "Planetary Carrier #2":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, 2.25f, 0);
                    break;
                case "Cap #1":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, 3f, 0);
                    break;
                case "Axis":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, 3.75f, 0);
                    break;
                case "Sleeve":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, -1f, 0);
                    break;
                case "Fasteners":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, -2.5f, 0);
                    break;
                case "Planetary Carrier #1":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, -3.25f, 0);
                    break;
                case "Cap #2":
                    mesh.explodedPosition = item.transform.position + new Vector3(0, -4f, 0);
                    break;
            }
            childMeshRenderers.Add(mesh);
        }
    }

    private void Update()
    {
        // В следующих if-ах определяем фазу движения и слета/разлета, чтобы совершить анимированный
        // слет/разлет компонентов
        if(isMoving)
        {
            if(isInExplodedView)
            {
                foreach (var item in childMeshRenderers)
                {
                    item.meshRenderer.transform.position = Vector3.Lerp(item.meshRenderer.transform.position, item.explodedPosition, explosionSpeed * Time.deltaTime);
                    if(Vector3.Distance(item.meshRenderer.transform.position, item.explodedPosition) < 0.001f)
                    {
                        isMoving = false;
                        cameraControllerScript.GetComponent<CameraController>().FocusOn(planetarnyReductor);
                    }
                }
            }
            else
            {
                foreach (var item in childMeshRenderers)
                {
                    item.meshRenderer.transform.position = Vector3.Lerp(item.meshRenderer.transform.position, item.originalPosition, explosionSpeed * Time.deltaTime);
                    if(Vector3.Distance(item.meshRenderer.transform.position, item.originalPosition) < 0.00001f)
                    {
                        isMoving = false;
                        cameraControllerScript.GetComponent<CameraController>().FocusOn(planetarnyReductor);
                    }
                }
            }
        }
    }

    // При нажатии на название планетарного редуктора активируется этот метод и просходит
    // разлет/слет компонентов редуктора
    public void ToggleExplodedView()
    {
        if(isInExplodedView)
        {
            isInExplodedView = false;
            isMoving = true;
        }
        else
        {
            isInExplodedView = true;
            isMoving = true;
        }
    }
}
