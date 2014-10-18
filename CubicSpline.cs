using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
public class CubicSpline
{
	readonly object _queueLock = new object();
	int queue = 0;
	float length =0;
	int iterations = 50;
	float h;
	Vector3[] coefficients = new Vector3[4];

	public CubicSpline (Vector3 position0, Vector3 gradient0, Vector3 position1, Vector3 gradient1)
	{
		coefficients[3] = 2*position0 + gradient0 - 2*position1 + gradient1;
		coefficients[2] = 3*position1 - gradient1 - 3*position0 - 2*gradient0;
		coefficients[1] = gradient0;
		coefficients[0] = position0;
		h =  1/Convert.ToSingle(iterations);
	}

	public List<Vector3> GetSamples(int numSamples)
	{
		List<Vector3> samples = new List<Vector3>();
		float step = 1/Convert.ToSingle(numSamples);
		for (int i = 0; i < numSamples+1; i++) {
			float t = i*step;
			samples.Add(Sample(t));
		}
		return samples;
	}

	public Vector3 Sample(float t)
	{
		return coefficients[3]*t*t*t+coefficients[2]*t*t+coefficients[1]*t+coefficients[0];
	}

	public List<Vector3> GetGradientSamples(int numSamples){
		List<Vector3> samples = new List<Vector3>();
		float step = 1/Convert.ToSingle(numSamples);
		for (int i = 0; i < numSamples+1; i++) {
			float t = i*step;
			samples.Add(GradientSample(t));
		}
		return samples;
	}

	public Vector3 GradientSample(float t)
	{
		return 3*coefficients[3]*t*t+2*coefficients[2]*t+coefficients[1];
	}
	
	public float Length()
	{
		length = 0;
		queue = iterations;
		for (int i = 0; i < iterations+1; i++) {
			ThreadPool.QueueUserWorkItem(new WaitCallback(LengthThread),i);
		}
		while(queue>0)
		{
			Thread.Sleep(50);
		}
		return length;
	}
		
	private void LengthThread(object a)
	{
		float t = h*(int)a;
		float k1 = Ds(t);
		float k2 = Ds(t+(h/3)*k1);
		float k3 = Ds (t+h*((-1f/3f)*k1+k2));
		float k4 = Ds (t+h*(k1-k2+k3));
		float diff = h*(k1+3*k2+3*k3+k4)/8;
		lock (_queueLock) 
		{
			length += diff;
			--queue;
		}
	}

	private Vector3 ScalarProduct(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x*v2.x,v1.y*v2.y,v1.z*v2.z);
	}

	private float Ds(float t)
	{
		float t4 = 9*coefficients[3].sqrMagnitude*t*t*t*t;
		float t3 = 12*Vector3.Dot(coefficients[3],coefficients[2])*t*t*t;
		float t2 = (6*Vector3.Dot(coefficients[3],coefficients[1])+4*coefficients[2].sqrMagnitude)*t*t;
		float t1 = 4*Vector3.Dot(coefficients[2],coefficients[1])*t;
		float result =  Mathf.Sqrt (t4+t3+t2+t1+coefficients[1].sqrMagnitude);
		return result;
	}

}


