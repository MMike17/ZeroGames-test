/// <summary>Interface for zones containing interractions</summary>
public interface IInterractableZone
{
	public void OnZoneEntered ();
	public void OnZoneExit ();
	public void StartInterraction();
	public void StopInterraction();
}