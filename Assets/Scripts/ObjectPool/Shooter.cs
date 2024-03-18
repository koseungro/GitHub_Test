using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject _BulletPrefab;
    public Transform objectPool;
    public int poolSize;

    private Camera _MainCam;

    private IObjectPool<Bullet> _Pool;

    private Transform _tr;

    private void Awake()
    {
        // 생성, 가져오기, 반환, 삭제, Pool Size)
        _Pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:poolSize);
    }

    void Start()
    {
        _MainCam = Camera.main;
    } 

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = _MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitResult;
            if(Physics.Raycast(ray, out hitResult))
            {
                //Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 5f);

                var direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position; // 총알이 나갈 방향(hit 위치 - Shooter.position 위치)

                // 총알을 새로 생성해서 사용
                //var bullet = Instantiate(_BulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();

                var bullet = _Pool.Get(); // Pool에 오브젝트들이 없으면 => Create, 있으면 => Get
                bullet.transform.position = transform.position + direction.normalized; // 처음 총알 위치 설정 + 1
                bullet.Shoot(direction.normalized);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"<color=cyan> Space </color>");
            _tr = this.transform.GetComponent<Transform>();
        }
    }

    /// <summary>
    /// 총알 생성 후 생성된 bullet 오브젝트에 자신이 등록되어야 할 풀을 알려주고 총알 반환
    /// </summary>
    private Bullet CreateBullet()
    {
        //Debug.Log($"<color=yellow>[Shooter/CreateBullet]</color>");

        // 이미 생성되어있는 오브젝트가 없을 때(맨 처음 발사)
        Bullet bullet = Instantiate(_BulletPrefab,objectPool).GetComponent<Bullet>();
        bullet.SetManagedPool(_Pool);

        //bullet.transform.SetParent(objectPool);
        return bullet;
    }

    /// <summary>
    /// 풀에서 오브젝트를 빌려올 때 사용(이미 생성되어있는 오브젝트가 있을 때)
    /// </summary>
    private void OnGetBullet(Bullet bullet)
    {
        //Debug.Log($"<color=magente>[Shooter/OnGetBullet]</color>");

        bullet.gameObject.SetActive(true);
    }

    /// <summary>
    /// 오브젝트를 풀에 반환하고 비활성화
    /// </summary>
    private void OnReleaseBullet(Bullet bullet)
    {
        Debug.Log($"[Shooter/OnReleaseBullet] : 2");
        bullet.gameObject.SetActive(false);
    }

    /// <summary>
    /// 풀에서 Maxsize를 넘는 오브젝트 갯수에 대해 오브젝트를 파괴
    /// </summary>
    /// <param name="bullet"></param>
    private void OnDestroyBullet(Bullet bullet)
    {
        Debug.Log($"[Shooter/OnDestroyBullet] : 3");

        Destroy(bullet.gameObject);
    }

}
