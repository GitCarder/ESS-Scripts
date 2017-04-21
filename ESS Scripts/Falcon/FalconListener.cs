using UnityEngine;

public interface FalconListener {

	void SetFalconActive(bool falcon_active);

    Vector3 FalconTipPosition(Vector3 position);

    void FalconButtonPressed(int i);

    void FalconButtonReleased(int i);


}
