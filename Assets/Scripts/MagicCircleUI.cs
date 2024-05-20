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
    private HashSet<int> selectedPointsSet = new HashSet<int>(); // 이미 선택된 포인트를 저장하는 집합
    private SpellbookSystem spellbookSystem;

    private Color defaultColor = new Color(1f, 1f, 1f, 0.6f); // 기본 색상, 60% 투명도

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
                point.GetComponent<Image>().color = defaultColor; // 모든 버튼의 기본 색 설정
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
                ResetPattern(); // 패턴 초기화
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
                    points[i].GetComponent<Image>().color = GetSelectedColor(); // 선택된 색으로 변경
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
        selectedColor.a = 0.6f; // 투명도 60%
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
            Debug.Log("원거리 마법 발동..");
            spellbookSystem.CastSpellLong();
            CloseMagicCircleUI(); // 패턴이 맞으면 UI를 닫습니다.
        }
        else
        {
            Debug.Log("잘못된 패턴 입력");
            CloseMagicCircleUI(); // 잘못된 패턴일 때도 UI를 닫습니다.
        }
    }

    private void CloseMagicCircleUI()
    {
        isDrawing = false;
        if (magicCircleCanvas != null)
        {
            magicCircleCanvas.SetActive(false);
        }
        ResetPattern();
    }

    private void ResetPattern()
    {
        selectedPattern.Clear();
        selectedPointsSet.Clear();
        foreach (var point in points)
        {
            point.GetComponent<Image>().color = defaultColor; // 모든 버튼의 색을 기본 색으로 변경
        }
    }



    public bool IsDrawing()
    {
        return isDrawing;
    }
}