using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    public AudioClip brickSound;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.magenta);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.cyan);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.gray);
                break;
            default:
                block.SetColor("_BaseColor", Color.white);
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        AudioManager.Instance.Play(brickSound);

        onDestroyed.Invoke(PointValue);
        
        // slight delay to be sure the ball have time to bounce
        // Not too much, destroys game feel
        Destroy(gameObject, 0.02f);
    }
}