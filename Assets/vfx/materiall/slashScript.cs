using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic; // Fixed: Add using directive for System.Collections.Generic
public class slashScripPt : MonoBehaviour
{
    public Animator anim;
    public List<Slash> slashes; // Fixed: Use System.Collections.Generic.List

    private bool attacking;

    private void Start()
    {
        DisableSlashes();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !attacking)
        {
            attacking = true;
            anim.SetTrigger("attack");
            StartCoroutine(SlashAttack());
        }
    }

    IEnumerator SlashAttack()
    {
        for (int i = 0; i < slashes.Count; i++) // Fixed: Correct property name is Count
        {
            yield return new WaitForSeconds(slashes[i].delay);
            slashes[i].slashObj.SetActive(true);
        }

        yield return new WaitForSeconds(1);
        DisableSlashes();
        attacking = false;
    }

    void DisableSlashes()
    {
        for (int i = 0; i < slashes.Count; i++) // Fixed: Correct property name is Count
        {
            slashes[i].slashObj.SetActive(false);
        }
    }

    [System.Serializable]
    public class Slash
    {
        public GameObject slashObj;
        public float delay;
    }
}
