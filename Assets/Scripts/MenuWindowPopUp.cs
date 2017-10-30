using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;
using Unity.Linq;
using System.Linq;

public class MenuWindowPopUp {

	//コルーチン呼び出し
	public static void MenuPopUp(){
		Observable.FromCoroutine<string[]> (observer => Question(observer))
			.Subscribe (
				ansewer => Debug.LogFormat("あなたは『{0}』と『{1}』を選びました！",ansewer),
				() => Debug.Log("質問は以上です！"));
	}

	public static IEnumerator Question (IObserver<string[]> observer)
	{
		//ポップアップに必要なデータの設定
		PopUpData.Instance.SetAllData ();

		// 質問1つ目
		var yes = PopUpData.Instance.yesButton.OnClickAsObservable ()
			.First ()
			.Select (_ => PopUpData.Instance.yesButton);
		var no = PopUpData.Instance.noButton.OnClickAsObservable ()
			.First ()
			.Select (_ => PopUpData.Instance.noButton);
		var first = yes.Amb (no).ToYieldInstruction ();
		yield return first;

		var firstAnsewer = first.Result.GetComponentInChildren<Text> ().text;
		PopUpData.Instance.popUp1.SetActive (false);

		//Noの時
		if (first.Result == PopUpData.Instance.noButton) {
			Debug.LogFormat ("あなたは『{0}』 を選びました！", firstAnsewer);
			observer.OnCompleted ();
			PopUpData.Instance.isAnsewer = false;
			yield break;
		}

		PopUpData.Instance.popUp2.SetActive (true);

		// 質問2つ目
		// 何か選択するまで、進まない
		while (!PopUpData.Instance.toggles.AnyTogglesOn()) yield return null;
		var second = PopUpData.Instance.okButton.OnClickAsObservable ()
			.First ()
			.Select (_ => PopUpData.Instance.okButton)
			.ToYieldInstruction ();
		yield return second;

		var secondAnsewer = PopUpData.Instance.toggles.ActiveToggles().FirstOrDefault()
			.GetComponentsInChildren<Text>()
			.First(t => t.name == "Label").text;

		// 回答を配列にぶち込んでOnNextに流す
		string[] ansewer = new string[]{ firstAnsewer, secondAnsewer };
		observer.OnNext (ansewer);
		PopUpData.Instance.popUp2.SetActive (false);
		PopUpData.Instance.isAnsewer = false;
		observer.OnCompleted ();
	}
}