using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour
{
    public Transform targetObject; // Inspector'dan atanacak obje
    private bool isHidden = false; // Objenin gizli olup olmadığını takip etmek için

    void Update()
    {
        // F tuşuna basılıp basılmadığını kontrol et
        if (Input.GetKeyDown(KeyCode.F) && !isHidden)
        {
            HideObject();
        }
    }

    void HideObject()
    {
        // Eğer hedef obje varsa
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Objeyi gizle
                renderer.enabled = false;
                isHidden = true;
                Debug.Log("Obje gizlendi ve bir daha görünmeyecek.");
            }
            else
            {
                Debug.LogWarning("Hedef objede Renderer bileşeni bulunamadı!");
            }
        }
        else
        {
            Debug.LogWarning("Hedef obje atanmamış!");
        }
    }
}