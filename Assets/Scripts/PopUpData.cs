using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using Unity.Linq;

public class PopUpData : SingletonMonoBehaviour<PopUpData> {
	
	public GameObject popUp, popUp1, popUp2;
	public Button menuButton, yesButton, noButton, okButton;
	public ToggleGroup toggles;
	public bool isAnsewer = false;

	//ポップアップに必要な変数をセット
	public void SetAllData(){
		isAnsewer = true;
		popUp = Instantiate (popUp,this.transform.position,popUp.transform.rotation);
		popUp.transform.parent = this.transform;
		popUp1 = popUp.Descendants ().Where (x => x.name == "PopUp1").First();
		popUp2 = popUp.Descendants ().Where (x => x.name == "PopUp2").First();
		yesButton = popUp.Descendants ().Where (x => x.name == "YesButton").First().GetComponent<Button>();
		noButton = popUp.Descendants ().Where (x => x.name == "NoButton").First().GetComponent<Button>();
		okButton = popUp.Descendants ().Where (x => x.name == "OkButton").First().GetComponent<Button>();
		toggles = popUp.Descendants ().Where (x => x.name == "PopUp2").First().GetComponent<ToggleGroup>();

		popUp1.SetActive (true);
		popUp2.SetActive (false);
	}
}
