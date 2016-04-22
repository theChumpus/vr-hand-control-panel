using UnityEngine;
using System.Collections;
using Leap.Unity;
using Leap;

public class HUDController : MonoBehaviour {

	public Transform hudPrefab;
	public Vector3 hudRelativePosition;

	LeapServiceProvider provider;
	Transform hud = null;

	// Use this for initialization
	void Start () {
		provider = FindObjectOfType<LeapServiceProvider> () as LeapServiceProvider;
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands) {
			if (hand.IsLeft) {
				if (isPalmUp(hand)) {
					showHud (hand);
				} else {
					removeHud();
				}
			}
		}
	}

	bool isPalmUp(Hand hand) {
		return hand.PalmNormal.ToVector3 ().y > 0.5f;	
	}

	void showHud(Hand hand) {

		//Vector3 hudPosition = hand.PalmPosition.ToVector3 () + hudRelativePosition;
		Vector3 hudPosition = hand.WristPosition.ToVector3 () + hudRelativePosition;
		Quaternion hudRotation = Quaternion.LookRotation (hudPosition);
		if (hud == null) {
			hud = Instantiate (hudPrefab, hudPosition, hudRotation) as Transform;
		} else {
			hud.localPosition = hudPosition;
			hud.localRotation = hudRotation;
		}
	}

	void removeHud() {
		if (hud != null) {
			Destroy (hud.gameObject);
			hud = null;
		}
	}
}
