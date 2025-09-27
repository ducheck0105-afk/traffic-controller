using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _0.Custom.Scripts;
using _0.Custom.Scripts.Gameplay;
using Random = UnityEngine.Random;

public class CarAI : MonoBehaviour
{
    public enum CarFlow
    {
        Main,
        Sub
    }

    [Header("Luồng xe")] public CarFlow flow = CarFlow.Main;

    [Header("Đường đi")] public List<Transform> waypoints;

    [Tooltip("Khoảng cách coi như đã tới point để chuyển sang point kế tiếp")]
    public float arriveDistance = 0.5f;

    [Tooltip("Có lặp lại đường đi không")] public bool loopPath = true;

    [Header("Di chuyển")] private float maxMoveSpeed = 7f; // m/s
    public float acceleration = 10f; // m/s^2
    public float deceleration = 12f; // m/s^2
    public float rotationSpeed = 6f; // tốc độ xoay hướng (lerp)

    [Header("Nhìn phía trước (chỉ áp dụng cho luồng chính)")]
    [Tooltip("Khoảng nhìn phía trước để phát hiện xe luồng chính đứng chắn")]
    public float lookAheadDistance = 4f;

    [Tooltip("Bán kính phát hiện để forgiving hơn Ray")]
    public float lookAheadRadius = 0.5f;

    public LayerMask carLayer; // Layer của xe

    public ParticleSystem smoke;
    private Rigidbody rb;
    private int wpIndex = 0;
    public float currentSpeed = 0f;

    private bool stop;
    private bool claimReward;
    private readonly HashSet<Junction> zones = new HashSet<Junction>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        if (waypoints == null || waypoints.Count == 0)
            Debug.LogWarning($"{name}: Chưa gán waypoints!");
    }
    
    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Transform target = waypoints[wpIndex];
        Vector3 toTarget = (target.position - rb.position);
        Vector3 dir = toTarget.normalized;

        // Xoay mượt về hướng di chuyển
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion desired = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, desired, rotationSpeed * Time.fixedDeltaTime));
        }

        // Quyết định có được đi hay phải dừng
        bool shouldStop = ShouldStop();

        // Tính tốc độ
        if (!shouldStop)
            currentSpeed += acceleration * Time.fixedDeltaTime;
        else
            currentSpeed -= deceleration * Time.fixedDeltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxMoveSpeed);
        currentSpeed *= flow == CarFlow.Main ? GameController.instance.booster : 1;
        // Di chuyển theo Rigidbody
        Vector3 step = dir * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + step);

        // Đến gần point thì chuyển point
        if (toTarget.magnitude <= arriveDistance)
        {
            NextWaypointOrFinish();
        }
        
        {
            if (smoke != null)
            {
                bool braking =  currentSpeed is > 0.1f and < 7;
                // bool fast = currentSpeed > 5f && currentSpeed < 7f;
                if (braking && smoke.isPlaying)
                {
                    smoke.Play();
                }
                //smoke.gameObject.SetActive(braking);
            }
            
            // if (!isPlay && ( braking))
            // {
            //     Debug.Log("smoke");
            //     isPlay = true;
            //     smoke.gameObject.SetActive(true);
            // }
            // else if (!braking && isPlay)
            // {
            //     Debug.Log("normal");
            //     smoke.gameObject.SetActive(false);
            //     isPlay = false;
            //     // anim.Play("Move");
            // }
        }
        
    }

    private bool isPlay = false;
    bool ShouldStop()
    {
        if (flow == CarFlow.Sub)
            return false;

        foreach (var z in zones)
        {
            if (z != null && z.IsRed) return true;
        }

        // 2) Có xe luồng chính đứng trước mặt trong tầm nhìn?
        if (DetectMainCarAhead()) return true;

        return false;
    }

    bool DetectMainCarAhead()
    {
        Vector3 origin = rb.position + Vector3.up * 0.25f; // nhích cao chút cho sạch va chạm mặt đất
        Vector3 forward = transform.forward;

        // SphereCast forgiving hơn Ray, giúp bắt xe hơi lệch hướng
        if (Physics.SphereCast(origin, lookAheadRadius, forward, out RaycastHit hit, lookAheadDistance, carLayer,
                QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.TryGetComponent<CarAI>(out var other))
            {
                // Chỉ dừng khi xe trước là luồng chính và đang gần như đứng (chắn đường)
                if (other.flow == CarFlow.Main)
                {
                    // Nếu xe trước đang chạy nhưng chậm, vẫn coi là cản
                    return true;
                }
            }
        }

        return false;
    }

    void NextWaypointOrFinish()
    {
        if (wpIndex < waypoints.Count - 1)
        {
            wpIndex++;
        }
        else
        {
            if (loopPath)
                wpIndex = 0;
            else
                enabled = false; // hết đường thì dừng script
        }
    }

    /// <summary>
    /// Thử cộng điểm tại một waypoint cụ thể.
    /// Chỉ xe luồng chính (Main) mới được xét thưởng.
    /// </summary>
  
    /// <summary>
    /// Khi xe bước vào một collider có TrafficZone,
    /// thêm zone đó vào tập 'zones' để biết hiện đang đứng trong các vùng giao thông nào.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // TryGetComponent an toàn & nhanh hơn GetComponent + null-check.
        if (other.TryGetComponent<Junction>(out var z))
            zones.Add(z); // HashSet nên không lo bị thêm trùng.
    }

    /// <summary>
    /// Khi xe rời khỏi một collider có TrafficZone,
    /// loại zone đó khỏi tập 'zones'.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Junction>(out var z))
            zones.Remove(z);
    }

    /// <summary>
    /// Xử lý va chạm vật lý (collider không phải trigger).
    /// Nếu hai xe khác luồng (Main vs Sub) đụng nhau → thua.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra đối tượng va chạm có phải CarAgent không.
        if (collision.transform.TryGetComponent<CarAI>(out var other))
        {
            // Điều kiện đúng khi một xe là Main và xe kia là Sub (theo cả hai chiều).
            bool oneMainOneSub =
                (this.flow == CarFlow.Main && other.flow == CarFlow.Sub) ||
                (this.flow == CarFlow.Sub && other.flow == CarFlow.Main);
            // if(flow == CarFlow.Sub) return;
            var rb = GetComponent<Rigidbody>();
            if (rb == null) return;
            // normal hướng từ other -> self tại điểm va chạm
            var cp = collision.GetContact(0);
            Vector3 n = cp.normal;

            // Dùng xung va chạm do Unity tính sẵn (đã là impulse)
            float J = collision.impulse.magnitude;

            // Tạo mô-men xoắn theo cạnh “bật” xe khỏi normal
            float toppleFactor = 0.02f; // tune theo game
            Vector3 torque = Vector3.Cross(transform.up, n) * J * toppleFactor;

            // Bỏ khoá rotation nếu lỡ khoá
            rb.constraints &= ~(RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ);

            rb.AddTorque(torque, ForceMode.Impulse);
            // Nếu khác luồng → gọi GameOver(false).
            if (oneMainOneSub)
            {
                // stop = true;
                // other.stop = true;
                if (flow == CarFlow.Main)
                {
                   var particle =  Instantiate(CarData.Instance.carExplode);
                   particle.transform.position = transform.position;
                }
                GameController.instance.GameOver(false);
            }
        }
       
    }
  

    /// <summary>
    /// Vẽ gizmos để debug tầm nhìn phía trước (SphereCast):
    /// - Một đường thẳng từ vị trí xe tới khoảng cách lookAheadDistance.
    /// - Một wire sphere tại điểm cuối để minh họa bán kính kiểm tra (lookAheadRadius).
    /// Lưu ý: Gizmos chỉ hiển thị trong Scene view, không ảnh hưởng gameplay.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Origin: ưu tiên dùng Rigidbody.position khi đang Play để khớp physics, 
        // nếu không có rb thì dùng transform.position.
        Vector3 origin = (Application.isPlaying ? rb?.position ?? transform.position : transform.position)
                         + Vector3.up * 0.25f; // nhích lên chút để dễ nhìn

        // Vẽ quả cầu tại điểm cuối phạm vi "nhìn" phía trước.
        Gizmos.DrawWireSphere(origin + transform.forward * lookAheadDistance, lookAheadRadius);

        // Vẽ đường thẳng biểu diễn hướng và tầm nhìn.
        Gizmos.DrawLine(origin, origin + transform.forward * lookAheadDistance);
    }
}