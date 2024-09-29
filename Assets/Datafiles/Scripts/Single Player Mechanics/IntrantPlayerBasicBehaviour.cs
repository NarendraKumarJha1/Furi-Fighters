using System.Collections;
using UnityEngine;

public class IntrantPlayerBasicBehaviour : MonoBehaviour
{
    public void PutInPocket(GameObject _obj)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
       
    }
    public IEnumerator TelePortSelf(float duration, Transform _pos, Rigidbody _rb)
    {
        _rb.isKinematic = true;
        yield return new WaitForSeconds(duration);
        transform.position = _pos.position;
        yield return new WaitForSeconds(duration);
        _rb.isKinematic = false;
    }
}
