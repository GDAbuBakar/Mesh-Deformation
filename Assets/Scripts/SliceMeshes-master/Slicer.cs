using UnityEngine;
using EzySlice;
public class Slicer : MonoBehaviour
{
    public Material materialAfterSlice;
    public LayerMask sliceMask;
    public bool isTouched;

    private readonly string hullName = "Lower_Hull";

    private void Update()
    {
        if (isTouched == true)
        {
            isTouched = false;

            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);
            print(objectsToBeSliced.Length);

            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {
                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);

                //if (slicedObject != null)
                //{
                    GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                    GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                    upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                    lowerHullGameobject.transform.position = objectToBeSliced.transform.position;

                    MakeItPhysical(upperHullGameobject);
                    MakeItPhysical(lowerHullGameobject);

                    Destroy(objectToBeSliced.gameObject);
                //}
            }
        }
    }

    private void MakeItPhysical(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        Rigidbody rb = obj.AddComponent<Rigidbody>();

        if (obj.name == hullName)
        {
            rb.isKinematic = true;
        }
        else
        { 
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddExplosionForce(100, rb.transform.position, 20);
        }


        obj.layer = 8;
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }
}
