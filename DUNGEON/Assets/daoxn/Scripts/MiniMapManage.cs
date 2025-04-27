using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManage : MonoBehaviour
{
    private int[,] map;//�}�b�v�̓񎟌��z��B�}�b�v�^�C���̑����𔻒肷��̂Ɏg���B

    [SerializeField]
    [Header("�~�j�}�b�v���i�[����e�I�u�W�F�N�g")]
    private GameObject mapCanvasObj;

    [SerializeField]
    [Header("�~�j�}�b�v�^�C���̃v���n�u")]
    private GameObject mapTilePrefab;

    [SerializeField]
    [Header("�������i�[����e�̃v���n�u")]
    private GameObject roomParentPrefab;

    [SerializeField]
    [Header("�v���C���[�I�u�W�F�N�g")]
    private GameObject playerObject;

    [SerializeField]
    [Header("�}�b�v��̃v���C���[�̈ʒu")]
    private GameObject playerPoint;

    private Vector3 playerPos;//�v���C���[�̌��ݒn

    private RectTransform playerPointPos;//�v���C���[�|�C���g�̈ʒu

    [SerializeField]
    [Header("�}�b�v��̊K�i�̈ʒu")]
    private GameObject stairPoint;

    [SerializeField]
    [Header("�~�j�}�b�v�ʒu�������l")]
    private int slideValueX;
    [SerializeField]
    private int slideValueY;


    private List<Vector2> roomEnd = new List<Vector2>();//�e�����̒[�̍��W��ۑ�

    private List<GameObject> roomParentList = new List<GameObject>();//�����̐e�̃��X�g

    private bool checkRoom;//�O�̍��W���������ǂ����𔻒� true:���� false:��������Ȃ�

    private int[] previousRoomNum;//�O�̐��l���i�[����z��

    private int roomParentNum;//�i�[���镔���̔ԍ�

    private Canvas miniMapCanvas;//���̃I�u�W�F�N�g��Canvas�R���|�l���g

    private void Start()
    {
        playerPointPos = playerPoint.GetComponent<RectTransform>();

        miniMapCanvas = this.gameObject.GetComponent<Canvas>();
    }

    private void Update()
    {
        //�v���C���[�̌��ݒn���擾���AUI���W�ɒ���
        playerPos = new Vector3((-playerObject.transform.position.z + slideValueX) * 10, (playerObject.transform.position.x - slideValueY) *10, 0);

        //�v���C���[�|�C���g���v���C���[�̈ʒu��
        playerPointPos.localPosition = playerPos;

        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMapCanvas.enabled = !miniMapCanvas.enabled;
        }
    }

    public void MiniMapGenerate(int[,] getMapArray)
    {
        //�񎟔z��̏�����
        map = new int[getMapArray.GetLength(0), getMapArray.GetLength(1)];

        previousRoomNum = new int[getMapArray.GetLength(1)];

        // �z��̎d����
        // ��:0 ��:1 ����:2�`
        for (int i = 0; i < getMapArray.GetLength(0); i++) // �s�̃��[�v
        {
            for (int j = 0; j < getMapArray.GetLength(1); j++) // ��̃��[�v
            {
                Debug.Log("Value at [" + i + "," + j + "] is: " + getMapArray[i, j]);

                //�擾�����z����i�[�B
                //room = 2  road = 3
                map[i, j] = getMapArray[i, j];

                //�~�j�}�b�v�̍��W���i�[
                Vector2 minimapPos = new Vector2(-i, j);

                // �}�b�v�̑������ǂłȂ���΃^�C���𐶐�����
                if(map[i,j] == 2)
                {
                    if (!checkRoom)
                    {
                        checkRoom = true;

                        if(!roomEnd.Contains(minimapPos))
                        {
                            //�����̒[��ۑ�
                            roomEnd.Add(minimapPos);

                            //�����̐e��V�����쐬
                            GameObject roomParent = Instantiate(roomParentPrefab, Vector3.zero, Quaternion.identity);

                            //�e�I�u�W�F�N�g�̐ݒ�
                            roomParent.transform.SetParent(mapCanvasObj.transform, false);

                            //�����̐e���X�g�ɒǉ�
                            roomParentList.Add(roomParent);
                        }

                        //���X�g�̉��Ԗڂɂ��邩���擾
                        roomParentNum = roomEnd.IndexOf(minimapPos);
                    }

                    previousRoomNum[j] = map[i, j];

                    map[i, j] = roomParentNum + 2;

                    //���^�C���̌����ƒǉ�
                    //��������E�̓��`�F�b�N
                    if(map[i -1, j] == 1)
                    {
                        map[i - 1, j] = roomParentNum + 2;
                    }

                    //�������牺�̓��`�F�b�N
                    if (map[i, j - 1] == 1)
                    {
                        map[i, j - 1] = roomParentNum + 2;
                    }
                }
                else
                {
                    //�p�ς݂̕����̒[�̍��W��ʂȂƂ��ɂ��炵�Ă���
                    if (roomEnd.Contains(minimapPos))
                    {
                        roomEnd[roomEnd.IndexOf(minimapPos)] = new Vector2(10, 10);
                    }

                    if (map[i, j] == 3)
                    {
                        previousRoomNum[j] = map[i, j];

                        map[i, j] = 1;

                        checkRoom = false;

                        //���^�C���̌����ƒǉ�
                        //���������̓��`�F�b�N
                        if (map[i, j - 1] >= 2 && (map[i -1, j - 1] >= 2 || map[i + 1, j -1] >= 2))
                        {
                            map[i, j] = map[i, j - 1];
                        }

                        //�������獶�̓��`�F�b�N
                        if (map[i - 1, j] >= 2 && (map[i - 1, j - 1] >= 2 || map[i - 1, j + 1] >= 2))
                        {
                            map[i, j] = map[i - 1, j];
                        }
                    }
                    else
                    {
                        previousRoomNum[j] = map[i, j];

                        map[i, j] = 0;

                        checkRoom = false;
                    }
                }

            }

            for (int k = 0; k < roomEnd.Count; k++)
            {
                roomEnd[k] += new Vector2(-1, 0);
            }
        }

        //�^�C���̐���
        for (int i = 0; i < getMapArray.GetLength(0); i++) // �s�̃��[�v
        {
            for (int j = 0; j < getMapArray.GetLength(1); j++) // ��̃��[�v
            {
                if (map[i, j] != 0)
                {
                    //�^�C���̐���
                    //slideValue�Ń}�b�v�̈ʒu�𒲐�
                    GameObject mapTileObj = Instantiate(mapTilePrefab, new Vector3((-i + slideValueX) * 10, (j - slideValueY) * 10, 0), Quaternion.identity);

                    if (map[i, j] == 1)
                    {
                        //�e�I�u�W�F�N�g��MiniMapCanvas�ɐݒ�
                        mapTileObj.transform.SetParent(mapCanvasObj.transform, false);
                    }
                    else
                    {
                        //�e�I�u�W�F�N�g��RoomTile�ɐݒ�
                        mapTileObj.transform.SetParent(roomParentList[map[i, j] - 2].transform, false);
                    }
                }
            }
        }
    }

    public void StairOnMiniMap(int stairX, int stairZ)
    {
        //�K�i���~�j�}�b�v�ɔz�u
        stairPoint.transform.localPosition = new Vector3((-stairZ + slideValueX) * 10, (stairX - slideValueY) * 10, 0); ;

        //�K�i�ɐe�I�u�W�F�N�g��ݒ�
        stairPoint.transform.SetParent(roomParentList[map[stairZ, stairX] - 2].transform, false);

        Debug.Log("�擾���W > stairX ;" + stairX + "stairZ :" + stairZ + "/n �}�b�v���� :" +  map[stairX, stairZ]);

    }
}
