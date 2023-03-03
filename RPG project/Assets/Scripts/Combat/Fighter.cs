using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 1.5f;
        [SerializeField] float timeBetweenAttacks = 1.3f;
        [SerializeField] float weaponDamage = 5f;
        
        Transform target;
        float timeSinceLastAttack = 0f;

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;

            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if(timeSinceLastAttack > timeBetweenAttacks)
            {    
                // Triggers the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        // Animation Event
        void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return (Vector3.Distance(transform.position, target.position) < weaponRange);
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);            
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
