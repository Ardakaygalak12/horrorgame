using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour {
    public Transform player;
    public float detectionRange = 10f;
    public float patrolRadius = 10f;
    public float moveSpeed = 3.5f;  // Tek bir hız değeri

    private NavMeshAgent agent;
    private Vector3 randomDestination;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            Debug.LogError("NavMeshAgent bileşeni bulunamadı!");
            return;
        }

        // NavMeshAgent ayarlarını güncelle
        agent.speed = moveSpeed;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false;

        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.transform;
            } else {
                Debug.LogError("Player objesi bulunamadı!");
            }
        }

        SetRandomDestination();
        Debug.Log("EnemyAI başlatıldı");
    }

    void Update() {
        if (player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Oyuncuya uzaklık: " + distanceToPlayer);

        if (distanceToPlayer <= detectionRange) {
            ChasePlayer();
        } else {
            Patrol();
        }
    }

    void ChasePlayer() {
        Debug.Log("Oyuncu takip ediliyor");
        agent.SetDestination(player.position);
    }

    void Patrol() {
        if (agent.remainingDistance < 0.5f) {
            SetRandomDestination();
        }
    }

    void SetRandomDestination() {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        randomDestination = hit.position;
        agent.SetDestination(randomDestination);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Oyuncuya çarpıldı!");
            RestartScene();
        }
    }

    void RestartScene() {
        Debug.Log("Sahne yeniden başlatılıyor...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}