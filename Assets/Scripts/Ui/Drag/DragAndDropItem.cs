using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Drag {
    /// <summary>
    /// Every "drag and drop" item must contain this script
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static DragAndDropItem draggedItem;                                      // Item that is dragged now
        public static GameObject icon;                                                  // Icon of dragged item
        public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

        public delegate void DragEvent(DragAndDropItem item);
        public static event DragEvent OnItemDragStartEvent;                             // Drag start event
        public static event DragEvent OnItemDragEndEvent;                               // Drag end event

        /// <summary>
        /// This item is dragged
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            sourceCell = GetComponentInParent<DragAndDropCell>();                       // Remember source cell
            draggedItem = this;                                                         // Set as dragged item
            icon = new GameObject("Icon");                                              // Create object for item's icon
            Image image = icon.AddComponent<Image>();
            image.sprite = GetComponent<Image>().sprite;
            image.raycastTarget = false;                                                // Disable icon's raycast for correct drop handling
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            // Set icon's dimensions
            iconRect.sizeDelta = new Vector2(   GetComponent<RectTransform>().sizeDelta.x,
                GetComponent<RectTransform>().sizeDelta.y);
            Canvas canvas = GetComponentInParent<Canvas>();                             // Get parent canvas
            if (canvas != null)
            {
                // Display on top of all GUI (in parent canvas)
                icon.transform.SetParent(canvas.transform, true);                       // Set canvas as parent
                icon.transform.SetAsLastSibling();                                      // Set as last child in canvas transform
            }
            OnItemDragStartEvent?.Invoke(this);                                             // Notify all about item drag start
        }

        /// <summary>
        /// Every frame on this item drag
        /// </summary>
        /// <param name="data"></param>
        public void OnDrag(PointerEventData data)
        {
            if (icon != null)
            {
                icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor
            }
        }

        /// <summary>
        /// This item is dropped
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (icon != null)
            {
                Destroy(icon);                                                          // Destroy icon on item drop
            }
            MakeVisible(true);                                                          // Make item visible in cell
            OnItemDragEndEvent?.Invoke(this);                                               // Notify all cells about item drag end
            draggedItem = null;
            icon = null;
            sourceCell = null;
        }

        /// <summary>
        /// Enable item's raycast
        /// </summary>
        /// <param name="condition"> true - enable, false - disable </param>
        public void MakeRaycast(bool condition)
        {
            Image image = GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = condition;
            }
        }

        /// <summary>
        /// Enable item's visibility
        /// </summary>
        /// <param name="condition"> true - enable, false - disable </param>
        public void MakeVisible(bool condition)
        {
            GetComponent<Image>().enabled = condition;
        }
    }
}
