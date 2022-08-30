using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleGroundBox : MonoBehaviour
{
    [SerializeField] Vector3 targetMovePosition;
    private AudioSource audios;

    public bool isAssemble;
    private bool ended = false;

    [SerializeField] float speed = 1.5f;

    private void Awake() {
        ended = false;
        audios = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAssemble) {
            transform.position = Vector3.MoveTowards(transform.position, targetMovePosition, Time.deltaTime * speed);
            if(!ended && (transform.position - targetMovePosition).sqrMagnitude < 0.0001f) {
                ended = true;
                audios.Play();
            }
        }
    }
}
