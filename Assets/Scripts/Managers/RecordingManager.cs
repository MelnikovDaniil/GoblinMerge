using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    public Texture2D idleHand;
    public Texture2D clickHand;
    public Texture2D dragHand;
    public SpriteRenderer handRenderer;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(idleHand, Vector2.zero, CursorMode.ForceSoftware);
        //handRenderer.sprite = idleHand;
    }

    // Update is called once per frame
    void Update()
    {
        //handRenderer.transform.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.SetCursor(clickHand, Vector2.zero, CursorMode.ForceSoftware);
        }

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Cursor.SetCursor(dragHand, Vector2.zero, CursorMode.ForceSoftware);
        //}

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Cursor.SetCursor(idleHand, Vector2.zero, CursorMode.ForceSoftware);
        }

    }
}
