using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PieceMovement : MonoBehaviour
{
    private const float MAX_PLAYER_Y = 15f;
    public enum ChessType { bishop, knight, rook, queen };
    public ChessType chessType;

    private static Vector2Int[] knightMoves = { new Vector2Int(1, 2), new Vector2Int(2, 1), new Vector2Int(-1, 2), new Vector2Int(-2, 1), new Vector2Int(-1, -2), new Vector2Int(-2, -1), new Vector2Int(1, -2), new Vector2Int(2, -1) };
    private List<Vector2Int> availableKinghtMoves = new List<Vector2Int>();

    public ChessBoard board;
    public bool passive = false;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float waitTime = 1f, knightMoveTime = 1.5f, knightJumpHeight = 2.5f, queenDashLength = 5f;
    //[SerializeField][ColorUsage(true, true)] private Color angryColor = Color.red, angryColor2 = Color.red;

    [System.NonSerialized] public Vector3 targetPos;
    Vector3 originPos, kstartPos;
    Quaternion originRot;
    //Color originColor, originColor2;
    Transform player;
    Rigidbody rigid;
    AudioSource audios;
    //Material mat, mat2;
    [SerializeField] private ParticleSystem ps, ps2;
    [SerializeField] private MeshRenderer mrender;

    bool init = false, angry = false, knightRest = false;
    float wait = 0f, time = 0f;

    private void Awake()
    {
        originPos = Pos();
        originRot = mrender != null ? mrender.transform.rotation : transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        init = angry = false;
        /*
        if(mrender == null) mrender = gameObject.GetComponent<MeshRenderer>();
        if(mrender.sharedMaterials.Length > 1) mat = mrender.materials[1];
        mat2 = mrender.materials[0];
        if (mrender.sharedMaterials.Length > 1) originColor = mat.color;
        originColor2 = mat2.color;*/
        if(ps == null) ps = GetComponentInChildren<ParticleSystem>();
        if(chessType == ChessType.queen) rigid = GetComponent<Rigidbody>();
        audios = GetComponent<AudioSource>();
    }

    private void Start() {
        if (chessType != ChessType.queen) {
            SetPos(TilePosition(GetTile()));
            originPos = Pos();
        }

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = transform.parent;
    }

    private void Update() {
        //if (mrender.sharedMaterials.Length > 1) mat.color = Color.Lerp(mat.color, angry ? angryColor : originColor, Time.deltaTime * 4f);
        //mat2.color = Color.Lerp(mat2.color, angry ? angryColor2 : originColor2, Time.deltaTime * 4f);
        if (!init) {
            SelectTarget(true);
            init = true;
        }
        if (wait < waitTime) {
            wait += Time.deltaTime;
            if(chessType == ChessType.queen) WaitQueen();
            return;
        }
        time += Time.deltaTime;

        if (!ps.isStopped) {
            ps.Stop();
        }

        if (Move()) {
            if (!passive && chessType != ChessType.queen && HasPlayerTile()) {
                SelectFocusedTarget();
                angry = true;
            }
            else {
                SelectTarget();
                angry = false;
            }
            wait = 0f;
        }
    }

    //target when idle
    private void SelectTarget(bool init = false) {
        time = 0f;
        Vector2Int pos = new Vector2Int();
        if (chessType != ChessType.queen) {
            kstartPos = Pos();
            pos = GetTile();
        }

        switch (chessType) {
            case ChessType.rook:
                if(Random.value < 0.5f) {
                    //move x
                    targetPos = TilePosition(new Vector2Int(Random.Range(1, board.width + 1), pos.y));
                }
                else {
                    //move x
                    targetPos = TilePosition(new Vector2Int(pos.x, Random.Range(1, board.height + 1)));
                }
                SetPSLine(targetPos);
                break;
            case ChessType.bishop:
                if(Random.value < 0.5f) {
                    //move /
                    int min = -Mathf.Min(pos.x - 1, pos.y - 1);
                    int max = Mathf.Min(board.width - pos.x, board.height - pos.y);
                    int move = Random.Range(min, max + 1);
                    targetPos = TilePosition(new Vector2Int(pos.x + move, pos.y + move));
                }
                else {
                    //move \
                    int min = -Mathf.Min(pos.x - 1, board.height - pos.y);
                    int max = Mathf.Min(board.width - pos.x, pos.y - 1);
                    int move = Random.Range(min, max + 1);
                    targetPos = TilePosition(new Vector2Int(pos.x + move, pos.y - move));
                }
                SetPSLine(targetPos);
                break;
            case ChessType.knight:
                knightRest = false;
                if (Random.value < 0.3f) {
                    //rest
                    targetPos = Pos();
                    knightRest = true;
                }
                else {
                    //get available spots
                    availableKinghtMoves.Clear();
                    for (int i = 0; i < knightMoves.Length; i++) {
                        if (ValidMove(pos + knightMoves[i])) availableKinghtMoves.Add(knightMoves[i]);
                    }
                    if (availableKinghtMoves.Count > 0) {
                        targetPos = TilePosition(pos + availableKinghtMoves[Random.Range(0, availableKinghtMoves.Count)]);
                        SetPSCircle(targetPos);
                    }
                }
                break;
            case ChessType.queen:
                if (init) {
                    targetPos = originPos;
                    knightRest = true;
                    return;
                }
                knightRest = false;
                Vector3 v = (player.position - transform.position).normalized * (queenDashLength + Random.Range(0f, 3f));
                targetPos = transform.position + v - transform.parent.position;
                SetPSLine(targetPos);
                break;
        }
    }

    //target when the player is on board
    private void SelectFocusedTarget() {
        time = 0f;
        kstartPos = Pos();
        Vector2Int p = GetPlayerTile();
        switch (chessType) {
            case ChessType.rook:
                float rand = Random.value;
                if(rand < 0.4f) {
                    //follow x
                    targetPos = TilePosition(new Vector2Int(p.x, GetTile().y));
                    SetPSLine(targetPos);
                }
                else if(rand < 0.8f){
                    //follow z
                    targetPos = TilePosition(new Vector2Int(GetTile().x, p.y));
                    SetPSLine(targetPos);
                }
                else {
                    SelectTarget();
                }
                break;
            case ChessType.bishop:
                //i give the fuketh up
                SelectTarget();
                break;
            case ChessType.knight:
                knightRest = false;
                if (Random.value < 0.25f) {
                    SelectTarget();
                }
                else {
                    Vector2Int pos = GetTile();
                    int xm = p.x > pos.x ? 1 : -1;
                    int ym = p.y > pos.y ? 1 : -1;
                    Vector2Int m1 = new Vector2Int(xm * 2, ym * 1);
                    Vector2Int m2 = new Vector2Int(xm * 1, ym * 2);

                    if (ValidMove(pos + m1)) {
                        if (ValidMove(pos + m2)) {
                            //pick one
                            targetPos = TilePosition(pos + (Random.value < 0.5f ? m1 : m2));
                        }
                        else {
                            targetPos = TilePosition(pos + m1);
                        }
                        SetPSCircle(targetPos);
                    }
                    else if (ValidMove(pos + m2)) {
                        targetPos = TilePosition(pos + m2);
                        SetPSCircle(targetPos);
                    }
                    else {
                        SelectTarget();
                    }
                }
                break;
        }
    }

    //return true if the move ended
    private bool Move() {
        switch (chessType) {
            default:
                if (!audios.isPlaying) audios.Play();
                SetPos(Vector3.Lerp(Pos(), targetPos, moveSpeed * Time.deltaTime));
                if((Pos() - targetPos).sqrMagnitude < 0.0001f) {
                    SetPos(targetPos);
                    audios.Stop();
                    return true;
                }
                return false;
            case ChessType.knight:
                if (knightRest) return true;
                float f = Mathf.Clamp01(time / knightMoveTime);
                SetPos(Vector3.Lerp(kstartPos, targetPos, f) + Vector3.up * (1 - f) * f * 4f * knightJumpHeight);
                if(f > 0.999f) {
                    SetPos(targetPos);
                    audios.Play();
                    return true;
                }
                return false;
            case ChessType.queen:
                if(!ps2.isPlaying) ps2.Play();
                rigid.MovePosition(transform.parent.position + Vector3.Lerp(Pos(), targetPos, moveSpeed * Time.deltaTime));
                if ((Pos() - targetPos).sqrMagnitude < 0.0001f) {
                    SetPos(targetPos);
                    return true;
                }
                return false;
        }
    }

    private void WaitQueen() {
        //make waits shorter the higher the player is up
        wait += Time.deltaTime * Mathf.Clamp01(player.position.y / MAX_PLAYER_Y);
        if (!knightRest) {
            mrender.transform.rotation = Quaternion.Lerp(mrender.transform.rotation, Quaternion.LookRotation(-(targetPos + transform.parent.position - transform.position)), Time.deltaTime * 5f);
        }
    }

    private bool ValidMove(Vector2Int pos) {
        return pos.x >= 1 && pos.y >= 1 && pos.x <= board.width && pos.y <= board.height;
    }

    private void SetPSLine(Vector3 target) {
        float length = (target - Pos()).magnitude;
        if(length > 0.01f) ps.transform.forward = (target - Pos());
        var shape = ps.shape;
        shape.scale = new Vector3(0, 0, length);
        shape.position = new Vector3(0, 0, length / 2f);
        var ema = ps.emission;
        ema.rateOverTimeMultiplier = length * 20f;

        if (length > 0.01f && ps.isStopped) {
            ps.Play();
        }
    }

    private void SetPSCircle(Vector3 target) {
        target = target + transform.parent.position;
        ps.transform.position = new Vector3(target.x, ps.transform.position.y, target.z);

        if (ps.isStopped) {
            ps.Play();
        }
    }

    private Vector3 Pos() {
        return transform.position - transform.parent.position;
    }

    private Vector3 Pos(Transform t) {
        return t.position - transform.parent.position;
    }

    private void SetPos(Vector3 p) {
        transform.position = p + transform.parent.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReStart player = other.GetComponent<ReStart>();
            player.ReStartGame();
            KilledPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.transform.GetComponent<ReStart>().ReStartGame();
            KilledPlayer();
        }
    }

    private void KilledPlayer() {
        if (chessType == ChessType.queen) {
            SetPos(originPos);
            mrender.transform.rotation = originRot;
            time = wait = 0f;
            angry = false;
            init = false;
            SelectTarget(true);
        }
    }

    private Vector2Int GetPlayerTile() {
        return GetPlayerTile(player);
    }

    private Vector2Int GetTile() {
        return GetPlayerTile(transform);
    }

    private Vector2Int GetPlayerTile(Transform player) {
        return new Vector2Int(Mathf.Clamp(Mathf.CeilToInt(Map(player.position.x, board.start.position.x, board.end.position.x, 0, board.width)), 1, board.width),
            Mathf.Clamp(Mathf.CeilToInt(Map(player.position.z, board.start.position.z, board.end.position.z, 0, board.height)), 1, board.height));
    }

    private bool HasPlayerTile() {
        return player.position.x >= board.start.position.x && player.position.x <= board.end.position.x && player.position.z >= board.start.position.z && player.position.z <= board.end.position.z && player.position.y > board.start.position.y - 2f && player.position.y < board.start.position.y + 5f;    
    }

    private Vector3 TilePosition(Vector2Int tile) {
        float w = Mathf.Abs(board.start.position.x - board.end.position.x) / board.width;
        float h = Mathf.Abs(board.start.position.z - board.end.position.z) / board.height;
        var wp = new Vector3(board.start.position.x + w * (-0.5f + tile.x), originPos.y + transform.parent.position.y, board.start.position.z + h * (-0.5f + tile.y));
        return wp - transform.parent.position;
    }

    private float Map(float x, float a, float b, float c, float d) {
        return (x - a) / (b - a) * (d - c) + c;
    }


    [System.Serializable]
    public class ChessBoard {
        public Transform start, end;
        public int width = 1, height = 1;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (board.start == null || board.end == null || board.width <= 0 || board.height <= 0) return;
        float w = Mathf.Abs(board.start.position.x - board.end.position.x) / board.width;
        float h = Mathf.Abs(board.start.position.z - board.end.position.z) / board.height;

        Vector3 s = board.start.position;
        Vector3 e = board.end.position;
        Vector3 se = new Vector3(s.x, (s.y + e.y) / 2f, e.z);
        Vector3 es = new Vector3(e.x, (s.y + e.y) / 2f, s.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(s, se);
        Gizmos.DrawLine(se, e);
        Gizmos.DrawLine(e, es);
        Gizmos.DrawLine(es, s);

        Gizmos.color = Color.green;
        for (int i = 1; i < board.width; i++) {
            Gizmos.DrawLine(s + Vector3.right * w * i, se + Vector3.right * w * i);
        }
        for (int i = 1; i < board.height; i++) {
            Gizmos.DrawLine(s + Vector3.forward * h * i, es + Vector3.forward * h * i);
        }
    }
#endif
}
