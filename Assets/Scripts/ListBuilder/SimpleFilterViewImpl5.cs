
using System;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(InputField))]
public class SimpleFilterViewImpl5 : MonoBehaviour , FilterView {
	
	#region FilterView Implementation
	
	public event Action OnFilterValueChanged;
	
	#endregion
	
	#region Behaviour

    public string Value { get { return TextField.text; } }
	
	public InputField TextField { get; private set; }

	public void Clear()
	{
		if (TextField != null)
			TextField.text = string.Empty;
	}

	public bool IsNullOrEmpty { get { return TextField == null || string.IsNullOrEmpty(TextField.text); } }

	protected virtual void Awake() {
		TextField = GetComponent<InputField>();
	}
	
	protected virtual void Start() {
        TextField.onValueChanged.AddListener(OnChangeCallback);
	}
	
	protected virtual void OnChangeCallback(string s ) {
		if ( OnFilterValueChanged != null )
			OnFilterValueChanged();
	}
	
	#endregion
	
}
