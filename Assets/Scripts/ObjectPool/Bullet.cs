using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Vector3 _Direction;

    [SerializeField]
    private float _Speed = 3f;

    // 오브젝트 풀
    private IObjectPool<Bullet> _ManagedPool;

    void Update()
    {
        transform.Translate(_Direction * Time.deltaTime * _Speed);
    }

    /// <summary>
    /// 현재 Bullet 프리팹의 ObjectPool을 할당
    /// </summary>
    /// <param name="pool"></param>
    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        _ManagedPool = pool;
    }

    /// <summary>
    /// 총알 발사
    /// </summary>    
    /// <param name="dir"> 발사 방향 </param>
    public void Shoot(Vector3 dir)
    {
        _Direction = dir;

        // 5초가 지난 후 총알을 파괴 => 3초 후에 풀에 반환
        //Destroy(gameObject, 3f);
        Invoke("DestroyBullet", 3f);
    }

    /// <summary>
    /// 총알 오브젝트를 풀에 반환
    /// </summary>
    public void DestroyBullet()
    {
        Debug.Log($"[Bullet/DestroyBullet] : 1"); 

        _ManagedPool.Release(this); // Pooling 종료 신호( 1번)
    }

}
