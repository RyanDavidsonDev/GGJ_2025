using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XP_Orb : MonoBehaviour
{
    [SerializeField] private Collider orbCollider;
    [Tooltip("This dictates a range for the amount of XP to drop, X is min, Y is max")]
    [SerializeField] private Vector2 XPAmount;
    [Tooltip("The max distance the orb will move on creation")]
    [SerializeField] private float moveSpeedRange;
    [SerializeField] private float moveDecaySpeed;

    private Vector2 direction;
    private float moveSpeed;
    private int exp;


    private void Start()
    {
        orbCollider = GetComponent<Collider>();
        float floatAngle = Random.Range(0f, 360f);
        direction = new Vector2 (Mathf.Sin(floatAngle * Mathf.Deg2Rad), Mathf.Cos(floatAngle * Mathf.Deg2Rad));
        moveSpeed = Random.Range(0.1f, moveSpeedRange);
        exp = Mathf.RoundToInt(Random.Range(XPAmount.x, XPAmount.y));
        transform.localScale = Vector3.one * (exp / XPAmount.y);
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {

        while (moveSpeed > 0)
        {
            transform.Translate(new Vector3(direction.x,0f,direction.y) * moveSpeed * Time.deltaTime);
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveDecaySpeed * Time.deltaTime);
            yield return null;
        }
    }
    public int GetXP()
    {
        return exp;
    }
}
