
public class CentreMenu : Menu
{

	void Start() {
		sort ();
		if (active)
			menuItems [index].Select ();
	}

	public override void OnActivation () {
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}
}
