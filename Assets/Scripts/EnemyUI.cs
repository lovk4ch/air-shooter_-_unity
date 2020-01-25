using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyController))]
public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowPrefab = null;
    private Image arrow;

    [SerializeField]
    private GameObject billetPrefab = null;
    private Text billet;

    private bool isVisible;
    private EnemyController enemy;

    private void Awake()
    {
        billet = Instantiate(billetPrefab, LevelManager.Instance.ui.transform).GetComponent<Text>();
        arrow = Instantiate(arrowPrefab, LevelManager.Instance.ui.transform).GetComponent<Image>();

        enemy = GetComponent<EnemyController>();
    }

    private void OnDestroy()
    {
        if (!LevelManager.Instance)
            return;

        Destroy(billet.gameObject);
        Destroy(arrow.gameObject);
    }

    private void LateUpdate()
    {
        if (enemy.target) {

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            isVisible = GeometryUtility.TestPlanesAABB(planes, enemy.collider.bounds);

            billet.gameObject.SetActive(isVisible);
            if (billet.gameObject.activeSelf)
            {
                billet.text = Mathf.Round(enemy.targetDir.magnitude).ToString();
                billet.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3);
            }

            arrow.gameObject.SetActive(!isVisible);
            if (arrow.gameObject.activeSelf)
            {
                var pos = Camera.main.WorldToScreenPoint(transform.position);
                var scale = arrow.rectTransform.rect.width / 2;

                var x = Mathf.Min(pos.x, Screen.width - pos.x);
                var y = Mathf.Min(pos.y, Screen.height - pos.y);
                int z;

                if (x < y)
                {
                    if (pos.x > Screen.width / 2)
                    {
                        pos.x = Screen.width - scale;
                        z = 0;
                    }
                    else
                    {
                        pos.x = scale;
                        z = 180;
                    }
                }
                else
                {
                    if (pos.y > Screen.height / 2)
                    {
                        pos.y = Screen.height - scale;
                        z = 90;
                    }
                    else
                    {
                        pos.y = scale;
                        z = -90;
                    }
                }

                arrow.transform.position = pos;
                arrow.transform.rotation = Quaternion.Euler(0, 0, z);
                // arrow.transform.rotation = Quaternion.Euler(0, 0, x > y ? (pos.y == scale ? -90 : 90) : (pos.x == scale ? 180 : 0));
            }
        }
    }
}