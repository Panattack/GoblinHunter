using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAnimationFSM;

public class EnemyHitState : BaseAnimationState<EnemyAnimation>
{
    private Dictionary<Aspects, AnimationClip> hitState = new Dictionary<Aspects, AnimationClip>();

    private EnemyStateMachine _stateMachine;

    private SpriteRenderer sprite;
    private Transform enemyTransform;
    private float staggerTime;
    private float timer;

    private bool flash;

    private Material flashMaterial;
    private Material originalMaterial;
    public EnemyHitState(AnimationAspectManager _aspectManager, EnemyStateMachine stateMachine, EnemyAnimation key = EnemyAnimation.HIT)
        : base(key)
    {
        Id = 0;

        _stateMachine = stateMachine;

        //sprite renderer
        sprite = _aspectManager.gameObject.GetComponentInChildren<SpriteRenderer>();
        flashMaterial = Object.Instantiate(sprite.material);
        Color currentColor = flashMaterial.color;
        Color newColor = currentColor * 4;
        flashMaterial.color = newColor;
        originalMaterial = sprite.material;

        enemyTransform = _aspectManager.gameObject.GetComponent<Transform>();
        staggerTime = 0.5f;
        flash = true;
        AspectManager = _aspectManager;

        hitState[Aspects.FRONT] = new AnimationClip(Animator.StringToHash("Hit"), 0f);
    }

    private void ChangeAnimation(Aspects aspect)
    {
        Id = hitState[aspect].State;
        Duration = hitState[aspect].Duration;
    }

    public override void EnterState()
    {
      timer = 0;        
        ChangeAnimation(Aspects.FRONT);

        enemyTransform.Translate(50 * Time.deltaTime * -enemyTransform.forward, Space.World);
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (flash)
        {
            sprite.material = flashMaterial;
            flash = false;
        }
        else
        {
            sprite.material = originalMaterial;
            flash = true;
        }

    }

    public override void ExitState()
    {
        sprite.material = originalMaterial;
    }

    public override EnemyAnimation GetNextState()
    {
        if (timer > staggerTime)
        {
            if (_stateMachine.isAttacking)
            {
                return EnemyAnimation.ATTACK;
            }

            if (_stateMachine.Agent.velocity.magnitude > 0)
            {
                return EnemyAnimation.WALK;
            }

            return EnemyAnimation.IDLE;
        }
        return EnemyAnimation.HIT;

    }
}
