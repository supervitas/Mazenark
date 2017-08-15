using UnityEngine;

namespace Ui.Drag {
    /// <summary>
    /// Example of control unit for drag and drop events handle
    /// </summary>
    public class DummyControlUnit : MonoBehaviour
    {
        void OnItemPlace(DragAndDropCell.DropDescriptor desc)
        {
            DummyControlUnit sourceSheet = desc.sourceCell.GetComponentInParent<DummyControlUnit>();
            DummyControlUnit destinationSheet = desc.destinationCell.GetComponentInParent<DummyControlUnit>();

            if (sourceSheet != destinationSheet) {
                destinationSheet.gameObject.SendMessageUpwards("OnSheetChange", desc, SendMessageOptions.DontRequireReceiver);
//                sourceSheet.gameObject.SendMessageUpwards("OnSheetChange", desc, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
