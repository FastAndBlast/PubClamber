using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPart { Legs, Arms, Body, Head };

public class RagDollParts : MonoBehaviour
{
    List<Rigidbody> rbs = new List<Rigidbody>();
    List<Collider> cols = new List<Collider>();

    public List<GameObject> legGameObjects;
    public List<GameObject> armGameObjects;
    public List<GameObject> bodyGameObjects;
    public List<GameObject> headGameObjects;

    public bool legsDisabled;
    public bool armsDisabled;
    public bool bodyDisabled;
    public bool headDisabled;

    //bool[] beforePause;

    private void Start()
    {
        //Rigidbody[] getRbs = GetComponentsInChildren<Rigidbody>();
        //Collider[] getCols = GetComponentsInChildren<Collider>();


        //foreach (Rigidbody rb in getRbs)
        //{
            //rbs.Add(rb);
        //}

        //foreach (Collider col in getCols)
        //{
            //cols.Add(col);
        //}

        Disable(legsDisabled, armsDisabled, bodyDisabled, headDisabled, false);
    }

    private void Update()
    {
        if (GameManager.paused)
        {
            //beforePause = new bool[4] { legsDisabled, armsDisabled, bodyDisabled, headDisabled };
            Disable(true, true, true, true, false);
        }
        else
        {
            Enable(!legsDisabled, !armsDisabled, !bodyDisabled, !headDisabled, false);
        }
    }

    public void Force(Vector3 force, BodyPart part)
    {
        if (part == BodyPart.Body)
        {
            foreach (GameObject obj in bodyGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.AddForce(force);
                }
            }
        }
        if (part == BodyPart.Arms)
        {
            foreach (GameObject obj in armGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.AddForce(force);
                }
            }
        }
    }

    public void Disable(bool legs = false, bool arms = false, bool body = false, bool head = false, bool commit = true)
    {
        //List<List<GameObject>>
        if (legs)
        {
            foreach (GameObject obj in legGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = true;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
            if (commit)
            {
                legsDisabled = legs;
            }
        }

        if (arms)
        {
            foreach (GameObject obj in armGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = true;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
            if (commit)
            {
                armsDisabled = arms;
            }
        }

        if (body)
        {
            foreach (GameObject obj in bodyGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = true;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
            if (commit)
            {
                bodyDisabled = body;
            }
        }

        if (head)
        {
            foreach (GameObject obj in headGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = true;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = false;
                }
            }
            if (commit)
            {
                headDisabled = head;
            }
        }

        /*
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
        */
    }

    private void Enable(bool legs = false, bool arms = false, bool body = false, bool head = false, bool commit = true)
    {
        if (legs)
        {
            foreach (GameObject obj in legGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = false;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = true;
                }
            }
            if (commit)
            {
                legsDisabled = !legs;
            }
        }

        if (arms)
        {
            foreach (GameObject obj in armGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = false;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = true;
                }
            }
            if (commit)
            {
                armsDisabled = !arms;
            }
        }

        if (body)
        {
            foreach (GameObject obj in bodyGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = false;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = true;
                }
            }
            if (commit)
            {
                bodyDisabled = !body;
            }
        }

        if (head)
        {
            foreach (GameObject obj in headGameObjects)
            {
                foreach (Rigidbody rb in obj.GetComponents<Rigidbody>())
                {
                    rb.isKinematic = false;
                }
                foreach (Collider col in obj.GetComponents<Collider>())
                {
                    col.enabled = true;
                }
            }
            if (commit)
            {
                headDisabled = !head;
            }
        }
    }

}
