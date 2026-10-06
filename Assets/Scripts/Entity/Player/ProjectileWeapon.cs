using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject projectilePrefab;
    private float attackOffset = 0.5f;
    private Vector2 bodyCenter = new Vector2(0f, 0.5f);

    public void PerformAttack(Vector2 facingDirection)
    {
        float targetX = bodyCenter.x + (facingDirection.x * attackOffset);
        float targetY = bodyCenter.y + (facingDirection.y * attackOffset);
        transform.localPosition = new Vector3(targetX, targetY, 0);

        if (projectilePrefab != null)
        {
            AudioManager.Instance.PlaySFX("SkillFire", "COMBAT");
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
                projectileScript.Setup(facingDirection);
        }
    }
}