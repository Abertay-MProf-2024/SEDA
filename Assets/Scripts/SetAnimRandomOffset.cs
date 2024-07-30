using UnityEngine;

public class SetAnimRandomOffset : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string initialAnimStateName;

    void Start()
    {
        float randomOffset = Random.Range(0f, 1f);
        animator.Play(initialAnimStateName, 0, randomOffset);
    }
}
