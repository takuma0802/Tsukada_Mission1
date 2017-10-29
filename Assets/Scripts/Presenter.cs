using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;
using Unity.Linq;
using System.Linq;

public class Presenter : MonoBehaviour {

	[SerializeField] private Button startButton, yesButton, noButton, okButton;
	[SerializeField] private GameObject popUp1, popUp2;
	[SerializeField] private ToggleGroup toggles;
	bool isAnsewer = false;

	// Use this for initialization
	void Start () {
		startButton.enabled = true;
		popUp1.SetActive (false);
		popUp2.SetActive (false);

		startButton.OnClickAsObservable ().Where (_ => !isAnsewer)
			.Subscribe (_ => {Observable.FromCoroutine<string[]> (observer => Question(observer))
					.Subscribe (
						ansewer => Debug.LogFormat("あなたは『{0}』と『{1}』を選びました！",ansewer),
						() => Debug.Log("質問は以上です！"));});
	}

	IEnumerator Question (IObserver<string[]> observer)
	{
		isAnsewer = true;
		popUp1.SetActive (true);

		var yes = yesButton.OnClickAsObservable ().First ().Select (_ => yesButton);
		var no = noButton.OnClickAsObservable ().First ().Select (_ => noButton);
		var x = yes.Amb (no).ToYieldInstruction ();
		yield return x;

		var firstAnsewer = x.Result.GetComponentInChildren<Text> ().text;
		popUp1.SetActive (false);
		if (x.Result == noButton) {
			Debug.LogFormat ("あなたは『{0}』 を選びました！", firstAnsewer);
			observer.OnCompleted ();
			isAnsewer = false;
			yield break;
		}
		popUp2.SetActive (true);
		while (!toggles.AnyTogglesOn()) yield return null;
		var ok = okButton.OnClickAsObservable ().First ().Select (_ => okButton).ToYieldInstruction ();
		yield return ok;

		var secondAnsewer = toggles.ActiveToggles().FirstOrDefault()
			.GetComponentsInChildren<Text>()
			.First(t => t.name == "Label").text;

		string[] ansewer = new string[]{ firstAnsewer, secondAnsewer };
		observer.OnNext (ansewer);
		popUp2.SetActive (false);
		isAnsewer = false;
		observer.OnCompleted ();
	}
}