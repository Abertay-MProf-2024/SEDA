using UnityEngine;

public class PlayAnimationsAtRandom : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Start()
    {
        float randomOffset = Random.Range(0f, 1f);
        animator.Play("Cow Feeding", 0, randomOffset);

        int animationIndex = Random.Range(0, 2);
        animator.SetInteger("AnimIndex", animationIndex);
        print("Index: " + animationIndex);
        print("Animation Index " + animator.GetInteger("AnimIndex"));
    }
}
