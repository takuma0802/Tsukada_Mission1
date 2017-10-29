using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;

public class Presenter : MonoBehaviour {


	[SerializeField] private Button startButton;
	[SerializeField] private GameObject popUp1;
	[SerializeField] private Button yesButton;
	[SerializeField] private Button noButton;
	[SerializeField] private GameObject popUp2;
	[SerializeField] private Toggle Button1;
	[SerializeField] private Toggle Button2;
	[SerializeField] private Toggle Button3;
	[SerializeField] private Button okButton;

	private Toggle[] buttons;

	string z;

	bool isAnsewer = false;

	// Use this for initialization
	void Start () {
		startButton.enabled = true;
		popUp1.SetActive (false);
		popUp2.SetActive (false);
		buttons = new Toggle[] {Button1,Button2,Button3};

		startButton.OnClickAsObservable ()
			.Where (_ => !isAnsewer)
			.Subscribe (_ => {
				Observable.FromCoroutine<string[]> (observer => Question(observer))
					.Subscribe (
						x => Debug.LogFormat("あなたは{0}と、{1}を選びました！",x),
						() => Debug.Log("質問は以上です！"));
				});
	}


	IEnumerator Question (IObserver<string[]> observer)
	{
		popUp1.SetActive (true);
		popUp2.SetActive (false);

		var yes = yesButton
			.OnClickAsObservable ()
			.First ()
			.Select (_ => yesButton);
		
		var no = noButton
			.OnClickAsObservable ()
			.First ()
			.Select (_ => noButton);
		
		var x = yes.Amb (no).ToYieldInstruction ();

		yield return x;

		popUp1.SetActive (false);

		if (x.Result == noButton) {
			Debug.LogFormat ("{0} がクリックされました", x.Result);
			observer.OnCompleted ();
			yield break;
		}

		popUp2.SetActive (true);
		var ok = okButton
			.OnClickAsObservable ()
			.First ()
			.Select (_ => okButton)
			.ToYieldInstruction ();

		yield return ok;

		for (int i = 0; i < buttons.Length; i++) {
			if (buttons [i].isOn)
				z = buttons [i].name;
		}
		string[] zz = new string[]{ x.Result.gameObject.name, z };
		observer.OnNext (zz);
		popUp2.SetActive (false);
		observer.OnCompleted ();
	}
}

