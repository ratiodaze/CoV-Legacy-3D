public class ChargeInputHandler
{
    public float ChargeDuration { get; private set; }
    public bool IsCharging { get; private set; }
    public bool IsPreviewing { get; private set; }

    public float PreviewThreshold = 0.15f;
    public float MaxChargeTime = 2f;

    private System.Action onPreviewStart;
    private System.Action<float> onChargeRelease;

    public void BeginCharge(System.Action onPreviewStart, System.Action<float> onChargeRelease)
    {
        this.onPreviewStart = onPreviewStart;
        this.onChargeRelease = onChargeRelease;

        IsCharging = true;
        ChargeDuration = 0f;
        IsPreviewing = false;
    }

    public void Update(float deltaTime)
    {
        if (!IsCharging) return;

        ChargeDuration += deltaTime;

        if (!IsPreviewing && ChargeDuration >= PreviewThreshold)
        {
            IsPreviewing = true;
            onPreviewStart?.Invoke();
        }

        if (ChargeDuration >= MaxChargeTime)
        {
            ReleaseCharge(); // Auto-release on full charge
        }
    }

    public void ReleaseCharge()
    {
        if (!IsCharging) return;

        onChargeRelease?.Invoke(ChargeDuration);

        IsCharging = false;
        ChargeDuration = 0f;
        IsPreviewing = false;
    }

    public void CancelCharge()
    {
        IsCharging = false;
        ChargeDuration = 0f;
        IsPreviewing = false;
    }
}
