using UnityEngine;

public class StarfishRandomStart : MonoBehaviour
{
    [SerializeField]
    float cycleStart = 0;
    [SerializeField]
    float animSpeed = 0.5f;
    private Animator anim;

    private static readonly int RandomStart = Animator.StringToHash("RandomStart");
    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat(RandomStart, cycleStart);
        anim.SetFloat(Speed, animSpeed);
        
    }
}
