using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour {
    private uint _owner;
    private bool _inited;
    public float timeDestroy = 3f;
    public float speed = 3f;
    
    
    [Server]
    public void Init(uint owner, Vector3 playerPosition) 
    {
        _owner = owner;
        _inited = true;
        Invoke("DestroyBullet", timeDestroy);
        Debug.Log("Init");
        
    }
    
    void Update() {
        if (_inited && isServer) {
            transform.position = transform.position * speed;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (_inited && isServer && player) {
            if (player.netId != _owner) {
                player.ChangeHealthValue(player.Health - 1);
                NetworkServer.Destroy(gameObject);
                Debug.Log("damage");
            }
        }
        //NetworkServer.Destroy(gameObject);
        Debug.Log("destroy");
    }
}