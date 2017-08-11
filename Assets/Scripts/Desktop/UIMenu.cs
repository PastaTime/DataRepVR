
public class UIMenu : Menu {

	void Start() {
		if (active)
			menuItems [index].Select ();
	}

	public override void OnActivation () {
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}
}
