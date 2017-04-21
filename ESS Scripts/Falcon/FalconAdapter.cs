using UnityEngine;

public class FalconAdapter : MonoBehaviour {
/*
    FalconListener[] listeners;

    void Start() {
        listeners = GetComponentsInChildren<FalconListener>();
        if (listeners == null)
            listeners = new FalconListener[0];
    }

    public void FalconTipPosition(Vector3 position) {
        foreach (FalconListener listener in listeners)
            listener.FalconTipPosition(position);
    }

    public void FalconButtonPressed(int i) {
        foreach (FalconListener listener in listeners)
            listener.FalconButtonPressed(i);
    }

    public void FalconButtonReleased(int i) {
        foreach (FalconListener listener in listeners)
            listener.FalconButtonReleased(i);
    }
*/
	public string displayName;
	FalconListener listener;
	
	void Start() {
		listener = GetComponent<FalconListener>();
//		if (listener == null)
//			listener = new FalconListener();
	}

	public void SetFalconActive(bool falcon_active){
		listener.SetFalconActive (falcon_active);
	}

	public Vector3 FalconTipPosition(Vector3 position) {
		//foreach (FalconListener listener in listeners)
			return listener.FalconTipPosition(position);
	}
	
	public void FalconButtonPressed(int i) {
		//foreach (FalconListener listener in listeners)
			listener.FalconButtonPressed(i);
	}
	
	public void FalconButtonReleased(int i) {
		//foreach (FalconListener listener in listeners)
			listener.FalconButtonReleased(i);
	}
}
