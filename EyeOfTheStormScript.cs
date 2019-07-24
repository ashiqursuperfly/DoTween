using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EyeOfTheStormScript : MonoBehaviour {
    private bool isPlayerCaught = false;
    private Transform player;
    float zPos, xPos, yPos;
    private void Update() {
        if (isPlayerCaught) {
            float circleSpeed = 9;
            float forwardSpeed = -1.4f; // Assuming negative Z is towards the camera
            float circleSize = 0.5f;
            float circleGrowSpeed = 1.75f;

            xPos = Mathf.Sin(Time.time * circleSpeed) * circleSize;
            yPos = Mathf.Cos(Time.time * circleSpeed) * circleSize;

            circleSize = circleSize - circleGrowSpeed;
            zPos += forwardSpeed * Time.deltaTime;
            player.localPosition = new Vector3(xPos, yPos, zPos);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.GetComponent<PlayerFeetScript>() != null || other.GetComponent<PlayerTopScript>() != null)
         && !PlayerParams.isOnShield && !PlayerParams.isOnJumpItemBoost) {
            //destroy player
            player = other.transform.parent;
            this.transform.DOPunchScale(Vector3.one * -0.1f, 1, 1, 0.5f);
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (other.GetComponent<PlayerFeetScript>() != null) other.GetComponent<BoxCollider2D>().enabled = false;
            if (other.GetComponent<PlayerTopScript>() != null) {
                player.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
            }
            player.SetParent(this.transform);
            player.DOScale(Vector3.one * 0.05f, 1.5f).OnComplete(
                () => {
                    GamePlayParameters.isGameover = true;
                    isPlayerCaught = false;
                }
            );

            isPlayerCaught = true;

        }
    }
}
