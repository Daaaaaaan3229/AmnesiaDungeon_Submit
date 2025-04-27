using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //変数宣言
    #region
    Rigidbody rb;
    CapsuleCollider cc;
    PlayerStatusControl statusControl;
    PlayerStatus ps;        //PlayerStatus RW
    PlayerStatus ps_max;    //PlayerStatus R
    Transform trans;

    [SerializeField]
    [Header("プレイヤーのカメラ")]
    GameObject cameraObj;
    Transform cameraObj_trans;

    [SerializeField]
    [Header("回避時の消費スタミナ")]
    private float decreace_st;

    Vector3 target_dir;

    [SerializeField]
    [Header("プレイヤーの回転速度のスムーズさ")]
    [Range(1, 100)]
    float smooth;

    //キー入力の状態管理用
    const int up = 0;
    const int down = 1;
    const int left = 2;
    const int right = 3;

    List<int> inputs = new List<int>();

    //どのキーが押されているかの判定用
    bool _up;
    bool _down;
    bool _left;
    bool _right;

    Animator anim;

    //----------------------------プレイヤーの移動関連のパラメーター----------------------------
    #region
    [SerializeField]
    [Header("プレイヤーの歩行スピード")]
    [Range(1, 100)]
    int walk_speed;

    [SerializeField]
    [Header("プレイヤーの走行スピードの倍率")]
    [Range(1, 100)]
    float run_rate;

    public static PlayerMove instance;
    bool _move;     //移動可能かどうか。TRUE:移動可能  FALSE:移動不可


    //スタミナ管理用関数に渡す用
    const int increace = 1;
    const int decreace = -1;

    float speed;      //プレイヤーの移動スピード
    float locom_mode; //0:歩行 1:走行 BlendTree設定用

    [SerializeField]
    [Header("重力")]
    int gravity;

    float xspeed;
    float zspeed;

    bool _jump;//ジャンプの入力判定。True:ジャンプ それ以外:False
    bool _isAirAttack;//空中での攻撃判定。True:空中での攻撃 それ以外:False
    bool _isAttacktime;//攻撃判定。True:攻撃中 それ以外:False
    bool _run;

    int attack_count;//攻撃の回数をカウント。0:初回攻撃 1:2回目の攻撃 2:3回目の攻撃


    [SerializeField]
    [Header("攻撃の持続時間")]
    float attack_time;
    float time;//時間計測用

    //地面判定用のレイヤーマスク
    [SerializeField]
    LayerMask ground;
    Vector3 rayPosition;
    Ray ray;
    #endregion
    //------------------------------------------------------------------------------------------

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        ResetMove();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        statusControl = PlayerStatusControl.instance;
        ps = statusControl.GetPlayerStatusSO("current");
        ps_max = statusControl.GetPlayerStatusSO("max");
        cameraObj_trans = cameraObj.transform;
        trans = transform;
        speed = walk_speed;

        _jump = false;
        _move = true;

        locom_mode = -1;
        anim.SetFloat("Mode", locom_mode);
    }

    /// <summary>
    /// Inputの入力
    /// </summary>
    void Update()
    {
        //キーが押されたらTrue、離されたらFalse
        //押された瞬間にキーをリストに追加して管理するため、リストに追加された状態で保持している
        if (Input.GetKeyDown(KeyCode.A)) PushInputs(left);
        if (Input.GetKeyDown(KeyCode.D)) PushInputs(right);
        if (Input.GetKeyDown(KeyCode.W)) PushInputs(up);
        if (Input.GetKeyDown(KeyCode.S)) PushInputs(down);

        if (Input.GetKeyUp(KeyCode.A)) PullInputs(left);
        if (Input.GetKeyUp(KeyCode.D)) PullInputs(right);
        if (Input.GetKeyUp(KeyCode.W)) PullInputs(up);
        if (Input.GetKeyUp(KeyCode.S)) PullInputs(down);

        //押されているキーに応じて移動方向を決定
        if (_left && SearchElement(left) > SearchElement(right)) xspeed = -1;
        if (_right && SearchElement(left) < SearchElement(right)) xspeed = 1;
        if (_up && SearchElement(up) > SearchElement(down)) zspeed = 1;
        if (_down && SearchElement(up) < SearchElement(down)) zspeed = -1;

        if (!_left && !_right || !_move) xspeed = 0;
        if (!_up && !_down || !_move) zspeed = 0;

        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Brink") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && !anim.GetCurrentAnimatorStateInfo(0).IsName("AroundAttack"))//アニメーション中でない場合
        {
            if (speed == walk_speed && (_left || _right || _up || _down))//歩行中のキー入力がある場合、スピードを歩行速度に設定
            {
                locom_mode = 0;
                anim.SetFloat("Mode", locom_mode);
            }
            else if (speed != walk_speed && (_left || _right || _up || _down))//走行中のキー入力がある場合、スピードを走行速度に設定
            {
                locom_mode = 1;
                anim.SetFloat("Mode", locom_mode);
            }
            else //それ以外の場合は待機アニメーションの設定
            {
                locom_mode = -1;
                anim.SetFloat("Mode", locom_mode);
                anim.SetTrigger("Stay");
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && statusControl.GetPlayerStatusSO("current").Get("st") > 0.1) //左Shift(走行) 回避
        {
            speed = walk_speed * run_rate;
            _run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || statusControl.GetPlayerStatusSO("current").Get("st") <= 0.1) //左Shift(走行)
        {
            speed = walk_speed;
            _run = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && statusControl.GetPlayerStatusSO("current").Get("st") >= Mathf.Abs(decreace_st)) //左Ctrl 回避 スタミナ消費より多かった場合
        {
            if ((_left || _right || _up || _down) && GroundCheck() && !anim.GetCurrentAnimatorStateInfo(0).IsName("Brink"))
            {
                anim.SetTrigger("Roll");
                statusControl.StatusCalculate("st", decreace_st);
            }
        }

        if (Input.GetKeyDown("space") && GroundCheck()) _jump = true; //ジャンプ

        if (Input.GetMouseButtonDown(0) && _move) //左クリック 攻撃
        {
            if (GroundCheck() && !anim.GetCurrentAnimatorStateInfo(2).IsName("AroundAttack")) //空中でない場合
            {
                if (attack_count == 0 && !anim.GetCurrentAnimatorStateInfo(2).IsName("Attack")) //初回攻撃 attack_countが0の場合
                {
                    _isAttacktime = true;
                    anim.SetFloat("Attack_Count", attack_count);
                    anim.SetTrigger("Attack");
                    attack_count++;
                }
                else if (attack_count == 1 && !anim.GetCurrentAnimatorStateInfo(2).IsName("Attack")) //2回目の攻撃 attack_countが1の場合
                {
                    time = 0;
                    anim.SetFloat("Attack_Count", attack_count);
                    anim.SetTrigger("Attack");
                    attack_count++;
                }
                else if (attack_count == 2 && !anim.GetCurrentAnimatorStateInfo(2).IsName("Attack"))//3回目の攻撃 attack_countが2の場合
                {
                    anim.SetFloat("Attack_Count", attack_count);
                    anim.SetTrigger("Attack");
                    attack_count = 0;
                }
            }
            else if (!GroundCheck())//空中の場合
            {
                if (!_isAirAttack)
                {
                    _isAirAttack = true;
                    anim.SetTrigger("AroundAttack");
                }
            }
        }

        if (GroundCheck()) _isAirAttack = false;


        if (_isAttacktime)
        {
            time += Time.deltaTime;
        }

        if (_run)
        {
            statusControl.StaminaCalculate(decreace);
        }
        else if (ps.Get("st") != ps_max.Get("st"))
        {
            statusControl.StaminaCalculate(increace);
        }

        if (time > attack_time)//3回目の攻撃の持続時間が終了したらリセット
        {
            _isAttacktime = false;
            attack_count = 0;
            time = 0;
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Brink") && statusControl.GetInvincible() == true)
        {
            statusControl.SetInvincible(0);
        }
    }

    /// <summary>
    /// RigidBodyなどの物理演算
    /// </summary>
    void FixedUpdate()
    {
        if (!_jump) Gravity();
        Rotate();
        //入力がない場合は速度を0に。入力がある場合は移動方向に速度を設定
        if (!_left && !_right && !_up && !_down) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        else
        {
            rb.velocity = target_dir.normalized * speed + trans.up * rb.velocity.y;
        }

        if (_jump && GroundCheck())
        {
            anim.SetTrigger("Jump");
            _jump = false;
        }
    }

    //関数宣言
    #region

    /// <summary>
    /// 移動キー入力のリセット
    /// </summary>
    void ResetMove()
    {
        _up = false;
        _down = false;
        _left = false;
        _right = false;
    }

    /// <summary>
    /// プレイヤーの回転
    /// </summary>
    void Rotate()
    {
        //プレイヤーのカメラ
        var rotate = Quaternion.AngleAxis(cameraObj_trans.localEulerAngles.y, Vector3.up);
        target_dir = rotate * (new Vector3(xspeed, 0, zspeed)).normalized;

        //回転処理
        //プレイヤーのベクトルの大きさが0.1より大きい場合プレイヤーを回転
        //移動中の回転処理
        if (target_dir.magnitude > 0.1)
        {
            Quaternion rotation = Quaternion.LookRotation(target_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);//( a, b, t)aからbの間をtで補間
        }

        else if (target_dir == Vector3.zero)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    /// <summary>
    /// 入力されたキーをリストに追加して管理
    /// </summary>
    /// <param name="dir_num">リストに追加するキーの番号</param>
    void PushInputs(int dir_num)
    {
        if (dir_num == left) _left = true;
        else if (dir_num == right) _right = true;
        else if (dir_num == up) _up = true;
        else if (dir_num == down) _down = true;
        inputs.Add(dir_num);
    }

    /// <summary>
    /// 入力が離されたキーをリストから削除
    /// </summary>
    /// <param name="dir_num">リストから削除するキーの番号</param>
    void PullInputs(int dir_num)
    {
        if (dir_num == left) _left = false;
        else if (dir_num == right) _right = false;
        else if (dir_num == up) _up = false;
        else if (dir_num == down) _down = false;
        inputs.Remove(dir_num);
    }

    /// <summary>
    /// リストの中から指定されたキーのインデックスを検索
    /// </summary>
    /// <param name="target">検索するキー</param>
    /// <returns>見つかった場合：キーのインデックス 見つからなかった場合：-1</returns>
    int SearchElement(int target)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i] == target)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 地面判定
    /// </summary>
    /// <returns>地面に接している場合：True 接していない場合：False</returns>
    bool GroundCheck()
    {
        rayPosition = new Vector3(trans.position.x, trans.position.y + 0.1f, trans.position.z);
        ray = new Ray(rayPosition, Vector3.down);
        return Physics.Raycast(ray, 0.15f, ground);
    }

    /// <summary>
    /// 重力を加える
    /// </summary>
    void Gravity()
    {
        rb.AddForce(0, gravity, 0);
    }

    public bool GetMove()
    {
        return _move;
    }

    public void SetMove(bool set)
    {
        _move = set;
    }

    #endregion
}
