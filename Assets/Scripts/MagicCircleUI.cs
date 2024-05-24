using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCircleUI : MonoBehaviour
{
    public GameObject magicCircleCanvas;
    public List<Button> points;

    private List<int> selectedPattern = new List<int>();
    private bool isDrawing = false;
    private bool validPattern = false;
    private bool longPattern = false;
    private bool buffPattern = false;
    private bool shortPattern = false;


    private HashSet<int> selectedPointsSet = new HashSet<int>();
    private SpellbookSystem spellbookSystem;

    private Color defaultColor = new Color(1f, 1f, 1f, 0.6f);

    void Start()
    {
        spellbookSystem = GetComponentInParent<SpellbookSystem>();

        if (spellbookSystem == null)
        {
            Debug.LogError("SpellbookSystem not found on parent object.");
        }

        if (magicCircleCanvas != null)
        {
            magicCircleCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("MagicCircleCanvas is not assigned in the inspector.");
        }

        foreach (var point in points)
        {
            if (point == null)
            {
                Debug.LogError("One of the points is not assigned in the inspector.");
            }
            else
            {
                point.GetComponent<Image>().color = defaultColor;
            }
        }
    }

    void Update()
    {
        if (validPattern)
        {
            if (longPattern && Input.GetMouseButtonDown(0))
            {
                Debug.Log("CastSpellLong 함수 실행됨");
                spellbookSystem.CastSpellLong();
                longPattern = false;
                validPattern = false; // 스킬 발동 후 유효한 패턴 플래그 초기화
            }
            else if (buffPattern)
            {
                spellbookSystem.CastSpellBuff();
                buffPattern = false;
                validPattern = false; // 스킬 발동 후 유효한 패턴 플래그 초기화
            }
            else if (shortPattern && Input.GetMouseButtonDown(0))
            {
                spellbookSystem.CastSpellShort();
                shortPattern = false;
                validPattern = false; // 스킬 발동 후 유효한 패턴 플래그 초기화
            }
        }
    }

    public void HandleMagicCircle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDrawing = !isDrawing;
            if (magicCircleCanvas != null)
            {
                magicCircleCanvas.SetActive(isDrawing);
            }

            if (!isDrawing)
            {
                ValidatePattern();
                ResetPattern();
            }
        }

        if (isDrawing && Input.GetMouseButtonDown(0))
        {
            ResetPattern();
        }

        if (isDrawing && Input.GetMouseButton(0))
        {
            CheckMouseOverPoints();
        }
    }

    private void CheckMouseOverPoints()
    {
        Vector2 localMousePosition = magicCircleCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        for (int i = 0; i < points.Count; i++)
        {
            RectTransform rectTransform = points[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
            {
                if (!selectedPointsSet.Contains(i))
                {
                    selectedPointsSet.Add(i);
                    selectedPattern.Add(i);
                    points[i].GetComponent<Image>().color = GetSelectedColor();
                    Debug.Log("Point " + i + " selected");
                }
            }
        }
    }

    private Color GetSelectedColor()
    {
        Color selectedColor = Color.white;
        switch (spellbookSystem.currentSpellbook)
        {
            case SpellbookType.Attack:
                selectedColor = Color.red;
                break;
            case SpellbookType.Health:
                selectedColor = Color.green;
                break;
            case SpellbookType.Speed:
                selectedColor = Color.blue;
                break;
        }
        selectedColor.a = 0.6f;
        return selectedColor;
    }

    public void UpdateSelectedPointsColor()
    {
        foreach (int index in selectedPointsSet)
        {
            points[index].GetComponent<Image>().color = GetSelectedColor();
        }
    }

    private void ValidatePattern()
    {
        Debug.Log("Selected Pattern: " + string.Join(", ", selectedPattern));

        if (selectedPattern.Count == 3 && selectedPattern[0] == 1 && selectedPattern[1] == 4 && selectedPattern[2] == 7)
        {
            Debug.Log("원거리 마법 패턴.");
            validPattern = true;
            longPattern = true;
        }
        else if (selectedPattern.Count == 3 && selectedPattern[0] == 7 && selectedPattern[1] == 4 && selectedPattern[2] == 1)
        {
            Debug.Log("버프 마법 패턴.");
            validPattern = true;
            buffPattern = true;
        }
        else if (selectedPattern.Count == 3 && selectedPattern[0] == 3 && selectedPattern[1] == 4 && selectedPattern[2] == 5)
        {
            Debug.Log("근접 마법 패턴.");
            validPattern = true;
            shortPattern = true;
        }
        else
        {
            Debug.Log("잘못된 패턴 입력");
        }

        CloseMagicCircleUI();
    }

    private void CloseMagicCircleUI()
    {
        isDrawing = false;
        if (magicCircleCanvas != null)
        {
            magicCircleCanvas.SetActive(false);
        }
    }

    private void ResetPattern()
    {
        selectedPattern.Clear();
        selectedPointsSet.Clear();
        foreach (var point in points)
        {
            point.GetComponent<Image>().color = defaultColor;
        }
    }

    public bool IsDrawing()
    {
        return isDrawing;
    }
}
