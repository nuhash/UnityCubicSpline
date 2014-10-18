UnityCubicSpline
================

Calculates cubic hermite splines, coded to work in Unity using C#.

The functionality is 100% finished, there are a few features I intend to add in the future but they aren't necessary for me right now (possibly not ever). The numerical solver for calculating the length is RK4 (Fourth Order Runga-Kutta), it can easily by modified to RKF45 (Runga-Kutta Fehlberg, Fourth order with Fifth order error estimator), however, the benefits of doing so will be minimal.

I have provided an example below on how to use the script, the functions are pretty straight-forward to use so it shouldn't take long for you to figure out how to use it.

The licence is MIT.

If there are any bugs please let me know.

Usage
-----
```C#
CubicSpline splineSampler = new CubicSpline(Vector3.zero, Vector3.right, Vector3.forward,-Vector3.right);
float splineLength = splineSampler.Length();
int numSamples = Mathf.CeilToInt(splineLength/0.5f);
List<Vector3> splineSamples = splineSampler.GetSamples(numSamples);
for(int i = 0;i<splinesSamples.Count;i++)
{
	var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	cube.transform.position = splineSamples[i];
}
```
