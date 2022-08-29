using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PieceMovement : MonoBehaviour
{
    public enum ChessType { bishop, knight, rook };
    public ChessType chessType;

    public ChessBoard board;
    public bool passive = false;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private Color angryColor = Color.red, angryColor2 = Color.red;

    [System.NonSerialized] public Vector3 targetPos;
    Vector3 originPos;
    Color originColor, originColor2;
    Transform player;
    Material mat, mat2;
    ParticleSystem ps;
    [SerializeField] private MeshRenderer mrender;

    bool init = false, angry = false;
    float wait = 0f;

    private void Awake()
    {
        originPos = Pos();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        init = angry = false;
        mat = mrender.materials[1];
        mat2 = mrender.materials[0];
        originColor = mat.color;
        originColor2 = mat2.color;
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        SetPos(TilePosition(GetTile()));
        originPos = Pos();

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = transform.parent;
    }

    private void Update() {
        mat.color = Color.Lerp(mat.color, angry ? angryColor : originColor, Time.deltaTime * 4f);
        mat2.color = Color.Lerp(mat2.color, angry ? angryColor2 : originColor2, Time.deltaTime * 4f);
        if(wait < waitTime) {
            wait += Time.deltaTime;
            return;
        }
        if (!ps.isStopped) {
            ps.Stop();
        }
        if (!init) {
            SelectTarget();
            init = true;
        }

        if (Move()) {
            if (!passive && HasPlayerTile()) {
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
    private void SelectTarget() {
        Vector2Int pos = GetTile();
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
                break;
        }
    }

    //target when the player is on board
    private void SelectFocusedTarget() {
        Vector2Int p = GetPlayerTile();
        switch (chessType) {
            case ChessType.rook:
                float rand = Random.value;
                if(rand < 0.35f) {
                    //follow x
                    targetPos = TilePosition(new Vector2Int(p.x, GetTile().y));
                    SetPSLine(targetPos);
                }
                else if(rand < 0.7f){
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
                break;
        }
    }

    //return true if the move ended
    private bool Move() {
        switch (chessType) {
            default:
                SetPos(Vector3.Lerp(Pos(), targetPos, moveSpeed * Time.deltaTime));
                if((Pos() - targetPos).sqrMagnitude < 0.0001f) {
                    SetPos(targetPos);
                    return true;
                }
                return false;
            case ChessType.knight:
                return false;
        }
    }

    private void SetPSLine(Vector3 target) {
        float length = (target - Pos()).magnitude;
        if(length > 0.01f) ps.transform.forward = (target - Pos());
        var shape = ps.shape;
        shape.scale = new Vector3(0, 0, length);
        shape.position = new Vector3(0, 0, length / 2f);

        if (length > 0.01f && ps.isStopped) {
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
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.transform.GetComponent<ReStart>().ReStartGame();
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
        return player.position.x >= board.start.position.x && player.position.x <= board.end.position.x && player.position.z >= board.start.position.z && player.position.z <= board.end.position.z;    
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

    /*
    IEnumerator RockMoveMent(int x, int y)
    {
        endState = false;
        changePos = this.transform.position;
        while (!endState)
        {
            if (x == 1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(1, 0, 0), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(1, 0, 0))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(0, 0, 1), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(0, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == -1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-1, 0, 0), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(-1, 0, 0))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == -1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(0, 0, -1), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(0, 0, -1))
                {
                    endState = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPos, speed * Time.deltaTime);
                if (transform.position == originPos)
                {
                    endState = true;
                }
            }
            yield return null;
        }

        chessX = Random.Range(-1, 2);
        chessY = Random.Range(-1, 2);

        minusX += chessX;
        minusY += chessY;

        if (minusX == 2)
        {
            chessX -= minusX;
        }

        if (minusY == 2)
        {
            chessY -= minusY;
        }
        if (minusX == -2)
        {
            chessX += minusX;
        }
        if (minusY == -2)
        {
            chessY += minusY;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(RockMoveMent(chessX, chessY));
    }

    IEnumerator BiShopMoveMent(int x, int y)
    {
        endState = false;
        changePos = this.transform.position;
        while (!endState)
        {
            if (x == 1 && y == 0)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(1, 0, 1), speed * Time.deltaTime);
                if(transform.position == changePos + new Vector3(1, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == 2 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-1, 0, 1), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(-1, 0, 1))
                {
                    endState = true;
                }
            }
            else if (x == 1 && y == 2)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-1, 0, -1), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(-1, 0, -1))
                {
                    endState = true;
                }
            }
            else if (x == 0 && y == 1)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(1, 0, -1), speed * Time.deltaTime);
                if (transform.position == changePos + new Vector3(1, 0, -1))
                {
                    endState = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPos, speed * Time.deltaTime);
                if (transform.position == originPos)
                {
                    endState = true;
                }
            }
            yield return null;
        }
        
        chessX = Random.Range(1, 3);
        chessY = Random.Range(0, 3);

        minusX += chessX;
        minusY += chessY;

        if(minusX == 3)
        {
            chessX -= minusX + 1;
        }

        if (minusY == 4)
        {
            chessY -= minusY + 2;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(BiShopMoveMent(chessX, chessY));
    }

    IEnumerator KnightMoveMent(int x, int y)
    {
        endState = false;
        changePos = this.transform.position;
        while (!endState)
        {
            if (x == -1)
            {
                if(y == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if(y == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (x == 1)
            {
                if (y == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if (y == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (y == -1)
            {
                if (x == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, -1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, -1))
                    {
                        endState = true;
                    }
                }
                else if (x == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-2, 0, 1), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-2, 0, 1))
                    {
                        endState = true;
                    }
                }
            }
            else if (y == 1)
            {
                if (x == -1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(-1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(-1, 0, 2))
                    {
                        endState = true;
                    }
                }
                else if (x == 1)
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(1, 0, 2))
                    {
                        endState = true;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, changePos + new Vector3(1, 0, 2), speed * Time.deltaTime);
                    if (transform.position == changePos + new Vector3(1, 0, 2))
                    {
                        endState = true;
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, originPos, speed * Time.deltaTime);
                if (transform.position == originPos)
                {
                    endState = true;
                }
            }
            yield return null;
        }

        chessX = Random.Range(-1, 2);
        chessY = Random.Range(-1, 2);
        
        //minusX += chessX;
        //minusY += chessY;

        //if (minusX == 2)
        //{
        //    chessX -= minusX;
        //}

        //if (minusY == 2)
        //{
        //    chessY -= minusY;
        //}
        //if (minusY == -2)
        //{
        //    chessX += minusY;
        //}

        //if (minusX == -2)
        //{
        //    chessY += minusX;
        //}

        yield return new WaitForSeconds(1f);
        StartCoroutine(KnightMoveMent(chessX, chessY));
    }*/


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
