using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Nolet.Outline;
using UnityEditor;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D _DefaultCursor;
    [SerializeField] Texture2D _HoverInteractableCursor;
    [SerializeField] Texture2D _ClickInteractableCursor;
    [SerializeField] Texture2D _InvestigateCursor;
    [SerializeField] Camera _MainCamera;
    [SerializeField] LayerMask _InteractableLayer;

    GameObject _CurrentHoveredObject;
     void OnEnable()
    {
        SceneManager.activeSceneChanged += SetMainCamera;
    }
    void OnDisable()
    {
        SceneManager.activeSceneChanged -= SetMainCamera;
    }

    private void SetMainCamera(Scene arg0, Scene arg1)
    {
        _MainCamera = Camera.main;
    }

    void Start()
    {
        Cursor.SetCursor(_DefaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if(_MainCamera == null)
            return;

        Ray ray = _MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _InteractableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.TryGetComponent<Outline>(out Outline _Outline))
                return;
            Outline highlight = hitObject.AddComponent<Outline>();
            highlight.OutlineMode = Outline.Mode.OutlineAll;
            highlight.OutlineColor = Color.white;
            highlight.OutlineWidth = 15f;

            _CurrentHoveredObject = hitObject;
        }
        else
        {
            if(_MainCamera == null || _CurrentHoveredObject == null)
                return;
            Destroy(_CurrentHoveredObject.GetComponent<Outline>());
            _CurrentHoveredObject = null;
        }
    }
}
