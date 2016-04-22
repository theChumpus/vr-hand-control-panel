using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

public class TouchDetector : MonoBehaviour {

	public Color highlightedColour;
	public float touchThreshold;
	private Color normalColour;
	private UnityEngine.UI.Image button;
	private Vector3 startPosition;
	private bool startPositionSet = false;

	LeapServiceProvider provider;

	// Use this for initialization
	void Start () {
		provider = FindObjectOfType<LeapServiceProvider> () as LeapServiceProvider;
		button = GetComponent<UnityEngine.UI.Image> ();
		normalColour = button.color;
	}

	// Update is called once per frame
	void Update () {
		Hand hand = getRightHand (provider.CurrentFrame.Hands);
		if (hand != null) {
			Finger indexFinger = hand.Fingers [1];
			float distanceToFingerTip = Vector3.Distance (indexFinger.TipPosition.ToVector3 (), transform.position);
			bool isTouching = distanceToFingerTip < touchThreshold;
			if (isTouching && indexFinger.IsExtended) {
				setStartPosition (transform.position);
				button.color = highlightedColour;
				transform.position = indexFinger.TipPosition.ToVector3 ();
				if(Vector3.Distance(startPosition, transform.position) > 0.1f) {
					print("should release");
					transform.position = startPosition;
					startPositionSet = false;
				}
			} else {
				button.color = normalColour;
				if (startPositionSet) {
					transform.position = startPosition;
					startPositionSet = false;
				}
			}
		}
	}

	void setStartPosition(Vector3 position) {
		if (!startPositionSet) {
			startPosition = position;
			startPositionSet = true;
		}
	}

	Hand getRightHand(IList hands) {
		foreach (Hand hand in hands) {
			if (hand.IsRight) {
				return hand;
			}
		}
		return null;
	}
}
