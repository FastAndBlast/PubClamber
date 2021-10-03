using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WalkingState { Resting, LeftMoving, RightMoving };

public enum FootEnum { Left, Right };

public class Foot
{
    public Foot() { }
    public Foot(Transform footTransform, Vector3 offset, FootEnum newState)
    {
        transform = footTransform;
        worldPos = footTransform.position;
        restingPosition = footTransform.position;
        forwardOffset = offset;
        state = newState;
    }
    public Foot(Vector3 pos, Transform footTransform, Vector3 offset, Vector3 resting, FootEnum newState)
    {
        worldPos = pos;
        transform = footTransform;
        forwardOffset = offset;
        restingPosition = resting;
        state = newState;
    }


    public Vector3 worldPos;
    public Transform transform;
    public Vector3 forwardOffset;
    public Vector3 restingPosition;
    public FootEnum state;

}

public class WalkingManager : MonoBehaviour
{
    //Vector3 leftFootWorldPos;
    //Vector3 rightFootWorldPos;

    [SerializeField]
    Vector3 initialMousePos;

    public Foot left;
    public Foot right;

    public Transform leftFoot;
    public Transform rightFoot;

    //Vector3 leftFootRestingPosition;
    //Vector3 rightFootRestingPosition;

    public WalkingState state = WalkingState.Resting;

    //Vector3 leftFootForwardOffset = new Vector3(-0.186948732f, 0.194999993f, 0.327000004f);
    //Vector3 rightFootForwardOffset = new Vector3(0.186948776f, 0.194999993f, 0.327000004f);

    float liftHeight = 0.2f;

    float stumbleDistance = 0.1f;
    float endDistance = 0.05f;
    float denyDistance = 0.025f;

    float timeToRestMax = 1.25f;
    float timeToRest;

    float stepLength;
    float stepSpeed = 1f;
    float shuffleSpeed = 0.5f;

    FootEnum lastFootMoved = FootEnum.Left;

    private RagDollParts ragDoll;

    float timeToNudgeMax = 1.5f;
    float timeToNudge;

    [SerializeField]
    Transform spinePart;

    float spineBaseAngle;
    public float spineStumbleAngle = 40;

    float stumbleTimeMax = 1.5f;
    float stumbleTime = 0;
    

    public void Start()
    {
        left = new Foot(leftFoot, new Vector3(-0.186948732f, 0.194999993f, 0.327000004f), FootEnum.Left);
        right = new Foot(rightFoot, new Vector3(0.186948732f, 0.194999993f, 0.327000004f), FootEnum.Right);

        spineBaseAngle = spinePart.rotation.x;

        ragDoll = GetComponent<RagDollParts>();

        //left.transform = leftFoot;
        //right.transform = rightFoot;

        //leftFootRestingPosition = leftFoot.position;
        //left.restingPosition = left.transform.position;
        //right.restingPosition = right.transform.position;

        //left.worldPos = left.restingPosition;
        //right.worldPos = right.restingPosition;

        //left.forwardOffset = new Vector3(-0.186948732f, 0.194999993f, 0.327000004f);
        //right.forwardOffset = new Vector3(0.186948776f, 0.194999993f, 0.327000004f);

        //leftFootWorldPos = leftFootRestingPosition;
    }

    private void Update()
    {
        LookAt();

        Vector3 newTransformPosition = (left.worldPos + right.worldPos) / 2;
        newTransformPosition.y = 0;
        transform.position = newTransformPosition;


        //leftFoot.transform.position = left.worldPos;
        //rightFoot.transform.position = right.worldPos;

        StumbleUpdate();

        if (Input.GetKeyDown(KeyCode.S))
        {
            Stumble();
        }

        if (Input.GetButtonDown("LeftFoot"))
        {
            if (state == WalkingState.RightMoving)
            {
                if (Vector3.Distance(left.worldPos, transform.position + left.forwardOffset) > stumbleDistance)
                {
                    //Stumble
                }
                else
                {
                    //lastFootMoved = FootEnum.Left;
                    state = WalkingState.LeftMoving;
                    stepLength = Vector3.Distance(leftFoot.transform.localPosition, left.forwardOffset);
                }
            }
            else if (state == WalkingState.Resting)
            {
                //lastFootMoved = FootEnum.Left;
                state = WalkingState.LeftMoving;
                stepLength = Vector3.Distance(leftFoot.transform.localPosition, left.forwardOffset);
            }
        }
        if (Input.GetButtonDown("RightFoot"))
        {
            if (state == WalkingState.LeftMoving)
            {
                if (Vector3.Distance(left.worldPos, transform.position + left.forwardOffset) > stumbleDistance)
                {
                    //Stumble
                }
                else
                {
                    //lastFootMoved = FootEnum.Right;
                    state = WalkingState.RightMoving;
                    stepLength = Vector3.Distance(rightFoot.transform.localPosition, right.forwardOffset);
                }
            }
            else if (state == WalkingState.Resting)
            {
                //lastFootMoved = FootEnum.Right;
                state = WalkingState.RightMoving;
                stepLength = Vector3.Distance(rightFoot.transform.localPosition, right.forwardOffset);
            }
        }

        if (state == WalkingState.Resting)
        {
            if (timeToRest > 0)
            {
                //TODO PAUSE
                timeToRest -= Time.deltaTime;
                if (!(timeToRest > 0))
                {
                    if (lastFootMoved == FootEnum.Left)
                    {
                        stepLength = Vector3.Distance(leftFoot.transform.localPosition, left.restingPosition) - denyDistance;
                    }
                    else
                    {
                        stepLength = Vector3.Distance(rightFoot.transform.localPosition, right.restingPosition) - denyDistance;
                    }
                }
                left.worldPos = leftFoot.transform.position;
                right.worldPos = rightFoot.transform.position;
            }
            else
            {
                //Drag foot to rest
                if (lastFootMoved == FootEnum.Left)
                {
                    MoveFoot(right, right.restingPosition, shuffleSpeed);
                }
                else
                {
                    MoveFoot(left, left.restingPosition, shuffleSpeed);
                }
            }
        }
        else if (state == WalkingState.LeftMoving)
        {
            MoveFoot(left, left.forwardOffset, stepSpeed);
        }
        else if (state == WalkingState.RightMoving)
        {
            //rightFootWorldPos = rightFoot.TransformPoint(Vector3.Lerp(rightFoot.InverseTransformPoint(rightFootWorldPos), rightFoot.localPosition + rightFootForwardOffset, Time.deltaTime * 5));
            //print(Vector3.Distance(rightFoot.transform.localPosition, rightFootForwardOffset));
            MoveFoot(right, right.forwardOffset, stepSpeed);
        }
    }
        
    public void MoveFoot(Foot foot, Vector3 target, float speed)
    {
        float disp = Vector3.Distance(foot.transform.localPosition, target);

        if (disp < denyDistance)
        {
            state = WalkingState.Resting;
            timeToRest = timeToRestMax;
            lastFootMoved = foot.state;
            return;
        }

        Vector3 newFootPosition = foot.transform.TransformPoint(Vector3.MoveTowards(foot.transform.localPosition, target, Time.deltaTime * speed));

        print(newFootPosition);
        print(foot.transform.GetComponent<Rigidbody>());
        foot.transform.GetComponent<Rigidbody>().MovePosition(newFootPosition);

        //foot.transform.TransformDirection()

        //print(rightFoot.localPosition + rightFootForwardOffset);

        disp = Vector3.Distance(foot.transform.localPosition, target);

        left.worldPos = left.transform.position;
        right.worldPos = right.transform.position;

        float lift = 0;

        if (stepLength != 0)
        {
            lift = Mathf.Clamp(-disp * (disp - stepLength) / (stepLength * stepLength / 4) * liftHeight - 0.1f, 0, 100);
        }

        //print(lift);

        //print(disp);

        //foot.transform.localPosition += new Vector3(0, lift, 0);

        if (disp < endDistance)//(Vector3.Distance(rightFoot.InverseTransformPoint(rightFootWorldPos), rightFoot.localPosition + rightFootForwardOffset) < endDistance)
        {
            state = WalkingState.Resting;
            timeToRest = timeToRestMax;
            lastFootMoved = foot.state;
            //lastFootMoved = 1 - lastFootMoved;
            //print(lastFootMoved);
        }
    }

    public void LookAt()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 15;

        Vector3 dir = (Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position).normalized;

        //point at which ray hits y = 0
        Vector3 point = Camera.main.transform.position + (Camera.main.transform.position.y / -dir.y) * dir;

        //print(point);
        Debug.DrawLine(Camera.main.transform.position, point, Color.yellow);
        //Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + dir * 10, Color.yellow);

        //Ray r = new Ray(Camera.main.transform.position + dir, dir);

        //RaycastHit hit;

        //Physics.Raycast(r, out hit);

        //mousePos.z = Vector3.Distance(hit.point, Camera.main.transform.position);

        //Vector3 lookPosition = Camera.main.ScreenToWorldPoint(mousePos);

        //lookPosition.y = 0;

        Vector3 targetRotation = point - transform.position; //lookPosition - transform.position;

        transform.forward = Vector3.MoveTowards(transform.forward, targetRotation, Time.deltaTime * 2);

        if (timeToNudge > 0)
        {
            //TODO PAUSE
            timeToNudge -= Time.deltaTime;
        }
        else
        {
            ragDoll.Force(20f * transform.forward, BodyPart.Body);
            timeToNudge = timeToNudgeMax;
        }
    }

    private void OnDrawGizmos()
    {
        if (left != null)
        {
            //left.transform.TransformDirection(Vector3.forward);
            //Gizmos.DrawLine( );
            Gizmos.DrawLine(left.transform.position, left.transform.TransformPoint(left.forwardOffset));
            
        }
        if (right != null)
        {
            Gizmos.DrawLine(right.transform.position, right.transform.TransformPoint(right.forwardOffset));
        }
        

    }

    public void StumbleUpdate()
    {
        if (stumbleTime > 0)
        {
            if (stumbleTime > 0.5f)
            {
                spinePart.eulerAngles = new Vector3(Mathf.MoveTowards(spinePart.eulerAngles.x, spineStumbleAngle, Time.deltaTime * 25), spinePart.eulerAngles.y, spinePart.eulerAngles.z);
            }
            else
            {
                spinePart.eulerAngles = new Vector3(Mathf.MoveTowards(spinePart.eulerAngles.x, spineBaseAngle, Time.deltaTime * 25), spinePart.eulerAngles.y, spinePart.eulerAngles.z);
            }

            stumbleTime -= Time.deltaTime;
        }
    }

    public void Collided(Vector3 collisionPoint)
    {
        if (state == WalkingState.LeftMoving)
        {
            left.transform.position -= (transform.position - collisionPoint).normalized * 0.5f;
        }
        else if (state == WalkingState.RightMoving)
        {
            right.transform.position -= (transform.position - collisionPoint).normalized * 0.5f;
        }

        state = WalkingState.Resting;

        print("Collided");
    }

    public void Stumble()
    {
        print("Stumbled");
        stumbleTime = stumbleTimeMax;
        ragDoll.Force(100 * transform.forward, BodyPart.Body);
        ragDoll.Force(200 * transform.forward, BodyPart.Arms);
    }

    /*
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 15;

            Vector3 dir = (Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position).normalized;

            Ray r = new Ray(Camera.main.transform.position + dir, dir);

            RaycastHit hit;

            Physics.Raycast(r, out hit);

            mousePos.z = Vector3.Distance(Camera.main.transform.position, hit.point);


            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.y = 0;
            //mousePos.y = 0;
            initialMousePos = mousePos; //new Vector2(mousePos.x, mousePos.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(initialMousePos, 0.5f);

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 15;

            Vector3 dir = (Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position).normalized;

            Ray r = new Ray(Camera.main.transform.position + dir, dir);

            Gizmos.DrawRay(r);

            RaycastHit hit;

            Physics.Raycast(r, out hit);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(hit.point.x, 0, hit.point.z), 0.5f);

            Gizmos.color = Color.red;

            mousePos.z = Vector3.Distance(Camera.main.transform.position, hit.point);

            //Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + dir * mousePos.z);

            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.y = 0;
            //mousePos.y = 0;
            //Gizmos.DrawLine(new Vector3(initialMousePos.x, 0, initialMousePos.y), mousePos);
            Gizmos.DrawLine(initialMousePos, mousePos);
        }
    }
    */



}