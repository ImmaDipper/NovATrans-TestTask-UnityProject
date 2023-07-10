using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Класс для кнопок компонентов нашего редуктора, внутри него запоминаем исходную позицию кнопки  
// компонента и позицию кнопки компонента при разлете, а так же саму кнопку компонента 
public class ComponentButtons
{
    public Button button;
    public Vector2 originalPosition;
    public Vector2 explodedPosition;
}

public class MainSceneMenuController : MonoBehaviour
{
    // Список объектов класса ComponentButtons с нашими кнопками компонентами их исходными позициями
    // и позициями при разлете
    private List<ComponentButtons> componentButtonList;
    // Булевая переменная для определения фазы слета/разлета кнопок компонентов
    private bool isInExplodedView = false;
    // Булевая переменная для определенния фазы движения кнопок компонентов при слете/разлете
    private bool isMoving = false;
    // Переменная скорости слета/разлета кнопок компонентов
    private float explosionSpeed = 10.0f;

    void Awake()
    {
        componentButtonList = new List<ComponentButtons>();

        // В данном цикле мы достаем дочерние объекты класса RectTransform наших кнопок для
        // определения им начальной позиции и позиция при разлете и добавляем в объявленный выше список
        foreach (var item in GetComponentsInChildren<Button>())
        {
            ComponentButtons componentButton = new ComponentButtons();
            RectTransform rectTransform = item.GetComponent<RectTransform>();

            componentButton.button = item;
            componentButton.originalPosition = rectTransform.anchoredPosition;

            switch(item.name)
            {
                case "SunGearButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -720.0f);         
                    break;
                case "PlanetaryGearsButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -580.0f);                     
                    break;
                case "CrownGearButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -300.0f);                    
                    break;
                case "PlanetaryCarrier2Button":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -510.0f);                    
                    break;
                case "Cap1Button":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -160.0f);                    
                    break;
                case "AxisButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -90.0f);                    
                    break;
                case "SleeveButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -650.0f);                    
                    break;
                case "FastenersButton":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -370.0f);                    
                    break;
                case "PlanetaryCarrier1Button":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -440.0f);                    
                    break;
                case "Cap2Button":
                    componentButton.explodedPosition = componentButton.originalPosition + new Vector2(0, -230.0f);                    
                    break;
            } 
            componentButtonList.Add(componentButton);
        }
    }

    void Update()
    {
        // В следующих if-ах определяем фазу движения и слета/разлета, чтобы совершить анимированный
        // слет/разлет кнопок компонентов
        if(isMoving)
        {
            if(isInExplodedView)
            {
                foreach (var item in componentButtonList)
                {
                    item.button.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(item.button.GetComponent<RectTransform>().anchoredPosition, item.explodedPosition, explosionSpeed * Time.deltaTime);
                    if(Vector3.Distance(item.button.GetComponent<RectTransform>().anchoredPosition, item.explodedPosition) < 0.1f)
                    {
                        isMoving = false;
                    }
                }
            }
            else
            {
                foreach (var item in componentButtonList)
                {
                    item.button.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(item.button.GetComponent<RectTransform>().anchoredPosition, item.originalPosition, explosionSpeed * Time.deltaTime);
                    if(Vector3.Distance(item.button.GetComponent<RectTransform>().anchoredPosition, item.originalPosition) < 0.1f)
                    {
                        isMoving = false;
                    }
                }
            }
        }
    }

    // При нажатии на название планетарного редуктора активируется этот метод и просходит
    // разлет/слет списка названий компонентов редуктора
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
