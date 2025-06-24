using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SlashController : MonoBehaviour
{
    public Animator anim;
    public List<AttackData> attacks;
    private bool attacking;
    private int currentAttackIndex = 0;

    private void Start()
    {
        DisableAllSlashes();
    }

    void Update()
    {
        // OPTIONAL: Use keyboard for testing in Editor
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && !attacking)
        {
            PerformAttack(currentAttackIndex);
            currentAttackIndex = (currentAttackIndex + 1) % attacks.Count;
        }

        for (int i = 0; i < Mathf.Min(attacks.Count, 7); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !attacking)
            {
                PerformAttack(i);
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !attacking)
        {
            int randomAttack = Random.Range(0, attacks.Count);
            PerformAttack(randomAttack);
        }
#endif
    }

    // Performs the next attack in sequence (cycles through all attacks)
    public void PerformNextAttack()
    {
        if (!attacking && attacks.Count > 0)
        {
            PerformAttack(currentAttackIndex);
            currentAttackIndex = (currentAttackIndex + 1) % attacks.Count;
        }
    }

    private void PerformAttack(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < attacks.Count)
        {
            attacking = true;
            anim.SetTrigger(attacks[attackIndex].animationTrigger);
            StartCoroutine(SlashAttack(attackIndex));
        }
    }

    private IEnumerator SlashAttack(int attackIndex)
    {
        AttackData currentAttack = attacks[attackIndex];

        for (int i = 0; i < currentAttack.slashes.Count; i++)
        {
            yield return new WaitForSeconds(currentAttack.slashes[i].delay);
            currentAttack.slashes[i].slashObj.SetActive(true);
        }

        yield return new WaitForSeconds(currentAttack.attackDuration);
        DisableAttackSlashes(attackIndex);
        attacking = false;
    }

    private void DisableAttackSlashes(int attackIndex)
    {
        for (int i = 0; i < attacks[attackIndex].slashes.Count; i++)
        {
            attacks[attackIndex].slashes[i].slashObj.SetActive(false);
        }
    }

    private void DisableAllSlashes()
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            DisableAttackSlashes(i);
        }
    }

    [System.Serializable]
    public class AttackData
    {
        public string attackName;
        public string animationTrigger;
        public float attackDuration = 1f;
        public List<Slash> slashes;
    }

    [System.Serializable]
    public class Slash
    {
        public GameObject slashObj;
        public float delay;
    }
}