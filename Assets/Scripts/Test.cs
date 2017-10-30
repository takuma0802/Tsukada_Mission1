using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.UpdateAsObservable ()
			.Where (x => Input.GetMouseButtonDown (1) & !PopUpData.Instance.isAnsewer)
			.Subscribe (x => MenuWindowPopUp.MenuPopUp ());
			
		PopUpData.Instance.menuButton.OnClickAsObservable ().Where (_ => !PopUpData.Instance.isAnsewer)
			.Subscribe (_ => MenuWindowPopUp.MenuPopUp ());
	}
}
