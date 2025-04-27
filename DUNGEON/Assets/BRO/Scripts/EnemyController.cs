using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour, IDamagable
{
    #region インスペクターで設定する 
    [SerializeField] [Header("攻撃判定オブジェクト")] protected GameObject AttackObject;
    [SerializeField] [Header("攻撃を当てたときのエフェクト")] protected GameObject AttackEffect;
    [SerializeField, Range(0.1f, 50.0f)] [Header("ランダムな移動位置を取る半径")] float randomRadius;
    [SerializeField] [Header("ダメージ音")] public AudioClip damageSound;
    [SerializeField] [Header("気絶音")] public AudioClip deadSound;
    [SerializeField] [Header("攻撃音")] public AudioClip attackSound;
    #endregion

    #region プライベート変数 
    //protected GameObject currentTarget;
    protected GameObject playerObj;//プレイヤーオブジェクト、生成時にSetPlayerObject()で渡してもらう
    protected IDamagable iDamagable;//IDamagebleコンポネント、生成時に取得しておく
    protected DropItemScript dropItemScript;
    protected Animator anim = null;
    protected bool isRun;
    protected bool isAttack;
    protected bool isinvincible=false;
    protected Rigidbody rb;
    protected CapsuleCollider cc;
    protected AudioSource audioSource;
    public string name;
    public float hp;
    public float atk;
    public float def;

    protected int floorNum;//階層

    protected Vector3 destinationPos;//移動先の座標
    #endregion

    [SerializeField] protected EnemyStatus enemyStatus;
    protected NavMeshAgent m_navMeshAgent; // NavMeshAgent 

    // 敵のリストを保持するための参照
    public static List<GameObject> activeEnemies = new List<GameObject>();

    protected void Initialize()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dropItemScript = GetComponent<DropItemScript>();
        cc = GetComponent<CapsuleCollider>();

        //ステータスの取得 
        name = enemyStatus.EnemyName;
        hp = enemyStatus.Hp * (1 + Mathf.Pow(floorNum,1.1f) );
        atk = enemyStatus.Atk * (1 + Mathf.Pow(floorNum,1.1f));
        def = enemyStatus.Def;

        //IDamagebleの取得
        iDamagable = playerObj.GetComponent<IDamagable>();

        //目的地に向かわせる
        SetDestination();
    }

    public void SetTarget()
    {
        isRun = true;
    }

    public void ResetTarget()
    {
        //元の目的地に向かわせる
        m_navMeshAgent.SetDestination(destinationPos);
        isRun = false;
    }

    public void Attack()
    {
        m_navMeshAgent.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;
        isAttack = true;
    }

    public void StopAttack()
    {

        m_navMeshAgent.isStopped = false;
        rb.constraints = RigidbodyConstraints.None;
        isAttack = false;
    }

    //アニメーションの設定 
    protected void SetAnimation()
    {
        anim.SetBool("Attack", isAttack);
        anim.SetBool("Run", isRun);
    }

    /// <summary> 
    /// アニメーションに同期して呼び出す 
    /// </summary> 
    protected void StartAttackDetection()
    {
        Debug.Log("attack.");
        AttackObject.SetActive(true);
    }

    protected void StopAttackDetection()
    {
        Debug.Log("attack stop.");
        AttackObject.SetActive(false);
    }

    /// <summary> 
    /// atkをインターフェースに与える 
    /// </summary> 
    public virtual void AddAtk()
    {
        //プレイヤーにダメージを与える
        iDamagable.AddDamage(atk);
        audioSource.PlayOneShot(attackSound);
        AttackEffect.SetActive(true);
        Debug.Log(name + "はプレイヤーに" + atk + "ダメージ与えた");
    }

    /// <summary>
    /// アニメーショントリガーで、Dieアニメーション後に呼び出す
    /// </summary>
    protected virtual void Die()
    {
        //移動を止める
        m_navMeshAgent.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //アイテムをドロップする
        dropItemScript.Drop();
        //オブジェクトを消す
        Destroy(this.gameObject, 0.4f);
    }

    /// <summary>
    /// 目的地を設定し、向かわせる
    /// </summary>
    protected void SetDestination()
    {
        NavMeshHit hit;

        //ランダムなポジションを取得し、navMeshで移動可能かを判定
        do
        {
            //円の中からランダムなポジションを取得
            //自分自身の位置から円の範囲
            destinationPos = this.gameObject.transform.position + Random.insideUnitSphere * randomRadius;
        } 
        while (NavMesh.SamplePosition(destinationPos, out hit, 0.1f, UnityEngine.AI.NavMesh.AllAreas));

        m_navMeshAgent.SetDestination(destinationPos);
    }

    /// <summary>
    /// 敵が目的地に達成したかどうかを判定する  true:到達した false:到達していない
    /// </summary>
    /// <returns></returns>
    protected bool ReachDestination()
    {
        // エージェントが目的地に到達したかどうかの確認
        if (!m_navMeshAgent.pathPending) // パスがまだ計算中でないか
        {
            if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance) // 残りの距離が停止距離以下かどうか
            {
                if (!m_navMeshAgent.hasPath || m_navMeshAgent.velocity.sqrMagnitude == 0f)// パスがない、もしくはエージェントが停止している場合
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 敵生成時にプレイヤー、階層を取得する
    /// 階層毎にステータスをパワーアップする
    /// </summary>
    /// <param name="getObject"></param>
    public void SetParameter(GameObject getObject, int num)
    {
        playerObj = getObject;

        floorNum = num;
    }

    //ダメージを受ける 
    public virtual void AddDamage(float damage)
    {
        if (isinvincible) return;
        hp -= damage;

        Debug.Log("敵は" + damage + "ダメージ受けた");

        if (hp <= 0)
        {
            //判定を消す
            cc.enabled = false;

            anim.Play("Die");
            Debug.Log("Enemyを倒した");
            audioSource.PlayOneShot(deadSound);
            isinvincible = true;
        }
        else
        {
            audioSource.PlayOneShot(damageSound);
        }
        
    }

    /// <summary>
    /// このオブジェクトが破棄された時の処理
    /// </summary>
    private void OnDestroy()
    {
        // 敵が破棄されるときにリストから削除
        if (activeEnemies.Contains(this.gameObject))
        {
            activeEnemies.Remove(this.gameObject);
        }
    }
}
