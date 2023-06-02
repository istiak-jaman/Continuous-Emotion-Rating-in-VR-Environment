using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler {

	//[HideInInspector]
	public Transform parentToReturnTo = null;
	//[HideInInspector]
	public RectTransform placeHolderParent = null;

	private GameObject placeHolder = null;

	private Vector2 pointOffset;
	
	private Canvas canvas;
	
	void Start() {
		canvas = GetComponent<Image>().canvas;
	}
	
	public void OnInitializePotentialDrag(PointerEventData eventData) {
		eventData.useDragThreshold = false;
	}
	
	public void OnBeginDrag(PointerEventData eventData) {
		placeHolder = new GameObject();
		placeHolder.transform.SetParent( this.transform.parent );
		LayoutElement le = placeHolder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex() );

		parentToReturnTo = this.transform.parent;
		SetPlaceholderParent(parentToReturnTo);
		GetComponent<LayoutElement>().ignoreLayout = true;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		
		Vector2 point;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(placeHolderParent, eventData.position, eventData.pressEventCamera, out point);
		lastPoint = point;
		pointOffset = point - new Vector2(transform.localPosition.x, transform.localPosition.y);
		
		//point -= pointOffset;
		//transform.position = placeHolderParent.TransformPoint(point.x, point.y, depthOffset);
		
		transform.SetParent(canvas.transform, true);
	}
	
	// Adjust to taste
	float depthOffset = -.01f;
	
	Vector2 lastPoint;
	public void OnDrag(PointerEventData eventData) {
		Vector2 point;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(placeHolderParent, eventData.position, eventData.pressEventCamera, out point);
		lastPoint = point;
		point -= pointOffset;
		transform.position = placeHolderParent.TransformPoint(point.x, point.y, depthOffset);
		
		if (placeHolder.transform.parent != placeHolderParent) {
			placeHolder.transform.SetParent(placeHolderParent);
		}
		
		int newSiblingIndex = placeHolderParent.childCount;

		Vector3 refPosition = placeHolderParent.InverseTransformPoint(transform.position);
		for (int i = 0; i < placeHolderParent.childCount; i++) {
			if (refPosition.x < placeHolderParent.GetChild(i).localPosition.x) {
				newSiblingIndex = i;

				if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;
				
				break;
			}
		}

		placeHolder.transform.SetSiblingIndex(newSiblingIndex);

		Debug.Log("Dragging with position " + eventData.position);
	}
	public void SetPlaceholderParent(Transform t) {
		placeHolderParent = t.GetComponent<RectTransform>();
	}

	public void OnEndDrag(PointerEventData eventData) {
		transform.position = placeHolderParent.TransformPoint(lastPoint.x, lastPoint.y, 0);
		
		GetComponent<LayoutElement>().ignoreLayout = false;
		
		//transform.SetParent(parentToReturnTo);
		transform.SetParent(placeHolderParent);
		transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		Destroy(placeHolder);
	}
}
