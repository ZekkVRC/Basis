using UnityEngine;

public class BasisOverrideRotations : MonoBehaviour
{
    public BasisBoneControl Head;
    public BasisBoneControl Hips;
    public BasisBoneControl Chest;
    public BasisBoneControl Spine;

    public float DelayedHips = 6f;
    public float DelayedUpperChest = 6f;
    public float DelayedChest = 6f;
    public float DelayedSpine = 6f;
    public float RangeOfMotionBeforeTurn = 13;
    public float roty;
    public BasisLocalBoneDriver Driver;
    public bool HasEvent = false;
    /// <summary>
    /// this is used to simulate the rest of the body if no tracked is issued to said bonecontrol
    /// </summary>
    public void Simulate()
    {
        float DeltaTime = Time.deltaTime;

        roty = Head.OutGoingData.rotation.eulerAngles.y;
        Quaternion coreRotation = Quaternion.Euler(0, roty, 0);

        if (AngleCheck(coreRotation, Hips.OutGoingData.rotation, RangeOfMotionBeforeTurn))
        {
            // Slerp rotation for hips and upper body
            if (Hips.HasTracked == BasisHasTracked.HasNoTracker)
            {
                Hips.OutGoingData.rotation = SlerpYRotation(Hips.OutGoingData.rotation, coreRotation, DelayedHips * DeltaTime);
            }
            if (Chest.HasTracked == BasisHasTracked.HasNoTracker)
            {
                Chest.OutGoingData.rotation = SlerpYRotation(Chest.OutGoingData.rotation, coreRotation, DelayedChest * DeltaTime);
            }
            if (Spine.HasTracked == BasisHasTracked.HasNoTracker)
            {
                Spine.OutGoingData.rotation = SlerpYRotation(Spine.OutGoingData.rotation, coreRotation, DelayedSpine * DeltaTime);
            }
        }
    }

    private Quaternion SlerpYRotation(Quaternion from, Quaternion to, float t)
    {
        Vector3 fromEuler = from.eulerAngles;
        Vector3 toEuler = to.eulerAngles;
        Vector3 resultEuler = new Vector3(fromEuler.x, Mathf.LerpAngle(fromEuler.y, toEuler.y, t), fromEuler.z);
        return Quaternion.Euler(resultEuler);
    }
    public bool AngleCheck(Quaternion AngleA, Quaternion AngleB, float MaximumTolerance = 0.005f)
    {
        float Angle = Quaternion.Angle(AngleA, AngleB);
        bool AngleLargeEnough = Angle > MaximumTolerance;
        return AngleLargeEnough;
    }
    public void Initialize()
    {
        Driver = BasisLocalPlayer.Instance.LocalBoneDriver;
        if (Driver.FindBone(out Head, BasisBoneTrackedRole.Head))
        {
            Head.HasRigLayer = BasisHasRigLayer.HasRigLayer;
        }
        if (Driver.FindBone(out Hips, BasisBoneTrackedRole.Hips))
        {
            Hips.HasRigLayer = BasisHasRigLayer.HasRigLayer;
        }
        if (Driver.FindBone(out Chest, BasisBoneTrackedRole.Chest))
        {
            Chest.HasRigLayer = BasisHasRigLayer.HasRigLayer;
        }
        if (Driver.FindBone(out Spine, BasisBoneTrackedRole.Spine))
        {
            Spine.HasRigLayer = BasisHasRigLayer.HasRigLayer;
        }
        if (HasEvent == false)
        {
            Driver.OnPostSimulate += Simulate;
            HasEvent = true;
        }
    }
    public void OnDestroy()
    {
        if (HasEvent)
        {
            Driver.OnPostSimulate -= Simulate;
            HasEvent = false;
        }
    }
}