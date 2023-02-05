using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 WiggleDelta = new Vector2(2.011f, 0.11f);

    public Vector2 WiggleSpeed = new Vector2(0.67f, 1.73f);

    public Vector2 MicroWiggleDelta = new Vector2(0.6f, 0.2f);
    public Vector2 MicroWiggleSpeed = new Vector2(0.9f, 0.7f);



    private Vector2 _wiggleTime = Vector2.zero;

    private Vector2 _microWiggleTime = Vector2.zero;


    public void Update()
    {
        this._wiggleTime.x += Time.deltaTime * this.WiggleSpeed.x;
        this._wiggleTime.y += Time.deltaTime * this.WiggleSpeed.y;

        this._microWiggleTime.x += Time.deltaTime * this.MicroWiggleSpeed.x;
        this._microWiggleTime.y += Time.deltaTime * this.MicroWiggleSpeed.y;

        var x = Mathf.Sin(this._wiggleTime.x) * this.WiggleDelta.x;
        var y = Mathf.Sin(this._wiggleTime.y) * this.WiggleDelta.y;

        var mx = Mathf.Sin(this._microWiggleTime.x) * this.MicroWiggleDelta.x * 0.1f;
        var my = Mathf.Sin(this._microWiggleTime.y) * this.MicroWiggleDelta.y * 0.1f;

        this.transform.localPosition = new Vector3(x + mx, y + my, this.transform.localPosition.z);
    }
}
