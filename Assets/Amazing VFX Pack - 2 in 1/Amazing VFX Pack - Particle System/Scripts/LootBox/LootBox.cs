using UnityEngine;

namespace ParticleEffect.Scripts
{
    public class LootBox : MonoBehaviour
    {
        [SerializeField] private GameObject lootRewardPrefab;
        [SerializeField] private GameObject lootFracturePrefab;
        [SerializeField] private GameObject crackGlow;
        private Animator _animator;
        private Ray ray;
        private RaycastHit hit;
        private bool clickLeftMouse = false;
    
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
    
        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == gameObject.name)
                {
                    Debug.Log("Hover");
    
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickLeftMouse = true;
                        Debug.Log("Click Left Mouse = " + clickLeftMouse);
                    }
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("Hover", true);
    
                    if (clickLeftMouse)
                    {
                        _animator.SetBool("Open",true);
                    }
                }
                else
                {
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Hover", false);
                }
            }
        }
    
        public void LootReward()
        {
            if (lootRewardPrefab != null)
            {
                var loot = Instantiate(lootRewardPrefab, transform.parent.position, transform.rotation);
                var lootFracture = Instantiate(lootFracturePrefab, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
            
        }
    }
}

