using UnityEngine;
using UnityEngine.XR;
using System;

[Serializable]
public class Rig
{
  InputDevice headset, offCon, mainCon;

  // Tracking
  public Vector3 offset = Vector3.down;
  public Vector3 headsetPos, offConPos, mainConPos;
  public Vector3 headsetVel, offConVel, mainConVel;
  [HideInInspector]
  public Quaternion headsetRot, offConRot, mainConRot;

  // Input
  public Btn mainConTrigger = new Btn();
  public Btn mainConOne = new Btn();
  public Vector2 mainConJoystick;

  [Header("References")]
  public Camera headsetCam;
  public Material mat;
  public Mesh meshCon;

  Monolith mono;
  public void Start(Monolith mono)
  {
    this.mono = mono;

    headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);
    offCon = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    mainCon = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
  }

  Matrix4x4 m4 = new Matrix4x4();
  public void Update()
  {
    // Tracking
    headset.TryGetFeatureValue(CommonUsages.devicePosition, out headsetPos);
    headset.TryGetFeatureValue(CommonUsages.deviceRotation, out headsetRot);
    headset.TryGetFeatureValue(CommonUsages.deviceVelocity, out headsetVel); // deviceAcceleration?

    offCon.TryGetFeatureValue(CommonUsages.devicePosition, out offConPos);
    offCon.TryGetFeatureValue(CommonUsages.deviceRotation, out offConRot);
    offCon.TryGetFeatureValue(CommonUsages.deviceVelocity, out offConVel);

    mainCon.TryGetFeatureValue(CommonUsages.devicePosition, out mainConPos);
    mainCon.TryGetFeatureValue(CommonUsages.deviceRotation, out mainConRot);
    mainCon.TryGetFeatureValue(CommonUsages.deviceVelocity, out mainConVel);

    headsetRot.Normalize();
    offConRot.Normalize();
    mainConRot.Normalize();

    headsetPos += offset;
    offConPos += offset;
    mainConPos += offset;

    // Input
    bool state;
    mainCon.TryGetFeatureValue(CommonUsages.triggerButton, out state);
    mainConTrigger.On(state);

    // cam
    headsetCam.transform.position = headsetPos;
    headsetCam.transform.rotation = headsetRot;

    // rbHead.velocity += headsetVel * Time.deltaTime;

    // Vector3 body = rbHead.transform.position;
    // headsetCam.transform.position = body;


    // Render
    m4.SetTRS(offConPos, offConRot, Vector3.one * 0.1f
    ); Graphics.DrawMesh(meshCon, m4, mat, 0);

    m4.SetTRS(mainConPos, mainConRot, Vector3.one * 0.1f
    ); Graphics.DrawMesh(meshCon, m4, mat, 0);
  }
}

public class Btn
{
  public bool onPress, held, onUp;

  public void On(bool state)
  {
    onPress = onUp = false;
    if (state)
    {
      if (!held) { onPress = true; }
      held = true;
    }
    else
    {
      if (held) { onUp = true; }
      held = false;
    }
  }
}