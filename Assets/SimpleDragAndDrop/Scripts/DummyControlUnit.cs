using UnityEngine;
using System.Collections;

/// <summary>
/// Example of control unit for drag and drop events handle
/// </summary>
public class DummyControlUnit : MonoBehaviour
{
    void OnItemPlace(DragAndDropCell.DropDescriptor desc)
    {
        DummyControlUnit sourceSheet = desc.sourceCell.GetComponentInParent<DummyControlUnit>();
        DummyControlUnit destinationSheet = desc.destinationCell.GetComponentInParent<DummyControlUnit>();
        // If item dropped between different sheets
        if (destinationSheet != sourceSheet)
        {
            Debug.Log(desc.item.name + " is dropped from " + sourceSheet.name + " to " + destinationSheet.name);
        }
    }
}
