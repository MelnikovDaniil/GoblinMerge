using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClickAndDragWithRigidbody : MonoBehaviour
{
    public static ClickAndDragWithRigidbody Instance;
    public float mergeDetectionRadius = 1f;

    public List<Sprite> unitSprites;

    private DragableObject selectedObject;
    private Vector3 offset;
    private Vector3 mousePosition;

    private int unitLayerMask;

    private void Awake()
    {
        Instance = this;
        unitLayerMask = LayerMask.GetMask("Unit");
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition, unitLayerMask);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<DragableObject>();
                offset = selectedObject.transform.position - mousePosition;
                selectedObject.Drag();
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            var colliders = Physics2D.OverlapCircleAll(mousePosition, mergeDetectionRadius, unitLayerMask);
            var unit = colliders.FirstOrDefault(x => 
                    x.gameObject != selectedObject.gameObject
                    && x.GetComponent(selectedObject.GetType()))?
                .GetComponent<Unit>();
            var selectedUnit = selectedObject.GetComponent<Unit>();
            if (unit != null && unit.IsMatch(selectedUnit))
            {
                unit.Merge();
                selectedUnit.Disable();
            }
            else
            {
                var crystal = Physics2D.OverlapPoint(mousePosition, LayerMask.GetMask("Crystal"));
                if (crystal != null)
                {
                    var radius = ((CircleCollider2D)crystal).radius;
                    selectedObject.transform.position = crystal.transform.position + 
                        selectedObject.transform.position.normalized * selectedObject.transform.localScale.x * radius;
                }
                selectedObject.Drop();
            }
            selectedObject = null;
        }
    }

    void FixedUpdate()
    {
        if (selectedObject)
        {
            selectedObject.Move(mousePosition + offset);
        }
    }
}