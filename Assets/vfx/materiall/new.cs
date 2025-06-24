//using System.Collections.Generic;
//using UnityEngine;

//public class slashScripPtt : MonoBehaviour
//{
//    public Animator anim;
//    public List<SlashSequence> attackSequences;
//    private bool attacking;
//    private int currentAttack = 0;

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && !attacking)
//        {
//            attacking = true;

//            // Choose the current animation (can be random, sequential, etc.)  
//            var sequence = attackSequences[currentAttack];
//            anim.SetTrigger(sequence.triggerName);
//            StartCoroutine(SlashAttack(sequence.slashes));

//            currentAttack = (currentAttack + 1) % attackSequences.Count;
//        }
//    }

//    IEnumerator SlashAttack(List<Slash> slashes) // Fix: Use non-generic IEnumerator  
//    {
//        foreach (var slash in slashes)
//        {
//            yield return new WaitForSeconds(slash.delay);
//            slash.slashObj.SetActive(true);
//        }

//        yield return new WaitForSeconds(1); // Optional cooldown  
//        foreach (var slash in slashes)
//        {
//            slash.slashObj.SetActive(false);
//        }

//        attacking = false;
//    }

//    [System.Serializable]
//    public class Slash
//    {
//        public GameObject slashObj;
//        public float delay;
//    }

//    [System.Serializable]
//    public class SlashSequence
//    {
//        public string triggerName;
//        public List<Slash> slashes;
//    }
//}
