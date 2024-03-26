using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{

    [SerializeField]
    int boids = 40, sightRange = 10, minDistance = 2, bounds = 30;

    [SerializeField]
    Transform boid = default;

    List<Transform> boidList;
    List<Vector3> boidVelocity;


    void Start()
    {
        boidList = new List<Transform>();
        boidVelocity = new List<Vector3>();
        for (int i = 0; i < boids; i++)
        {
            Transform b = Instantiate(boid);
            b.position = RandomPos();
            boidList.Add(b);
            //boidVelocity.Add(Vector3.zero);
            boidVelocity.Add(RandomPos() / 10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalcPositions();
    }

    private void CalcPositions()
    {
        Transform b;
        Vector3 velocity = Vector3.zero;
        for (int i = 0; i < boidList.Count; i++)
        {
            b = boidList[i];
            velocity = Coherence(b) + Separation(b) + Alignment(boidVelocity[i]) /*+ CheckBounds(b)*/;
            boidVelocity[i] += velocity;
            b.position += velocity;
        }
    }

    private Vector3 Coherence(Transform b)
    {
        Vector3 p = Vector3.zero;
        int flockSize = 0;
        Transform targetBoid;

        for (int i = 0; i < boidList.Count; i++)
        {
            targetBoid = boidList[i];
            if (Vector3.Distance(b.position, targetBoid.position) < (float)sightRange && targetBoid != b)
            {
                p += targetBoid.position;
                flockSize++;
            }
        }
        if (flockSize != 0)
        {
            p /= flockSize;
        }

        return (p - b.position) / 200f;
    }

    private Vector3 Separation(Transform b)
    {
        Vector3 p = Vector3.zero;
        int flockSize = 0;
        Transform targetBoid;

        for (int i = 0; i < boidList.Count; i++)
        {
            targetBoid = boidList[i];
            
            if (Vector3.Distance(b.position, targetBoid.position) < (float)minDistance && targetBoid != b)
            {
                p -= targetBoid.position - b.position;
                flockSize++;
            }
        }
        return p / 20;
    }

    private Vector3 Alignment(Vector3 b)
    {
        Vector3 p = Vector3.zero;
        int flockSize = 0;
        Vector3 targetBoid;

        for (int i = 0; i < boidList.Count; i++)
        {
            targetBoid = boidVelocity[i];
                if (Vector3.Distance(b, targetBoid) < (float)sightRange && targetBoid != b)
                {
                    p += targetBoid;
                    flockSize++;
                }

        }

        if (flockSize != 0)
        {
            p /= flockSize;
        }

        return (p - b) / 20f;
    }

    private Vector3 CheckBounds(Transform b)
    {
        Vector3 p = Vector3.zero;
        if (b.position.x > (float)bounds)
        {
            p.x -= 1f;
        }
        else if (b.position.z < -(float)bounds)
        {
            p.x += 1f;
        }
        if (b.position.z > (float) bounds)
        {
            p.z -= 1f;
        }
        else if (b.position.z < -(float)bounds)
        {
            p.z += 1f;
        }
        return p;
    }
    private Vector3 RandomPos()
    {
       return new Vector3(Random.Range(-bounds, bounds), 0, Random.Range(-bounds, bounds));
    }
}
