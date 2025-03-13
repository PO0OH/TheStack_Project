using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f; // 블럭 사이즈
    private const float MovingBoundsSize = 3f;  // 이동하는 블럭 양
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f; // 성공으로 취급할 마진

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2 (BoundSize, BoundSize);

    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;
    int comboCount = 0;

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    // Start is called before the first frame update
    void Start()
    {
        if(originBlock == null)
        {
            Debug.Log("OriginBlock is NULL");
            return;
        }

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;

        Spawn_Block();
        Spawn_Block();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (placeBlock())
            {
                Spawn_Block
            }
            Spawn_Block();
        }

        MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);    // original 블럭을 복제하여 반환

        if (newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform;
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        return true;
    }

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f)/255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer rn = go.GetComponent<Renderer>();

        if (rn == null)
        {
            Debug.Log("Renderer is NULL");
            return;
        }

        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }

    }

    void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        else
        {
            lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, movePosition * MovingBoundsSize);
        }

    }
}
