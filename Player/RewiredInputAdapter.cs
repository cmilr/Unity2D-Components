using Rewired;

public class RewiredInputAdapter : BaseBehaviour
{
	private Player playerControls;
	private ICreatureController creature;

	void Start()
	{
		playerControls = ReInput.players.GetPlayer(0);
		creature = GetComponent<ICreatureController>();
	}

	void Update()
	{
		float h;

		h = playerControls.GetAxisRaw("Move Horizontal");

		if (h > 0) {
			creature.MoveRight();
		}

		if (h < 0) {
			creature.MoveLeft();
		}

		if (playerControls.GetButtonDown("Jump")) {
			creature.Jump();
		}

		if (playerControls.GetButtonDown("Attack")) {
			creature.Attack();
		}

		if (playerControls.GetButtonDown("Next Weapon")) {
			EventKit.Broadcast<int>("switch weapon", RIGHT);
		}
	}
}
