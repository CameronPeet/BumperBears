using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ContactHandler : MonoBehaviour {

    [Header("Hit | Tweens")]
    public Ease ColorEase;
    public Color HitColor;
    public float Intensity;
    public float TweenLength;

    [Header("Hit | Strengths")]
    public float AddedForceMultiplier = 1000.0f;

    [Header("Hit | Audio Visual")]
    public GameObject HitEffect;
    public AudioClip HitSound;
    public AudioSource AudioSource;

    private Rigidbody m_rigidBody;
    float PreviousVelocity = 0.0f;

    static float pitch = 1.0f;
    static float startPitch;

    bool contactedThisFrame;

    List<string> bearsHitBy;

    Color cacheColor;

	// Use this for initialization
	void Start () {
        cacheColor = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.color;
        AudioSource = GetComponent<AudioSource>();
        startPitch = AudioSource.pitch;
        bearsHitBy = new List<string>();

        m_rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        contactedThisFrame = false;

        PreviousVelocity = Mathf.Abs(m_rigidBody.velocity.magnitude);
	}

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (contact.otherCollider.gameObject.tag == "BumperBear")
        {
            if (bearsHitBy.Contains(contact.otherCollider.gameObject.name))
                return;

            if (contactedThisFrame)
                return;

            print(gameObject.name + " is being hit by" + collision.gameObject.name);

            ContactHandler handler = collision.gameObject.GetComponent<ContactHandler>();
            float otherPrevVelocity = handler.GetPreviousVelocity();

            Quaternion rot = new Quaternion(0, 0, 0, 0);
            Vector3 pos = contact.point;
            GameObject obj = Instantiate(HitEffect, pos, rot, transform);

            AudioSource.pitch = Random.Range(startPitch - 0.5f, startPitch + 0.5f);
            AudioSource.PlayOneShot(HitSound);

            Vector3 v1 = collision.gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 v2 = GetComponent<Rigidbody>().velocity;

            float side = otherPrevVelocity - PreviousVelocity;

            //~~~~A check incase this script is run before the other Contact Handler~~~
            //Check if the one that got hit harder is the "Other bear"
            //(We already ignore bears added to our contact list for this frame / delay.
            if (side < 0.0f)
            {


                Vector3 forceDir = -contact.normal;
                Vector3 force = forceDir * ((AddedForceMultiplier * 10) * Mathf.Abs(PreviousVelocity / 30.0f));
                Vector3 forcePos = -contact.point;


                foreach (Orb orb in collision.gameObject.GetComponentsInChildren<Orb>())
                {
                    orb.Drop();
                }

                collision.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(force, forcePos);
                collision.gameObject.GetComponent<Animator>().Play("Impact");
                collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOComplete();
                collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOColor(HitColor, TweenLength).From().SetEase(ColorEase);

                print("Velocity nulled for " + gameObject.name);

                GetComponent<Rigidbody>().angularVelocity *= 0.1f;
                GetComponent<Rigidbody>().velocity *= 0.1f;

                if (handler != null)
                {
                    handler.contactedThisFrame = true;

                }
            }

            // The bear this script is attached to is the one being "Hit".
            else if (side > 0.0f)
            {
                Vector3 forceDir = contact.normal;
                Vector3 force = forceDir * ((AddedForceMultiplier * 10) * (Mathf.Abs(PreviousVelocity / 30.0f)));
                Vector3 forcePos = -contact.point;

                GetComponent<Rigidbody>().AddForceAtPosition(force, forcePos);
                GetComponent<Animator>().Play("Impact");
                GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOComplete();
                GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOColor(HitColor, TweenLength).From().SetEase(ColorEase);


                print("Side > 0 : Velocity nulled for " + collision.gameObject.name);
                collision.gameObject.GetComponent<Rigidbody>().angularVelocity *= 0.1f;
                collision.gameObject.GetComponent<Rigidbody>().velocity *= 0.1f;

                foreach (Orb orb in gameObject.GetComponentsInChildren<Orb>())
                {
                    orb.Drop();
                }

                contactedThisFrame = true;
            }

            StartCoroutine(PreventHitBy(collision.gameObject.name, 1.0f));
            handler.StartCoroutine(handler.PreventHitBy(gameObject.name, 1.0f));

        }

        else if (collision.gameObject.tag == "Traps")
        {

            Quaternion rot = new Quaternion(0, 0, 0, 0);
            Vector3 pos = contact.point;
            GameObject obj = Instantiate(HitEffect, pos, rot, transform);

            AudioSource.pitch = Random.Range(startPitch - 0.5f, startPitch + 0.5f);
            AudioSource.PlayOneShot(HitSound);

            Vector3 forceDir = collision.transform.forward + (transform.up * 0.1f);
            Vector3 force = forceDir * ((AddedForceMultiplier * 10));
            Vector3 forcePos = contact.point;


            foreach (Orb orb in collision.gameObject.GetComponentsInChildren<Orb>())
            {
                orb.Drop();
            }

            GetComponent<Rigidbody>().AddForceAtPosition(force, forcePos);
            GetComponent<Animator>().Play("Impact");
            GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOComplete();
            GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.DOColor(HitColor, TweenLength).From().SetEase(ColorEase);

            GetComponent<Rigidbody>().angularVelocity *= 0.1f;
            GetComponent<Rigidbody>().velocity *= 0.1f;
        }
    }


    public float GetPreviousVelocity()
    {
        return PreviousVelocity;
    }


    private IEnumerator PreventHitBy(string name, float time)
    {
        bearsHitBy.Add(name);

        print(gameObject.name + " blocking " + name);

        yield return new WaitForSeconds(time);

        bearsHitBy.Remove(name);
    }

    private void OnDestroy()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.color = cacheColor;
    }
}
