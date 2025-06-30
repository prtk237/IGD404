using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityHandler : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField] private List<AbilityBaseClass> abilities = new List<AbilityBaseClass>();

    [SerializeField] private Transform testTransform;

    //Currently not in use 
    [Header("Events")]
    public UnityEvent onStyleIncrease;
    public UnityEvent onStyleDecrease;

    [Header("AOESpell")]
    public float maxAbilityDistance = 7;

    private Vector3 pos;
    private Ray ray;
    private RaycastHit hit;

    public Color sphereColor = Color.yellow;
    public float castRadius;

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        AbilityCanvas();
        AbilityCooldown(abilities); 
    }

    private void AbilityCanvas()
    {

        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                pos = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbilityDistance);

        var newHitPos = transform.position + hitPosDir * distance;
        testTransform.position = (newHitPos);
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseAbility(0);
        }
    }

    private void UseAbility(int index)
    {
        if(abilities.Count > 0 && abilities[index] != null)
        {
            if (!abilities[index].IsOnCooldown)
            {
                abilities[index].Ability(testTransform);
                abilities[index].IsOnCooldown = true;
                //StartCoroutine(Cooldown(abilities[index], abilities[index].AbilityCooldown));
            }
        }

        if (abilities[index] == null)
        {
            Debug.Log("this");
        }

    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseAbility(1);
        }
    }

    private void OnDrawGizmos()
    {
        if(testTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(testTransform.position, castRadius);
        }
    }

    private IEnumerator Cooldown(AbilityBaseClass ability, float cooldown)
    {
        ability.IsOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        ability.IsOnCooldown = false;
    }


    private void AbilityCooldown(List<AbilityBaseClass> abilities)
    {
        foreach (AbilityBaseClass ability in abilities)
        {
            if (ability.IsOnCooldown)
            {
                ability.AbilityCooldownTimer -= Time.deltaTime;

                if (ability.AbilityCooldownTimer < 0.01f)
                {
                    ability.IsOnCooldown = false;
                    ability.AbilityCooldownTimer = ability.AbilityCooldown;
                }
            }
        }
    }
}
