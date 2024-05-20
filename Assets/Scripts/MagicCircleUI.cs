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
    private bool validPattern = false; // ��ȿ�� ���� ����
    private HashSet<int> selectedPointsSet = new HashSet<int>(); // �̹� ���õ� ����Ʈ�� �����ϴ� ����
    private SpellbookSystem spellbookSystem;

    private Color defaultColor = new Color(1f, 1f, 1f, 0.6f); // �⺻ ����, 60% ����

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
                point.GetComponent<Image>().color = defaultColor; // ��� ��ư�� �⺻ �� ����
            }
        }
    }

    void Update()
    {
        if (validPattern && Input.GetMouseButtonDown(0))
        {
            spellbookSystem.CastSpellLong();
            validPattern = false; // ��ų �ߵ� �� ��ȿ�� ���� �÷��� �ʱ�ȭ
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
                ResetPattern(); // ���� �ʱ�ȭ
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
                    points[i].GetComponent<Image>().color = GetSelectedColor(); // ���õ� ������ ����
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
        selectedColor.a = 0.6f; // ���� 60%
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
            Debug.Log("��ȿ�� ����.");
            validPattern = true; // ��ȿ�� ���� �÷��� ����
        }
        else
        {
            Debug.Log("�߸��� ���� �Է�");
        }

        CloseMagicCircleUI(); // ������ UI �ݱ�
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
            point.GetComponent<Image>().color = defaultColor; // ��� ��ư�� ���� �⺻ ������ ����
        }
    }

    public bool IsDrawing()
    {
        return isDrawing;
    }
}
